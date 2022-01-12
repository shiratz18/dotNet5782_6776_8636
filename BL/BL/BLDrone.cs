using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        #region Add drone
        /// <summary>
        /// Add a drone to the list of drone in data
        /// </summary>
        /// <param name="drone">the drone to add</param>
        /// <param name="stationNum">the station ID in which to put the drone</param>
        public void AddDrone(Drone drone, int stationNum)
        {
            if (drone.Id < 1000 || drone.Id > 9999)
                throw new InvalidNumberException("Drone ID must be 4 digits.");

            if (drone.Model.Length < 5)
            {
                throw new WrongFormatException("Drone model must be 5 characters.");
            }

            DO.Drone temp = new DO.Drone() //copy the drone to a temporary DAL drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (DO.WeightCategories)drone.MaxWeight,
                Active = true
            };

            Station s;
            try
            {
                //getting the station, will throw an exception if the station does not exist
                s = GetStation(stationNum);
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
            if (s.AvailableChargeSlots < 1) //checking that the station has available charging slots
                throw new NoAvailableChargeSlotsException($"Station {stationNum} has no available charging slots.");
            if (!s.Active)
                throw new NoIDException($"Station {stationNum} is no longer active.");

            try //trying to add the drone to the list in data layer
            {
                Data.AddDrone(temp);
            }
            catch (DO.DoubleIDException ex)
            {
                throw new DoubleIDException(ex.Message);
            }

            Drones.Add(new ListDrone() //adding the drone to the list of drones in BL
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Battery = R.Next(20, 41),
                Status = DroneStatuses.Maintenance,
                CurrentLocation = s.Location,
                ParcelId = 0,
                Active = true
            });

            DO.DroneCharge charge = new DO.DroneCharge()
            {
                DroneId = drone.Id,
                StationId = stationNum
            };
            Data.AddDroneCharge(charge); //adding drone charge to the list in data source

            UpdateStationChargingSlots(stationNum, s.AvailableChargeSlots - 1); //update the station to have one less charging slots           
        }
        #endregion

        #region Get drone
        /// <summary>
        /// Returns a drone according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object of the drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            if (!Drones.Exists(d => d.Id == id)) //if the drone does not exist in the list od drones in bll
                throw new NoIDException($"Drone {id} doesn't exist");

            ListDrone temp = Drones.Find(d => d.Id == id); //find the drone in the list of drones

            Drone drone = new Drone()
            {
                Id = temp.Id,
                Model = temp.Model,
                MaxWeight = temp.MaxWeight,
                Battery = temp.Battery,
                Status = temp.Status,
                InShipping = new ParcelInShipping(),
                CurrentLocation = temp.CurrentLocation
            };

            if (temp.ParcelId != 0)
            {
                drone.InShipping = getParcelInShipping(temp.ParcelId);
                if (drone.InShipping.DeliveryDistance == 0)
                    drone.InShipping.DeliveryDistance = getDistance(drone.CurrentLocation, drone.InShipping.PickUpLocation);
            }

            return drone;
        }
        #endregion

        #region Get drone list
        /// <summary>
        /// Returns the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ListDrone> GetDroneList(WeightCategories? wc = null, DroneStatuses? ds = null)
        {
            if (Drones.Count == 0)
                throw new EmptyListException("No drones to display.");

            if (wc != null) //if the weight filter is not null
            {
                if (ds != null) //if also the status filter is not null then return a filtered list according to both of thme
                {
                    return from d in Drones
                           where d.Active && d.MaxWeight == wc && d.Status == ds
                           select d;
                }
                else //otherwise return a filtered list only according to the weight
                {
                    return from d in Drones
                           where d.Active && d.MaxWeight == wc
                           select d;
                }
            }
            else if (ds != null) //otherwise if only the status isnt null return a filtered list according to the status
            {
                return from d in Drones
                       where d.Active && d.Status == ds
                       select d;
            }
            else //otherwise return the entire list of active drones
            {
                return from d in Drones
                       where d.Active
                       select d;
            }
        }
        #endregion

        #region Update drone
        /// <summary>
        /// Update a drone
        /// </summary>
        /// <param name="drone">The updates drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            if (!Drones.Exists(x => x.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} does not exist.");

            ListDrone ld = Drones.Find(x => x.Id == drone.Id);

            if (!ld.Active)
                throw new NoIDException($"Drone {drone.Id} does not exist.");

            ld.Id = drone.Id;
            ld.Model = drone.Model;
            ld.MaxWeight = drone.MaxWeight;
            ld.Battery = drone.Battery;
            ld.Status = drone.Status;
            ld.CurrentLocation = new Location() { Longitude = drone.CurrentLocation.Longitude, Latitude = drone.CurrentLocation.Latitude };
            ld.ParcelId = drone.InShipping.Id;

            DO.Drone d = new DO.Drone()
            {
                Id = drone.Id,
                MaxWeight = (DO.WeightCategories)drone.MaxWeight,
                Model = drone.Model,
                Active = true
            };
            lock (Data)
            {
                Data.UpdateDrone(d); //update the drone in data layer
            }
        }
        #endregion

        #region Update drone model
        /// <summary>
        /// Update the name of the drone
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <param name="name">the new name</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneName(int id, string name)
        {
            lock (Data)
            {
                if (!Drones.Exists(d => d.Id == id))
                    throw new NoIDException($"Drone {id} does not exist.");

                var drone = Drones.Find(d => d.Id == id);

                if (!drone.Active)
                    throw new NoIDException($"Drone {id} does not exist.");

                drone.Model = name;

                try
                {
                    Data.EditDroneModel(id, name); //update the drone in data layer
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
        }
        #endregion

        #region Charge
        /// <summary>
        /// Send a drone to charge
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeDrone(int id)
        {
            lock (Data)
            {
                ListDrone d = Drones.Find(x => x.Id == id); //finding the drone in the list of bll

                //throw an exception if the drone wasnt found
                if (d == null)
                    throw new NoIDException($"Drone {id} does not exist.");

                //throw an exception if d was deleted
                if (!d.Active)
                    throw new NoIDException($"Drone {id} does not exist.");

                //throw an exception if the drone is not available
                if (d.Status != DroneStatuses.Available)
                    throw new DroneStateException($"Drone {id} is not available.");

                IEnumerable<Station> stations;
                try
                {
                    stations = getListOfAvailableChargeSlotsStations(); //get the list of stations with availabe chargers
                }
                catch (EmptyListException ex)
                {
                    throw new EmptyListException(ex.Message); //throw an exception if no station has available charge slots
                }

                //get the station that is nearest to the drone, among the stations that have available charge slots
                Station s = new Station();
                s = (from st in stations
                     orderby getDistance(d.CurrentLocation, st.Location)
                     select st).First();

                //throw an exception if there is not enough battery to get to the station
                if (d.Battery < AvailableConsumption * getDistance(d.CurrentLocation, s.Location))
                    throw new NoBatteryException($"Drone {d.Id} does not have enough battery to get to a charging station.");

                d.Battery -= AvailableConsumption * getDistance(d.CurrentLocation, s.Location); //decreasing from the battery the percaentage it takes to get to the station
                d.CurrentLocation = s.Location; //changing the location to be the station
                d.Status = DroneStatuses.Maintenance; //change the status to be in maintenance

                DO.Drone drone = new DO.Drone()
                {
                    Id = d.Id,
                    Model = d.Model,
                    MaxWeight = (DO.WeightCategories)d.MaxWeight,
                    Active = true
                };
                Data.UpdateDrone(drone); //update the drone in dal

                DO.Station station = new DO.Station()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Latitude = s.Location.Latitude,
                    Longitude = s.Location.Longitude,
                    ChargeSlots = s.AvailableChargeSlots - 1,
                    Active = s.Active
                };
                Data.UpdateStation(station);

                DO.DroneCharge dc = new DO.DroneCharge()
                {
                    DroneId = drone.Id,
                    StationId = s.Id,
                    ChargingBegin = DateTime.Now
                };
                Data.AddDroneCharge(dc); //add the drone charge in the data layer
            }
        }
        #endregion

        #region Release charge
        /// <summary>
        /// Releases a drone from charging
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <param name="time">The time it has charged</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneCharge(int id)
        {
            lock (Data)
            {
                if (!Drones.Exists(d => d.Id == id))
                    throw new NoIDException($"Drone {id} does not exist.");

                ListDrone d = Drones.Find(d => d.Id == id);

                if (!d.Active)
                    throw new NoIDException($"Drone {id} does not exist.");

                if (d.Status != DroneStatuses.Maintenance) //if the drone isnt charging throw an exception
                    throw new DroneStateException($"Drone {id} is not currently charging.");

                DO.DroneCharge dc = Data.GetDroneCharge(d.Id);
                TimeSpan time = DateTime.Now - dc.ChargingBegin;

                d.Battery = d.Battery + (time.Seconds * DroneChargingRate); //adding the battery the drone has charged
                if (d.Battery > 100)
                    d.Battery = 100;
                d.Status = DroneStatuses.Available; //update the status to be available

                DO.Station st = Data.GetStation(dc.StationId);
                st.ChargeSlots += 1;
                Data.UpdateStation(st);

                Data.RemoveDroneCharge(dc); //remove the drone charge in the data layer
            }
        }
        #endregion

        #region Assign drone to parcel
        /// <summary>
        /// Assign a drone to a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignDroneToParcel(int id)
        {
            ListDrone drone = Drones.Find(d => d.Id == id);

            //throw exception if drone was not found or is not active
            if (drone == null || !drone.Active)
                throw new NoIDException($"Drone {id} does not exist.");

            //checking that the drone is available
            if (drone.Status != DroneStatuses.Available)
                throw new DroneStateException($"Drone {id} is currently unavailable for shipping.");

            //getting all parcels that are not assigned to a drone (in form of ParcelInShipping)
            IEnumerable<ParcelInShipping> parcels;
            try
            {
                parcels = getListOfNoDroneParcelsInShipping();
            }
            catch (EmptyListException ex)
            {
                throw new EmptyListException(ex.Message);
            }

            List<ParcelInShipping> highPriority = new List<ParcelInShipping>();
            List<ParcelInShipping> mediumPriority = new List<ParcelInShipping>();
            List<ParcelInShipping> lowPriority = new List<ParcelInShipping>();

            //deviding the parcels into 3 lists according to priority,
            foreach (ParcelInShipping p in parcels)
            {
                //checking that the drone could actually deliver them according to weight, distance and battery
                if (p.Weight <= drone.MaxWeight && drone.Battery >= ShippingConsumption[(int)p.Weight] * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                {
                    if (p.Priority == Priorities.Urgent) //adding to the list of urgent parcels
                        highPriority.Add(p);

                    else if (p.Priority == Priorities.Express) //adding to the list of less uregent parcels
                        mediumPriority.Add(p);

                    else if (p.Priority == Priorities.Regular) //adding to the list of low ptiority parcels
                        lowPriority.Add(p);
                }
            }

            List<ParcelInShipping> heavy = new List<ParcelInShipping>();
            List<ParcelInShipping> medium = new List<ParcelInShipping>();
            List<ParcelInShipping> light = new List<ParcelInShipping>();

            //dividng the list into 3 lists according to weight
            if (highPriority.Count > 0) //if there is at least one high priority parcel that the drone could carry
            {
                //dividing the list according to weight categories
                foreach (ParcelInShipping p in highPriority)
                {
                    if (p.Weight == WeightCategories.Heavy)
                        heavy.Add(p);
                    else if (p.Weight == WeightCategories.Medium)
                        medium.Add(p);
                    else
                        light.Add(p);
                }
            }
            else if (mediumPriority.Count > 0) //otherwise if there is at least one medium priotity parcel that the drone can carry
            {
                //dividing the list according to weight categories
                foreach (ParcelInShipping p in mediumPriority)
                {
                    if (p.Weight == WeightCategories.Heavy)
                        heavy.Add(p);
                    else if (p.Weight == WeightCategories.Medium)
                        medium.Add(p);
                    else
                        light.Add(p);
                }
            }
            else if (lowPriority.Count > 0) //otherwise if there is at least one low priotity parcel that the drone can carry
            {
                //dividing the list according to weight categories
                foreach (ParcelInShipping p in lowPriority)
                {
                    if (p.Weight == WeightCategories.Heavy)
                        heavy.Add(p);
                    else if (p.Weight == WeightCategories.Medium)
                        medium.Add(p);
                    else
                        light.Add(p);
                }
            }
            else //otherwise the drone cannot carry any parcel, throw an exception
                throw new NoBatteryException($"Drone {id} cannot currently deliver any parcel.");

            ParcelInShipping parcel;

            //finding the nearest parcel among the possible parcels
            if (heavy.Count > 0) //if there is at least one heavy parcel the drone could carry
            {
                parcel = closestParcel(id, heavy); //finding the closest parcel to the drone among the parcels
            }
            else if (medium.Count > 0) //otherwise if there is at least one medium weight parcel the drone could carry
            {
                parcel = closestParcel(id, medium); //finding the closest parcel to the drone among the parcels
            }
            else if (light.Count > 0) //otherwise there is only a lowweight parcel the drone could carry
            {
                parcel = closestParcel(id, light); //finding the closest parcel to the drone among the parcels
            }
            else
                throw new NoBatteryException($"Drone {id} cannot currently deliver any parcel.");

            drone.Status = DroneStatuses.Shipping; //updating the status of the drone to be in shipping
            drone.ParcelId = parcel.Id; //updating the parcel the drone carries
            lock (Data)
            {
                Data.AssignDroneToParcel(parcel.Id, drone.Id);
            }

        }
        #endregion

        #region Drone pick up parcel
        /// <summary>
        /// Update that a drone picked up a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DronePickUp(int id)
        {
            ListDrone d = Drones.Find(x => x.Id == id);

            if (d == null || !d.Active)
                throw new NoIDException($"Drone {id} does not exist.");

            Drone drone = GetDrone(id);

            if (drone.InShipping.Id == 0) //if the drone does not have a parcel 
                throw new DroneStateException($"Drone {id} does not have a parcel assigned.");

            if (drone.InShipping.IsPickedUp) //if the parcel has already been picked up
                throw new DroneStateException($"Drone {id} cannot pick up the parcel since it has already been picked up.");

            ParcelInShipping p = drone.InShipping;

            d.Battery -= AvailableConsumption * getDistance(drone.CurrentLocation, p.PickUpLocation);
            d.CurrentLocation = p.PickUpLocation;
            lock (Data)
            {
                Data.ParcelPickUp(p.Id); //updating the parcel in dll
            }
        }
        #endregion

        #region Drone deliver parcel
        /// <summary>
        /// Update that a drone delivered a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneDeliver(int id)
        {
            ListDrone d = Drones.Find(x => x.Id == id);

            if (d == null || !d.Active)
                throw new NoIDException($"Drone {id} does not exist.");

            Drone drone = GetDrone(id);

            if (drone.InShipping.Id == 0) //if the drone does not have a parcel 
            {
                throw new DroneStateException($"Drone {id} does not have a parcel assigned.");
            }
            if (!drone.InShipping.IsPickedUp) //if the parcel has not already been picked up
            {
                throw new DroneStateException($"Drone {id} cannot deliver the parcel since it has not been picked up.");
            }

            ParcelInShipping p = drone.InShipping;

            d.Battery -= ShippingConsumption[(int)p.Weight] * p.DeliveryDistance;
            d.CurrentLocation = p.DeliveryLocation;
            d.Status = DroneStatuses.Available;
            d.ParcelId = 0;
            lock (Data)
            {
                Data.ParcelDelivered(p.Id);
            }
        }
        #endregion

        #region Remove drone
        /// <summary>
        /// Remove a drone from the list 
        /// </summary>
        /// <param name="drone">The drone to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(int id)
        {
            ListDrone d = Drones.Find(x => x.Id == id);
            if (d.Status == DroneStatuses.Shipping)
                throw new DroneStateException($"Cannot deactivate drone {id} since it is currently in shipping. Try again later.");

            lock (Data)
            {
                try
                {
                    DO.Drone drone = new DO.Drone()
                    {
                        Id = id,
                        Model = d.Model,
                        MaxWeight = (DO.WeightCategories)d.MaxWeight
                    };
                    Data.RemoveDrone(drone); //removing in data

                    //deactivating the drone
                    d.Active = false;
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
        }
        #endregion
    }
}


