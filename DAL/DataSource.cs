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
                Id = 4582,
                Model = randomString(5),
                MaxWeight = WeightCategories.Heavy,
                Status = DroneStatuses.Available,
                Battery = r.Next(0, 101)
            });
            Drones.Add(new Drone
            {
                Id = 9215,
                Model = randomString(5),
                MaxWeight = WeightCategories.Light,
                Status = DroneStatuses.Available,
                Battery = r.Next(0, 101)
            });
            Drones.Add(new Drone
            {
                Id = 2131,
                Model = randomString(5),
                MaxWeight = WeightCategories.Medium,
                Status = DroneStatuses.Maintenance,
                Battery = r.Next(0, 101)
            });
            Drones.Add(new Drone
            {
                Id = 2586,
                Model = randomString(5),
                MaxWeight = WeightCategories.Heavy,
                Status = DroneStatuses.Shipping,
                Battery = r.Next(0, 101)
            });
            Drones.Add(new Drone
            {
                Id = 3674,
                Model = randomString(5),
                MaxWeight = WeightCategories.Light,
                Status = DroneStatuses.Shipping,
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
        /// adds 10 customers to the list of customers
        /// </summary>
        private static void createCustomers()
        { 
            Customers.Add(new Customer
            {
                Id = 212069325,
                Name = "Reuvan Cohen",
                Phone = "058-395-2489",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 324968520,
                Name = "Shimon Levy",
                Phone = "058-352-2962",
                Longitude = r.Next(317082, 318830) / 100000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 323658962,
                Name = "Tirtza Bitton",
                Phone = "050-356-7495",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 312696584,
                Name = "Isachar Friedman",
                Phone = "055-963-2587",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 213695826,
                Name = "David Peretz",
                Phone = "053-245-9852",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 326987415,
                Name = "Avraham Segal",
                Phone = "052-745-3969",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 213698521,
                Name = "Yaakov Kats",
                Phone = "054-852-1365",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 236985426,
                Name = "Leah Chadad",
                Phone = "050-985-0256",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
            Customers.Add(new Customer
            {
                Id = 206985147,
                Name = "Sara Silver",
                Phone = "050-987-9955",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.00
            });
            Customers.Add(new Customer
            {
                Id = 456932814,
                Name = "Rivka Ochayoun",
                Phone = "058-256-4258",
                Longitude = r.Next(317082, 318830) / 10000.0,
                Latitude = r.Next(351252, 352642) / 10000.0
            });
        }

        private static void createParcels()
        {
            for (int i = 0; i < 10; i++)
            {
                Parcels.Add(new Parcel
                {
                    Id = Config.IdNumber,
                    SenderId = r.Next(111111111, 999999999),
                    TargetId = r.Next(111111111, 999999999),
                    Weight = (WeightCategories)r.Next(0, 3),
                    Priority = (Priorities)r.Next(0, 3),
                    Requested = DateTime.Now,
                    DroneId = 0,
                    Scheduled = default,
                    PickedUp = default,
                    Delivered = default
                });
                Config.IdNumber++;
            }
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
