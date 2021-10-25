using System;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        /// <summary>
        /// constructor for DalObject class
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

        //add functions:

        /// <summary>
        /// adds a station to the list of stations
        /// </summary>
        /// <param name="id">id of the station</param>
        /// <param name="name">name of the station</param>
        /// <param name="lng">longitude of the station</param>
        /// <param name="lat">latitude of the station</param>
        /// <param name="charge">number of charge slots in the station</param>
        public static void AddStation(int id, string name, double lng, double lat, int charge)
        {
            DataSource.Stations.Add(new Station
            {
                Id = id,
                Name = name,
                Longitude = lng,
                Latitude = lat,
                ChargeSlots = charge
            });
        }

        /// <summary>
        /// adds a drone to the list of drones
        /// </summary>
        /// <param name="id">id of the drone</param>
        /// <param name="model">model of the drone</param>
        /// <param name="maxWeight">maximum weight the drone can carry</param>
        /// <param name="battery">battery percentage of the drone</param>
        public static void AddDrone(int id, string model, WeightCategories maxWeight, double battery)
        {
            DataSource.Drones.Add(new Drone
            {
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                Status = DroneStatuses.Available, //levarer
                Battery = battery
            });
        }

        /// <summary>
        /// adds a customer to list of customers
        /// </summary>
        /// <param name="id">id of the customer</param>
        /// <param name="name">name of the customer</param>
        /// <param name="phone">phone number of the customer</param>
        /// <param name="lng">longitude of the location of the customer</param>
        /// <param name="lat">latitude of the location of the customer</param>
        public static void AddCustomer(int id, string name, string phone, double lng, double lat)
        {
            DataSource.Customers.Add(new Customer
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = lng,
                Latitude = lat,
            });
        }

        /// <summary>
        /// adds a parcel to the list of parcels
        /// </summary>
        /// <param name="id">id of the parcel</param>
        /// <param name="sender">the id of the sender</param>
        /// <param name="target">the id of the target</param>
        /// <param name="weight">the weight of the parcel</param>
        /// <param name="priority">the priority of the parcel</param>
        public static void AddParcel(int sender, int target, WeightCategories weight, Priorities priority)
        {
            DataSource.Parcels.Add(new Parcel
            {
                Id = DataSource.Config.IdNumber,
                SenderId = sender,
                TargetId = target,
                Weight = weight,
                Priority = priority,
                Requested = DateTime.Now,
                DroneId = 0
            });
            DataSource.Config.IdNumber++;
        }

        //update functions:

        /// <summary>
        /// associate a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">the id of the parcel</param>
        /// <param name="droneId">the id of the drone</param>
        public static void ParcelDroneUpdate(int parcelId, int droneId)
        {
            DataSource.Parcels.ForEach(x =>
            {
                if (x.Id == parcelId)
                {
                    x.DroneId = droneId;
                    x.Scheduled = DateTime.Now;
                }
            });
        }

        /// <summary>
        /// updates the drone that was associated to a parcel to pick up the parcel
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public static void ParcelPickUp(int id)
        {
            ///if the parcel in the list has the id, update the drone that is associated to be in shipping and update pickup time
            DataSource.Parcels.ForEach(x =>
            {
                if (x.Id == id)
                {
                    UpdateDroneStatus(x.DroneId, DroneStatuses.Shipping);
                    x.PickedUp = DateTime.Now;
                }
            });
        }

        /// <summary>
        /// updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public static void ParcelDelivered(int id)
        {
            ///if the parcel in the list has the id, update the drone that is associated to be in available and update delivery time time
            DataSource.Parcels.ForEach(x =>
            {
                if (x.Id == id)
                {
                    UpdateDroneStatus(x.DroneId, DroneStatuses.Available);
                    x.Delivered = DateTime.Now;
                }
            });
        }

        /// <summary>
        /// send a drone to charge
        /// </summary>
        /// <param name="droneId">the drone to send to charge</param>
        /// <param name="stationId">the station to send it to charge</param>
        public static void DroneCharge(int droneId, int stationId)
        {
            UpdateDroneStatus(droneId, DroneStatuses.Maintenance);
            UpdateChargeSlots(stationId, -1);
            DataSource.DroneChargers.Add(new DroneCharge
            {
                DroneId = droneId,
                StationId = stationId
            });

        }

        /// <summary>
        /// release a drone from charge
        /// </summary>
        /// <param name="droneId">the id of the drone to release</param>
        /// <param name="stationId">the id of the station it is in</param>
        public static void ReleaseDroneCharge(int droneId, int stationId)
        {
            UpdateDroneStatus(droneId, DroneStatuses.Available);
            UpdateChargeSlots(stationId, 1);
            DataSource.DroneChargers.Remove(x => x.DroneId == droneId);
        }

        //display functions for objects

        /// <summary>
        /// returns the object Station that matches the id
        /// </summary>
        /// <param name="id">station id</param>
        /// <returns></returns>
        public static Station DisplayStation(int id)
        {
            return DataSource.Stations.Find(x => x.Id == id);
        }

        /// <summary>
        /// returns the object Drone that matches the id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns></returns>
        public static Drone DisplayDrone(int id)
        {
            return DataSource.Drones.Find(x => x.Id == id);
        }

        /// <summary>
        /// returns the object Customer that matches the id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        public static Customer DisplayCustomer(int id)
        {
            return DataSource.Customers.Find(x => x.Id == id);
        }

        /// <summary>
        /// returns the object Parcel that matches the id
        /// </summary>
        /// <param name="id">parcel id</param>
        /// <returns></returns>
        public static Parcel DisplayParcel(int id)
        {
            return DataSource.Parcels.Find(x => x.Id == id);
        }

        //display list fuctions

        /// <summary>
        /// return the list of stations in an array
        /// </summary>
        /// <returns></returns>
        public static Station[] stationsDisplay()
        {
            Station[] temp = new Station[DataSource.Stations.Count];
            for (int i = 0; i < DataSource.Stations.Count; i++)
            {
                temp[i] = DataSource.Stations[i];
            }
            return temp;
        }

        /// <summary>
        /// returns the list of drones in an array
        /// </summary>
        /// <returns></returns>
        public static Drone[] dronesDisplay()
        {
            Drone[] temp = new Drone[DataSource.Drones.Count];
            for (int i = 0; i < DataSource.Drones.Count; i++)
            {
                temp[i] = DataSource.Drones[i];
            }
            return temp;
        }

        /// <summary>
        /// returns the list of customers in an array
        /// </summary>
        /// <returns></returns>
        public static Customer[] customersDisplay()
        {
            Customer[] temp = new Customer[DataSource.Customers.Count];
            for (int i = 0; i < DataSource.Customers.Count; i++)
            {
                temp[i] = DataSource.Customers[i];
            }
            return temp;
        }

        /// <summary>
        /// returns a list of the parcels in an array
        /// </summary>
        /// <returns></returns>
        public static Parcel[] parcelsDisplay()
        {
            Parcel[] temp = new Parcel[DataSource.Parcels.Count];
            for (int i = 0; i < DataSource.Parcels.Count; i++)
            {
                temp[i] = DataSource.Parcels[i];
            }
            return temp;
        }

        /// <summary>
        /// returns an array with all the parcels that are not associated to a drone
        /// </summary>
        /// <returns></returns>
        public static Parcel[] noDroneParcels()
        {
            int count = 0;
            DataSource.Parcels.ForEach(x => { if (x.DroneId == 0) count++; }); //count how many parcels do not have a drone
            Parcel[] temp = new Parcel[count];
            int i = 0;
            DataSource.Parcels.ForEach(x => { if (x.DroneId == 0) { temp[i] = x; i++; } });
            return temp;
        }

        /// <summary>
        /// returns an array with tyhe list of stations with empty charge slots
        /// </summary>
        /// <returns></returns>
        public static Station[] emptyChargeSlots()
        {
            int count = 0;
            DataSource.Stations.ForEach(x => { if (x.ChargeSlots > 0) count++; }); //count how many stations have empty charge slots
            Station[] temp = new Station[count];
            int i = 0;
            DataSource.Stations.ForEach(x => { if (x.ChargeSlots > 0) { temp[i] = x; i++; } });
            return temp;
        }

        //helpful functions

        /// <summary>
        /// updates the status of a drone
        /// </summary>
        /// <param name="id">the id of the status</param>
        /// <param name="status">the status to update</param>
        private static void UpdateDroneStatus(int id, DroneStatuses status)
        {
            DataSource.Drones.ForEach(x =>
            {
                if (x.Id == id)
                {
                    x.Status = status;
                }
            });
        }

        /// <summary>
        /// update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">station id</param>
        /// <param name="num">the number to updat (add 1 or substarct 1)</param>
        private static void UpdateChargeSlots(int id, int num)
        {
            DataSource.Stations.ForEach(x =>
            {
                if (x.Id == id)
                {
                    x.ChargeSlots = x.ChargeSlots + num;
                }
            });
        }

        /// <summary>
        /// returns number of stations
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfStations()
        {
            return DataSource.Stations.Count;
        }
        /// <summary>
        /// return number of drones
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfDrone()
        {
            return DataSource.Drones.Count;
        }
        /// <summary>
        /// return number of customers
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfSCustomers()
        {
            return DataSource.Customers.Count;
        }
        /// <summary>
        /// return number of parcels
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfParcels()
        {
            return DataSource.Parcels.Count;
        }
    }
}


