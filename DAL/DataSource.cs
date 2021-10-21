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
        internal static List<Station> Stations = new List<Station>(5);
        internal static List<Customer> Customers = new List<Customer>(100);
        internal static List<Parcel> Parcels = new List<Parcel>(1000);
        internal class Config
        {
            internal static int IdNumber = 11;
        }
        internal static Random r = new Random();
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
                    Id = r.Next(1, 10),
                    Model = " ", //create random string function
                    MaxWeight = WeightCategories.Random.Range(0, 2),
                    Status = 0, //create random enum
                    Battery = r.Next(0, 100)
                });
            }
        }
        private static void createStations()
        {
            for (int i = 0; i < 2; i++)
            {
                Stations.Add(new Station
                {
                    Id = r.Next(1, 5),
                    Name = " ", //create random string function
                    Longitude = 5,//create random location
                    Latitude = 4,
                    ChargeSlots = r.Next(0, 100)//find out how many charging slots there are..
                });
            }
        }
        private static void createCustomers()
        {
            for (int i = 0; i < 10; i++)
            {
                Customers.Add(new Customer
                {
                    Id = r.Next(1, 5),
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
                    Id = r.Next(1, 10),
                    SenderId = r.Next(1, 10),
                    TargetId = r.Next(1, 10),
                    Weight = 4,//random enum
                    Priority = 3,//random enum
                    Requested = 4,//random date
                    DroneId = r.Next(1, 5),
                    Scheduled = 4,//random staetime
                    PickedUp = 4,//random date
                    Delivered = 4//random date
                });
            }
        }
    }
}
