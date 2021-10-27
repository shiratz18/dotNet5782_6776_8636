﻿using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
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
        /// assign a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">the id of the parcel</param>
        /// <param name="droneId">the id of the drone</param>
        public static void ParcelDroneUpdate(int parcelId, int droneId)
        {
            Parcel temp = new Parcel();
            foreach(Parcel x in DataSource.Parcels)
            {
                if(x.Id==parcelId)
                {
                    temp = x;
                    temp.DroneId = droneId;
                    temp.Scheduled = DateTime.Now;
                    DataSource.Parcels.Remove(x);
                    DataSource.Parcels.Add(temp);
                    break;
                }
            }
        }

        /// <summary>
        /// updates the drone that was assigned to a parcel to pick up the parcel
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public static void ParcelPickUp(int id)
        {
            Parcel temp = new Parcel();
            foreach (Parcel x in DataSource.Parcels)
            {
                if (x.Id == id)
                {
                    temp = x;
                    temp.PickedUp = DateTime.Now;
                    DataSource.Parcels.Remove(x);
                    DataSource.Parcels.Add(temp);
                    break;
                }
            }
        }

        /// <summary>
        /// updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public static void ParcelDelivered(int id)
        {
            Parcel temp = new Parcel();
            foreach (Parcel x in DataSource.Parcels)
            {
                if (x.Id == id)
                {
                    temp = x;
                    temp.Delivered = DateTime.Now;
                    DataSource.Parcels.Remove(x);
                    DataSource.Parcels.Add(temp);
                    break;
                }
            }
        }

        /// <summary>
        /// send a drone to charge
        /// </summary>
        /// <param name="droneId">the drone to send to charge</param>
        /// <param name="stationId">the station to send it to charge</param>
        public static void ChargeDrone(int droneId, int stationId)
        {
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
            DroneCharge temp = (new DroneCharge { DroneId = droneId, StationId = stationId });
            DataSource.DroneChargers.Remove(temp);
        }

        /// <summary>
        /// update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">station id</param>
        /// <param name="num">the number to updat (add 1 or substarct 1)</param>
        public static void UpdateChargeSlots(int id, int num)
        {

            Station temp = new Station();
            foreach (Station x in DataSource.Stations)
            {
                if (x.Id == id)
                {
                    temp = x;
                    temp.ChargeSlots += num;
                    DataSource.Stations.Remove(x);
                    DataSource.Stations.Add(temp);
                    break;
                }
            }
        }

        /// <summary>
        /// updates the status of a drone
        /// </summary>
        /// <param name="id">the id of the status</param>
        /// <param name="status">the status to update</param>
        public static void UpdateDroneStatus(int id, DroneStatuses status)
        {
            Drone temp = new Drone();
            foreach (Drone x in DataSource.Drones)
            {
                if (x.Id == id)
                {
                    temp = x;
                    temp.Status = status;
                    DataSource.Drones.Remove(x);
                    DataSource.Drones.Add(temp);
                    break;
                }
            }
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
        /// return the list of stations
        /// </summary>
        /// <returns>list Stations</returns>
        public static List<Station> StationsDisplay()
        {
            return DataSource.Stations;
        }

        /// <summary>
        /// returns the list of drones
        /// </summary>
        /// <returns>list Drones</returns>
        public static List<Drone> DronesDisplay()
        {
            return DataSource.Drones;
        }

        /// <summary>
        /// returns the list of customers
        /// </summary>
        /// <returns>list Customers</returns>
        public static List<Customer> CustomersDisplay()
        {
            return DataSource.Customers;
        }

        /// <summary>
        /// returns a list of the parcels
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> ParcelsDisplay()
        {
            return DataSource.Parcels;
        }

        /// <summary>
        /// returns a list of the drones charging
        /// </summary>
        /// <returns></returns>
        public static List<DroneCharge> DroneChargeDisplay()
        {
            return DataSource.DroneChargers;
        }

        /// <summary>
        /// returns a list with all the parcels that are not associated to a drone
        /// </summary>
        /// <returns></returns>
        public static List<Parcel> NoDroneParcels()
        {
            List<Parcel> temp = new List<Parcel>();
            DataSource.Parcels.ForEach(x => { if (x.DroneId == 0) temp.Add(x); });
            return temp;
        }

        /// <summary>
        /// returns an array with tyhe list of stations with empty charge slots
        /// </summary>
        /// <returns></returns>
        public static List<Station> EmptyChargeSlots()
        {
            List<Station> temp = new List<Station>();
            DataSource.Stations.ForEach(x => { if (x.ChargeSlots > 0) temp.Add(x); }); 
            return temp;
        }

        //distance functions:

        /// <summary>
        /// finds the distance from a station
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static double FindDistanceStation(double lat1, double lng1, int id)
        {
            Station temp = DataSource.Stations.Find(x => x.Id == id);
            Double lat2 = temp.Latitude, lng2 = temp.Longitude;
            return Location.GetDistanceFromLatLngInKm(lat1, lng1, lat2, lng2);
        }
        /// <summary>
        /// finds the distance from a customer
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static double FindDistanceCustomer(double lat1, double lng1, int id)
        {
            Customer temp = DataSource.Customers.Find(x => x.Id == id);
            Double lat2 = temp.Latitude, lng2 = temp.Longitude;
            return Location.GetDistanceFromLatLngInKm(lat1, lng1, lat2, lng2);
        }
    }
}


