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

        /// <summary>
        /// adds to the list of drones 5 drones
        /// </summary>
        private static void createDrones()
        {
            Drones.Add(new Drone
            {
                Id = r.Next(1000, 10000),
                Model = randomString(5),
                MaxWeight = (WeightCategories)r.Next(0, 3),
                Status = (DroneStatuses)r.Next(0, 3),
                Battery = r.Next(0, 101)
            });
        }

        /// <summary>
        /// adds to the list of stations 2 stations
        /// </summary>
        private static void createStations()
        {
            Stations.Add(new Station
            {
                Id = r.Next(1000, 10000),
                Name = "Jerusalem Central Station",
                Longitude = 31.7889,
                Latitude = 35.2031,
                ChargeSlots = 10
            });
            Stations.Add(new Station
            {
                Id = r.Next(1000, 10000),
                Name = "Malcha Mall",
                Longitude = 31.7515,
                Latitude = 35.1872,
                ChargeSlots = 7
            });
        }

        /// <summary>
        /// adds 10 customersw to the list of customers
        /// </summary>
        private static void createCustomers()
        {
            //Customers.Add(new Customer
            //{
            //    Id = 212069325,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 324968520,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 323658962,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 312696584,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 213695826,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 326987415,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 213698521,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 236985426,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 206985147,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
            //Customers.Add(new Customer
            //{
            //    Id = 456932814,
            //    Name = "-",
            //    Phone = "058-395-2489",
            //    Longitude = r.Next(31.7082, 31.8830),
            //    Latitude = r.Next(35.1252, 35.2642)
            //});
        }

        private static void createParcels()
        {

            //Parcels.Add(new Parcel
            //{
            //    Id = Config.IdNumber,
            //    SenderId = r.Next(100000000, 1000000000),
            //    TargetId = r.Next(100000000, 1000000000),
            //    Weight = (WeightCategories)r.Next(0, 3),
            //    Priority = (Priorities)r.Next(0, 3),
            //    Requested = 4,//random date
            //    DroneId = 0,
            //    Scheduled = 4,//random staetime
            //    PickedUp = 4,//random date
            //    Delivered = 4//random date
            //});
            Config.IdNumber++;
        }

        /// <summary>
        /// generates a random string
        /// </summary>
        /// <param name="length">length of the string ti generate</param>
        /// <returns>the random string</returns>
        private static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[r.Next(s.Length)]).ToArray());
        }
    }
}
