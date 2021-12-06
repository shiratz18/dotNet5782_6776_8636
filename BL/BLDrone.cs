﻿using System;
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
            if (drone.Id < 1000 || drone.Id > 9999)
                throw new InvalidNumberException($"Station ID must be 4 digits.");

            IDAL.DO.Drone temp = new IDAL.DO.Drone() //copy the drone to a temporary DAL drone
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight
            };

            Station s = GetStation(stationNum); //getting the station, will throw an exception if the station does not exist
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

            Drones.Add(new ListDrone() //adding the drone to the list of drones in BL
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight,
                Battery = R.Next(20, 41),
                Status = DroneStatuses.Maintenance,
                CurrentLocation = s.Location,
                ParcelId = 0,
                ChargingBegin = DateTime.Now
            });

            IDAL.DO.DroneCharge charge = new IDAL.DO.DroneCharge()
            {
                DroneId = drone.Id,
                StationId = stationNum
            };
            data.AddDroneCharge(charge); //adding drone charge to the list in data source

            UpdateStationChargingSlots(stationNum, s.AvailableChargeSlots - 1); //update the station to have one less charging slots           
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
                InShipping = new ParcelInShipping(),
                CurrentLocation = temp.CurrentLocation
            };

            if (temp.ParcelId != 0)
                drone.InShipping = getParcelInShipping(temp.ParcelId);

            return drone;
        }

        /// <summary>
        /// Returns the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<ListDrone> GetDroneList()
        {
            if (Drones.Count == 0)
                throw new EmptyListException("No drones to display.");
            return Drones;
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
            if (!Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} does not exist.");

            var drone = Drones.Find(d => d.Id == id);
            drone.Model = name;

            try
            {
                data.EditDroneModel(id, name); //update the drone in data layer
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
            ListDrone d = Drones.Find(x => x.Id == id); //finding the drone in the list of bll

            //throw an exception if the drone wasnt found
            if (d == null)
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

            //get the station that is nearest to the drone, among the stations that have availabke charge slots
            double min = 100000; //no two places in Jerusalem have a greater distance (our company is placed in Jerusalem)
            Station s = new Station();
            foreach (Station tmp in stations)
            {
                //if the distance between the location and the nearest station is smaller than the current minimum, so this is the minimum
                if (getDistance(d.CurrentLocation, tmp.Location) < min)
                {
                    min = getDistance(d.CurrentLocation, tmp.Location);
                    s = tmp; //the nearest station will be saved here
                }
            }

            //throw an exception if there is not enough battery to get to the station
            if (d.Battery < AvailableConsumption * getDistance(d.CurrentLocation, s.Location))
            {
                throw new NoBatteryException($"Drone {d.Id} does not have enough battery to get to a charging station.");
            }

            d.Battery -= AvailableConsumption * getDistance(d.CurrentLocation, s.Location); //decreasing from the battery the percaentage it takes to get to the station
            d.CurrentLocation = s.Location; //changing the location to be the station
            d.Status = DroneStatuses.Maintenance; //change the status to be in maintenance
            d.ChargingBegin = DateTime.Now;

            IDAL.DO.Drone drone = new IDAL.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight
            };
            data.UpdateDrone(drone); //update the drone in dal

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
        public void ReleaseDroneCharge(int id)
        {
            if (!Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} does not exist.");

            ListDrone d = Drones.Find(d => d.Id == id);

            if (d.Status != DroneStatuses.Maintenance) //if the drone isnt charging throw an exception
                throw new DroneStateException($"Drone {id} is not currently charging.");

            TimeSpan time = DateTime.Now - d.ChargingBegin;

            d.Battery = d.Battery + (time.Minutes * DroneChargingRate); //adding the battery the drone has charged
            if (d.Battery > 100)
                d.Battery = 100;
            d.Status = DroneStatuses.Available; //update the status to be available

            Station s = getStationByLocation(d.CurrentLocation);
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
            //checking if the drone exists
            if (!Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} does not exist.");

            ListDrone drone = Drones.Find(d => d.Id == id);

            //checking that the drone is available
            if (drone.Status != DroneStatuses.Available)
                throw new DroneStateException($"Drone {id} is currently unavailable for shipping.");

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
            //and checking that the drone could actually deliver them according to weight, distance and battery
            foreach (ParcelInShipping p in parcels)
            {
                if (p.Priority == Priorities.Urgent && p.Weight <= drone.MaxWeight) //creating the list of urgent parcels
                {
                    switch (p.Weight) //checking that there is enough battery according to weight and distance
                    {
                        case WeightCategories.Heavy:
                            if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                highPriority.Add(p);
                            break;
                        case WeightCategories.Medium:
                            if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                highPriority.Add(p);
                            break;
                        case WeightCategories.Light:
                            if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                highPriority.Add(p);
                            break;
                    }
                }

                else if (p.Priority == Priorities.Express && p.Weight <= drone.MaxWeight) //creating the list of less uregent parcels
                {
                    switch (p.Weight) //checking that there is enough battery according to weight and distance
                    {
                        case WeightCategories.Heavy:
                            if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                mediumPriority.Add(p);
                            break;
                        case WeightCategories.Medium:
                            if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                mediumPriority.Add(p);
                            break;
                        case WeightCategories.Light:
                            if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                mediumPriority.Add(p);
                            break;
                    }
                }

                else if (p.Priority == Priorities.Regular && p.Weight <= drone.MaxWeight) //creating the list of low ptiority parcels
                {
                    switch (p.Weight) //checking that there is enough battery according to weight and distance
                    {
                        case WeightCategories.Heavy:
                            if (drone.Battery >= HeavyWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                lowPriority.Add(p);
                            break;
                        case WeightCategories.Medium:
                            if (drone.Battery >= MediumWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                lowPriority.Add(p);
                            break;
                        case WeightCategories.Light:
                            if (drone.Battery >= LightWeightConsumption * distanceForDelivery(id, p.Sender.Id, p.Target.Id))
                                lowPriority.Add(p);
                            break;
                    }
                }
            }

            List<ParcelInShipping> heavy = new List<ParcelInShipping>();
            List<ParcelInShipping> medium = new List<ParcelInShipping>();
            List<ParcelInShipping> light = new List<ParcelInShipping>();

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
            else if (lowPriority.Count > 0) //otherwise if there is at least one low priotity parcel that the drone can carry
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
            else //otherwise the drone cannot carry any parcel, throw an exception
                throw new DroneStateException($"Drone {id} cannot currently deliver any parcel.");

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
            else //otherwise there is only a lowweight parcel the drone could carry
            {
                parcel = closestParcel(id, light); //finding the closest parcel to the drone among the parcels
            }

            drone.Status = DroneStatuses.Shipping; //updating the status of the drone to be in shipping
            drone.ParcelId = parcel.Id; //updating the parcel the drone carries

            data.AssignDroneToParcel(parcel.Id, drone.Id);
        }

        /// <summary>
        /// Update that a drone picked up a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DronePickUp(int id)
        {
            Drone drone = GetDrone(id);

            if (drone.InShipping.Id == 0) //if the drone does not have a parcel 
            {
                throw new DroneStateException($"Drone {id} does not have a parcel assigned.");
            }
            if (drone.InShipping.IsPickedUp) //if the parcel has already been picked up
            {
                throw new DroneStateException($"Drone {id} cannot pick up the parcel since it has already been picked up.");
            }

            ParcelInShipping p = drone.InShipping;
            ListDrone d = Drones.Find(x => x.Id == drone.Id);

            switch (p.Weight)
            {
                //update the battery according to weight and distance
                case WeightCategories.Heavy:
                    d.Battery -= HeavyWeightConsumption * getDistance(drone.CurrentLocation, p.PickUpLocation);
                    break;
                case WeightCategories.Medium:
                    d.Battery -= MediumWeightConsumption * getDistance(drone.CurrentLocation, p.PickUpLocation);
                    break;
                case WeightCategories.Light:
                    d.Battery -= LightWeightConsumption * getDistance(drone.CurrentLocation, p.PickUpLocation);
                    break;
            }
            d.CurrentLocation = p.PickUpLocation;

            data.ParcelPickUp(p.Id); //updating the parcel in dll
        }

        /// <summary>
        /// Update that a drone delivered a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DroneDeliver(int id)
        {
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
            ListDrone d = Drones.Find(x => x.Id == drone.Id);

            switch (p.Weight)
            {
                //update the battery according to weight and distance
                case WeightCategories.Heavy:
                    d.Battery -= HeavyWeightConsumption * p.DeliveryDistance;
                    break;
                case WeightCategories.Medium:
                    d.Battery -= MediumWeightConsumption * p.DeliveryDistance;
                    break;
                case WeightCategories.Light:
                    d.Battery -= LightWeightConsumption * p.DeliveryDistance;
                    break;
            }
            d.CurrentLocation = p.DeliveryLocation;
            d.Status = DroneStatuses.Available;
            d.ParcelId = 0;

            data.ParcelDelivered(p.Id);
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


