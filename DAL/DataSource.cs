using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>(10);
        internal static List<DroneCharge> DroneChargers = new List<DroneCharge>(10);
        internal static List<Station> Stations = new List<Station>(5);
        internal static List<Customer> Customers = new List<Customer>(100);
        internal static List<Parcel> Parcels = new List<Parcel>(1000);

        internal class Config
        {
            internal static int IdNumber = 1000000;
        }

        internal static Random r = new Random();

        /// <summary>
        /// intialize the lists 
        /// </summary>
        public static void Initialize()
        {
            createDrones();
            createStations();
            createCustomers();
            createParcels();
        }
        
        private static void createDrones()
        {
            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new Drone
                {
                    Id = r.Next(1000, 10000),
                    Model = " ", //create random string function
                    MaxWeight = (WeightCategories)r.Next(0, 3),
                    Status = (DroneStatuses)r.Next(0, 3),
                    Battery = r.Next(0, 101)
                });
            }
        }
        
        private static void createStations()
        {
            Stations.Add(new Station
            {
                Id = r.Next(1000, 10000),
                Name = "Katamon",
                Longitude = 31.8430,
                Latitude = 35.2242,
                ChargeSlots = r.Next(10, 20)
            });
            Stations.Add(new Station
            {
                Id = r.Next(1000, 10000),
                Name = "Giva't Shaul",
                Longitude = 31.8430,
                Latitude = 35.2242,
                ChargeSlots = r.Next(10, 20)
            });
        }

        private static void createCustomers()
        {
            for (int i = 0; i < 10; i++)
            {
                Customers.Add(new Customer
                {
                    Id = r.Next(100000000,1000000000),
                    Name = " ", //create random string function
                    Phone =//ramdom string
                    Longitude = 5,//create random location
                    Latitude = 4,
                });
            }
        }

        private static void createParcels()
        {
            for (int i = 0; i < 10; i++)
            {
                Parcels.Add(new Parcel
                {
                    Id = Config.IdNumber,
                    SenderId = r.Next(100000000, 1000000000),
                    TargetId = r.Next(100000000, 1000000000),
                    Weight = (WeightCategories)r.Next(0, 3),
                    Priority = (Priorities)r.Next(0, 3),
                    Requested = 4,//random date
                    DroneId = 0,
                    Scheduled = 4,//random staetime
                    PickedUp = 4,//random date
                    Delivered = 4//random date
                });
                Config.IdNumber++;
            }
        }
    }
}
