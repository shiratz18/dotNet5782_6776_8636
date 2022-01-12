using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    internal class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>(10);
        internal static List<DroneCharge> DroneChargers = new List<DroneCharge>(10);
        internal static List<Station> Stations = new List<Station>(5);
        internal static List<Customer> Customers = new List<Customer>(100);
        internal static List<Parcel> Parcels = new List<Parcel>(1000);

        internal class Config
        {
            internal static int IdNumber = 1000000;
            internal static double AvailableConsumption { get => 1; } //1% per km
            internal static double LightWeightConsumption { get => 2; } //2% per km
            internal static double MediumWeightConsumption { get => 3; }  //3% per km
            internal static double HeavyWeightConsumption { get => 4; } //4% per km
            internal static double ChargingRate { get => 5; } //1% per minute
            internal static string AccessCode = "1234"; //acces code for worker interface
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

        #region Create drones
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
                Active = true
            });
            Drones.Add(new Drone
            {
                Id = 9215,
                Model = randomString(5),
                MaxWeight = WeightCategories.Heavy,
                Active = true
            });
            Drones.Add(new Drone
            {
                Id = 2131,
                Model = randomString(5),
                MaxWeight = WeightCategories.Medium,
                Active = true
            });
            Drones.Add(new Drone
            {
                Id = 2586,
                Model = randomString(5),
                MaxWeight = WeightCategories.Heavy,
                Active = true
            });
            Drones.Add(new Drone
            {
                Id = 3674,
                Model = randomString(5),
                MaxWeight = WeightCategories.Light,
                Active = true
            });
        }
        #endregion

        #region Create stations
        /// <summary>
        /// adds to the list of stations 2 stations
        /// </summary>
        private static void createStations()
        {
            Stations.Add(new Station
            {
                Id = 1234,
                Name = "Jerusalem Central Station",
                Latitude = 31.78954,
                Longitude = 35.20373,
                ChargeSlots = 10,
                Active = true
            });
            Stations.Add(new Station
            {
                Id = 5678,
                Name = "Malcha Mall",
                Latitude = 31.75173,
                Longitude = 35.18726,
                ChargeSlots = 7,
                Active = true
            });
        }
        #endregion

        #region Create customers
        /// <summary>
        /// adds 10 customers to the list of customers
        /// </summary>
        private static void createCustomers()
        {
            Customers.Add(new Customer
            {
                Id = 123456789,
                Name = "Reuvan Cohen",
                Phone = "0583952489",
                Latitude = 31.78913413396946,
                Longitude = 35.17102699703434,
                Password = "aaaa",
                Answer = "bbbb",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 324968520,
                Name = "Shimon Levy",
                Phone = "0583522962",
                Latitude = 31.786015836732847,
                Longitude = 35.18862021237819,
                Password = "cccc",
                Answer = "dddd",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 323658962,
                Name = "Tirtza Bitton",
                Phone = "0503567495",
                Latitude = 31.78619053512268,
                Longitude = 35.21472236819816,
                Password = "eeee",
                Answer = "ffff",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 312696584,
                Name = "Isachar Friedman",
                Phone = "0559632587",
                Latitude = 31.76275454575024,
                Longitude = 35.18716715470557,
                Password = "gggg",
                Answer = "hhhh",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 213695826,
                Name = "David Peretz",
                Phone = "0532459852",
                Latitude = 31.76335370891613,
                Longitude = 35.18259758354167,
                Password = "iiii",
                Answer = "jjjj",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 326987415,
                Name = "Avraham Segal",
                Phone = "0527453969",
                Latitude = 31.789408369550717,
                Longitude = 35.17261664121437,
                Password = "kkkk",
                Answer = "llll",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 213698521,
                Name = "Yaakov Kats",
                Phone = "0548521365",
                Latitude = 31.787829416071816,
                Longitude = 35.18189506819828,
                Password = "mmmm",
                Answer = "nnnn",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 236985426,
                Name = "Leah Chadad",
                Phone = "0509850256",
                Latitude = 31.78437499940112,
                Longitude = 35.21786601052626,
                Password = "oooo",
                Answer = "pppp",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 206985147,
                Name = "Sara Silver",
                Phone = "0509879955",
                Latitude = 31.817709663575744,
                Longitude = 35.19441221052715,
                Password = "qqqq",
                Answer = "rrrr",
                Active = true
            });
            Customers.Add(new Customer
            {
                Id = 456932814,
                Name = "Rivka Ochayoun",
                Phone = "0582564258",
                Latitude = 31.770637478956328,
                Longitude = 35.18372692586983,
                Password = "ssss",
                Answer = "tttt",
                Active = true
            });
        }
        #endregion

        #region Create parcels
        /// <summary>
        /// adds 10 parcels to the list of parcels
        /// </summary>
        private static void createParcels()
        {
            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 4582,
                Scheduled = DateTime.Now,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 2131,
                Scheduled = DateTime.Now - new TimeSpan(1, 30, 50),
                PickedUp = DateTime.Now - new TimeSpan(0, 30, 45),
                Delivered = DateTime.Now - new TimeSpan(0, 15, 30)
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 9215,
                Scheduled = DateTime.Now - new TimeSpan(1, 30, 50),
                PickedUp = DateTime.Now,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 2131,
                Scheduled = DateTime.Now,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 2586,
                Scheduled = DateTime.Now,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 3674,
                Scheduled = DateTime.Now,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;

            Parcels.Add(new Parcel
            {
                Id = Config.IdNumber,
                SenderId = Customers[r.Next(0, 10)].Id,
                TargetId = Customers[r.Next(0, 10)].Id,
                Weight = (WeightCategories)r.Next(0, 3),
                Priority = (Priorities)r.Next(0, 3),
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            });
            Config.IdNumber++;
        }
        #endregion

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
