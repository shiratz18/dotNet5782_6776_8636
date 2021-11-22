using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// Add a drone to the list of drone in data
        /// </summary>
        /// <param name="drone">the drone to add</param>
        /// <param name="stationNum">the station ID in which to put the drone</param>
        public void AddDrone(Drone drone, int stationNum)
        {
            IDAL.DO.Drone temp = new IDAL.DO.Drone() //copy the drone to a temporary DAL drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };

            Station s = GetStation(stationNum); //getting the station
            if (s.AvailableChargeSlots < 1) //checking that the station has available charging slots
            {
                throw new NoAvailableChargeSlotsException($"Station {stationNum} has no available charging slots.");
            }

            try //trying to add the drone to the list in data layer
            {
                data.AddDrone(temp);
            }
            catch (IDAL.DO.DoubleIDException ex)
            {
                throw new DoubleIDException(ex.Message);
            }

            ListDrone dr = new ListDrone() //adding the drone to the list of drones in BL
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Battery = R.Next(20, 41),
                Status = DroneStatuses.Maintenance,
                CurrentLocation = s.Location,
                ParcelId = 0
            };

            IDAL.DO.DroneCharge charge = new IDAL.DO.DroneCharge()
            {
                DroneId = drone.Id,
                StationId = stationNum
            };
            data.AddDroneCharge(charge); //adding drone charge to the list in data source

            UpdateStationChargingSlots(stationNum, s.AvailableChargeSlots - 1); //update the station to have one less charging slots           
        }

        /// <summary>
        /// Update a drone
        /// </summary>
        /// <param name="drone">The updates drone</param>
        public void UpdateDrone(Drone drone)
        {
            if (!Drones.Exists(x => x.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} does not exist.");

            ListDrone ld = new ListDrone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Battery = drone.Battery,
                Status = drone.Status,
                CurrentLocation = drone.CurrentLocation,
                ParcelId = drone.InShipping.Id
            };

            Drones[Drones.FindIndex(x => x.Id == drone.Id)] = ld; //updating the drone in the list of drones in bll

            IDAL.DO.Drone d = new IDAL.DO.Drone()
            {
                Id = drone.Id,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                Model = drone.Model
            };

            data.UpdateDrone(d); //update the drone in data layer
        }

        /// <summary>
        /// Update the name of the drone
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <param name="name">the new name</param>
        public void UpdateDroneName(int id, string name)
        {
            try //try to get the drone from the list
            {
                IDAL.DO.Drone temp = data.GetDrone(id);

                temp.Model = name;
                data.UpdateDrone(temp); //sending the update to the data layer

                Drones[Drones.FindIndex(x => x.Id == id)].Model = name; //updtating in the list of drones of bll
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Send a drone to charge
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void ChargeDrone(int id)
        {
            if (!Drones.Exists(x => x.Id == id)) //checking if the drone exists
                throw new NoIDException($"Drone {id} does not exist.");

            ListDrone d = Drones.Find(x => x.Id == id); //finding the drone in the list of bll

            if (d.Status != DroneStatuses.Available)
                throw new DroneStateException($"Drone {id} is not available.");

            Station s = GetStation(NearestStationId(d.CurrentLocation)); //the station that is nearest to the drone

            while (s.AvailableChargeSlots == 0)
            {

            }

            if (d.Battery < AvailableConsumption * getDistance(d.CurrentLocation, s.Location)) //if there is not enough battery to get to the station
            {
                throw new NoBatteryException($"Drone {d.Id} does not have enough battery to get to a charging station.");
            }

            d.Battery -= AvailableConsumption * getDistance(d.CurrentLocation, s.Location); //decreasing from the battery the percaentage it takes to get to the station
            d.CurrentLocation = s.Location; //changing the location to be the station
            d.Status = DroneStatuses.Maintenance; //change the status to be in maintenance

            Drone drone = new Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight,
                Battery = d.Battery,
                Status = d.Status,
                CurrentLocation = d.CurrentLocation
            };
            UpdateDrone(drone); //update the drone

            UpdateStationChargingSlots(s.Id, s.AvailableChargeSlots - 1); //update the station charge slots to one less

            IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge()
            {
                DroneId = drone.Id,
                StationId = s.Id
            };
            data.AddDroneCharge(dc); //add the drone charge in the data layer
        }

        /// <summary>
        /// Releases a drone from charging
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <param name="time">The time it has charged</param>
        public void ReleaseDroneCharge(int id, double time)
        {
            Drone d;
            try
            {
                d = GetDrone(id); //getting the drone, will throw exception if drone doesnt exist
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            if (d.Status != DroneStatuses.Maintenance) //if the drone isnt charging throw an exception
                throw new DroneStateException($"Drone {id} is not currently charging.");

            d.Battery += time * DroneChargingRate; //adding the battery the drone has charged
            d.Status = DroneStatuses.Available; //update the status to be available
            UpdateDrone(d); //update the drone

            Station s = GetStationByLocation(d.CurrentLocation);
            UpdateStationChargingSlots(s.Id, s.AvailableChargeSlots + 1); //update the available charge slots to have one moe

            IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge()
            {
                DroneId = d.Id,
                StationId = s.Id
            };
            data.RemoveDroneCharge(dc); //remove the drone charge in the data layer
        }

        /// <summary>
        /// Assign a drone to a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void AssignDroneToParcel(int id)
        {
            Drone drone;
            try
            {
                drone = GetDrone(id); //try to get the drone, will throw an exception if ID doesnt exist
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            if (drone.Status != DroneStatuses.Available) //checking that the drone is available
                throw new DroneStateException($"Drone {id} is currently unavailable for shipping.");

            IEnumerable<Parcel> parcels = getListOfParcels(); //getting the list of parcels

            List<Parcel> highPriority = new List<Parcel>();
            List<Parcel> mediumPriority = new List<Parcel>();
            List<Parcel> lowPriority = new List<Parcel>();

            //deviding the parcels into 3 lists according to priority, and making sure they are not heavy enough and that they have enough battery to complete the delivery
            foreach (Parcel p in parcels)
            {
                if (p.AssignedDrone.Id == 0) //if the parcel does not yet have a drone
                {
                    if (p.Priority == Priorities.Urgent && p.Weight <= drone.MaxWeight)
                    {
                        switch (p.Weight)
                        {
                            case WeightCategories.Heavy:
                                if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    highPriority.Add(p);
                                break;
                            case WeightCategories.Medium:
                                if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    highPriority.Add(p);
                                break;
                            case WeightCategories.Light:
                                if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    highPriority.Add(p);
                                break;
                        }
                    }
                    else if (p.Priority == Priorities.Express && p.Weight <= drone.MaxWeight)
                    {
                        switch (p.Weight)
                        {
                            case WeightCategories.Heavy:
                                if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    mediumPriority.Add(p);
                                break;
                            case WeightCategories.Medium:
                                if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    mediumPriority.Add(p);
                                break;
                            case WeightCategories.Light:
                                if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    mediumPriority.Add(p);
                                break;
                        }
                    }
                    else if (p.Priority == Priorities.Regular && p.Weight <= drone.MaxWeight)
                    {
                        switch (p.Weight)
                        {
                            case WeightCategories.Heavy:
                                if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    lowPriority.Add(p);
                                break;
                            case WeightCategories.Medium:
                                if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    lowPriority.Add(p);
                                break;
                            case WeightCategories.Light:
                                if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id)) //if there is enough battery to do the delivery
                                    lowPriority.Add(p);
                                break;
                        }
                    }
                }
            }

            List<Parcel> heavy = new List<Parcel>();
            List<Parcel> medium = new List<Parcel>();
            List<Parcel> light = new List<Parcel>();

            if (highPriority.Count > 0) //if there is at least one high priority parcel that the drone could carry
            {
                foreach (Parcel p in highPriority)
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
                foreach (Parcel p in highPriority)
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
                foreach (Parcel p in highPriority)
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
                throw new DroneStateException($"Drone {id} cannot currently deliver any parcel.");

            Parcel parcel;

            //finding the nearest parcel among the possible parcels
            if (heavy.Count > 0) //if there is at least one heavy parcel the drone could carry
            {
                parcel = closestParcel(id, heavy); //finding the closest parcel to the drone among the parcels
            }
            else if (medium.Count > 0) //otherwise if there is at least one medium weight parcel the drone could carry
            {
                parcel = closestParcel(id, medium); //finding the closest parcel to the drone among the parcels
            }
            else //otherwise there is only a lowweight parcel the drone could carry
            {
                parcel = closestParcel(id, light); //finding the closest parcel to the drone among the parcels
            }

            drone.Status = DroneStatuses.Shipping; //updating the status of the drone to be in shipping
            drone.InShipping = parcel; //updating the parcel the drone carries
            UpdateDrone(drone); //update the drone

            parcel.AssignedDrone = new DroneOfParcel() { Id = drone.Id, Battery = drone.Battery, CurrentLocation = drone.CurrentLocation };
            parcel.Requested = DateTime.Now;
            UpdateParcel(parcel); //update the parcel
        }

        /// <summary>
        /// Update that a drone picked up a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DronePickUp(int id)
        {
            Drone drone = GetDrone(id);

            if (drone.InShipping.Id != 0) //if the drone does not have a parcel 
            {
                throw new DroneStateException($"Drone {id} does not have a parcel assigned.");
            }
            if(drone.InShipping.PickedUp != DateTime.MinValue) //if the parcel has already been picked up
            {
                throw new DroneStateException($"Drone {id} cannot pick up the parcel since it has already been picked up.");
            }

            Parcel p = drone.InShipping;

            switch(p.Weight)
            {
                //update the battery according to weight and distance
                case WeightCategories.Heavy:
                    drone.Battery -= HeavyWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Sender.Id).Location);
                    break;
                case WeightCategories.Medium:
                    drone.Battery -= MediumWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Sender.Id).Location);
                    break;
                case WeightCategories.Light:
                    drone.Battery -= LightWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Sender.Id).Location);
                    break;
            }
            drone.CurrentLocation = GetCustomer(p.Sender.Id).Location; //changing the location of the drone to the location of the sender
            UpdateDrone(drone); //update the drone

            p.PickedUp = DateTime.Now; //update the pick up time to be now
            UpdateParcel(p); //update the parcel
        }

        /// <summary>
        /// Update that a drone delivered a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DroneDeliver(int id)
        {
            Drone drone = GetDrone(id);

            if (drone.InShipping.Id != 0) //if the drone does not have a parcel 
            {
                throw new DroneStateException($"Drone {id} does not have a parcel assigned.");
            }
            if (drone.InShipping.PickedUp == DateTime.MinValue) //if the parcel has not already been picked up
            {
                throw new DroneStateException($"Drone {id} cannot deliver the parcel since it has not been picked up.");
            }

            Parcel p = drone.InShipping;

            switch (p.Weight)
            {
                //update the battery according to weight and distance
                case WeightCategories.Heavy:
                    drone.Battery -= HeavyWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Target.Id).Location);
                    break;
                case WeightCategories.Medium:
                    drone.Battery -= MediumWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Target.Id).Location);
                    break;
                case WeightCategories.Light:
                    drone.Battery -= LightWeightConsumption * getDistance(drone.CurrentLocation, GetCustomer(p.Target.Id).Location);
                    break;
            }
            drone.CurrentLocation = GetCustomer(p.Target.Id).Location; //changing the location of the drone to the location of the target
            UpdateDrone(drone); //update the drone

            p.Delivered = DateTime.Now; //update the deliverey time to be now
            UpdateParcel(p); //update the parcel
        }

        /// <summary>
        /// Returns a drone according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object of the drone</returns>
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
                InShipping = GetParcel(temp.ParcelId),
                CurrentLocation = temp.CurrentLocation
            };

            return drone;
        }

        /// <summary>
        /// Returns the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<ListDrone> GetDroneList()
        {
            return Drones;
        }

        /// <summary>
        /// Remove a drone from the list 
        /// </summary>
        /// <param name="drone">The drone to remove</param>
        public void RemoveDrone(Drone drone)
        {
            IDAL.DO.Drone temp = new IDAL.DO.Drone() //copy the drone to a temporary DAL drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };

            try
            {
                data.RemoveDrone(temp); //removing from the list in data

                int index = Drones.FindIndex(d => d.Id == drone.Id); //finding the index for the list in bll
                Drones.RemoveAt(index); //remove from the list in bll
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}


