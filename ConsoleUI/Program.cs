using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using DO;
using DalApi;

//namespace ConsoleUI
//{
//    class Program
//    {
//        public enum MenuOptions { Exit, Add, Update, DisplayOne, DisplayList, FindDistance }
//        public enum AddOptions { Exit, Station, Drone, Customer, Parcel }
//        public enum UpdateOptions { Exit, ParcelToDrone, PickUp, Deliver, Charge, ReleaseCharge, DroneStatus }
//        public enum DisplayOptions { Exit, Station, Drone, Customer, Parcel, NoDroneParcel, AvailableChargeStations }
//        public enum DistanceOptions { Exit, Station, Customer }

//        static void Main(string[] args)
//        {
//            IDal data = new DalObject.DalObject();
//            MainMenu(data);
//        }

//        /// <summary>
//        /// Print main menu to user
//        /// </summary>
//        public static void MainMenu(IDal data)
//        {
//            Console.WriteLine("What would you like to do:\n 1 - Add an object\n 2 - Update an object\n 3" +
//                " - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
//            MenuOptions choice;
//            MenuOptions.TryParse(Console.ReadLine(), out choice);
//            while (choice != MenuOptions.Exit)
//            {
//                switch (choice)
//                {
//                    case MenuOptions.Add:
//                        AddMenu(data);
//                        break;
//                    case MenuOptions.Update:
//                        UpdateMenu(data);
//                        break;
//                    case MenuOptions.DisplayOne:
//                        DisplayOneMenu(data);
//                        break;
//                    case MenuOptions.DisplayList:
//                        DisplayListMenu(data);
//                        break;
//                    case MenuOptions.FindDistance:
//                        FindDistanceMenu(data);
//                        break;
//                }
//                Console.WriteLine("\nWhat would you like to do:\n 1 - Add an object\n 2 - Update an object\n" +
//                    " 3 - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
//                MenuOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        /// <summary>
//        /// Print add menu to user
//        /// </summary>
//        public static void AddMenu(IDal data)
//        {
//            Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n" +
//                " 4 - Add a parcel\n 0 - Exit");
//            AddOptions choice;
//            AddOptions.TryParse(Console.ReadLine(), out choice);
//            string name = "";
//            int id = 0;
//            double lng = 0, lat = 0;
//            while (choice != AddOptions.Exit)
//            {
//                switch (choice)
//                {
//                    case AddOptions.Station:
//                        Console.WriteLine("Enter the ID, name, location, and number of available charging slots of the station:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        name = Console.ReadLine();
//                        double.TryParse(Console.ReadLine(), out lng);
//                        double.TryParse(Console.ReadLine(), out lat);
//                        int num;
//                        int.TryParse(Console.ReadLine(), out num);
//                        Station station = new Station
//                        {
//                            Id = id,
//                            Name = name,
//                            Longitude = lng,
//                            Latitude = lat,
//                            ChargeSlots = num
//                        };
//                        data.AddStation(station);
//                        break;

//                    case AddOptions.Drone:
//                        Console.WriteLine("Enter the ID, model, maximum weight (0-light, 1-mediun, 2-heavy), and battery of the drone:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        string model = Console.ReadLine();
//                        WeightCategories max;
//                        WeightCategories.TryParse(Console.ReadLine(), out max);
//                        double battery;
//                        double.TryParse(Console.ReadLine(), out battery);
//                        Drone drone = new Drone
//                        {
//                            Id = id,
//                            Model = model,
//                            MaxWeight = max
//                        };
//                        data.AddDrone(drone);
//                        break;

//                    case AddOptions.Customer:
//                        Console.WriteLine("Enter the  ID, name, phone, location of the customer:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        name = Console.ReadLine();
//                        string phone = Console.ReadLine();
//                        double.TryParse(Console.ReadLine(), out lng);
//                        double.TryParse(Console.ReadLine(), out lat);
//                        Customer customer = new Customer
//                        {
//                            Id = id,
//                            Name = name,
//                            Phone = phone,
//                            Longitude = lng,
//                            Latitude = lat,
//                        };
//                        data.AddCustomer(customer);
//                        break;

//                    case AddOptions.Parcel:
//                        Console.WriteLine("Enter the sender's ID, target's ID, parcel weight (0-light, 1-mediun, 2-heavy)," +
//                            " and priority (0-regular, 1-express, 2-urgent) of the parcel:");
//                        int senderId;
//                        int.TryParse(Console.ReadLine(), out senderId);
//                        int targetId;
//                        int.TryParse(Console.ReadLine(), out targetId);
//                        WeightCategories weight;
//                        WeightCategories.TryParse(Console.ReadLine(), out weight);
//                        Priorities priority;
//                        Priorities.TryParse(Console.ReadLine(), out priority);
//                        Parcel parcel = new Parcel
//                        {
//                            SenderId = senderId,
//                            TargetId = targetId,
//                            Weight = weight,
//                            Priority = priority,
//                            Requested = DateTime.Now,
//                            DroneId = 0
//                        };
//                        data.AddParcel(parcel);
//                        break;
//                }
//                Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n" +
//                    " 4 - Add a parcel\n 0 - Exit");
//                AddOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        /// <summary>
//        /// Print the update menu to the user
//        /// </summary>
//        public static void UpdateMenu(IDal data)
//        {
//            Console.WriteLine("\nWhat would you like to update:\n 1 - Assign drone to parcel \n 2 - Drone pick up of parcel\n" +
//                " 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n" +
//                " 0 - Exit");
//            UpdateOptions choice;
//            UpdateOptions.TryParse(Console.ReadLine(), out choice);
//            int droneId = 0, parcelId = 0, stationId = 0;
//            while (choice != UpdateOptions.Exit)
//            {
//                switch (choice)
//                {
//                    case UpdateOptions.ParcelToDrone:
//                        Console.WriteLine("Pick a parcel:");
//                        PrintAllNoDroneParcels(data);
//                        Console.WriteLine("Enter the ID of the parcel and of the drone:");
//                        int.TryParse(Console.ReadLine(), out parcelId);
//                        int.TryParse(Console.ReadLine(), out droneId);
//                        data.AssignDroneToParcel(parcelId, droneId);
//                        break;

//                    case UpdateOptions.PickUp:
//                        Console.WriteLine("Enter the ID of the parcel:");
//                        int.TryParse(Console.ReadLine(), out parcelId);
//                        data.ParcelPickUp(parcelId);
//                        break;

//                    case UpdateOptions.Deliver:
//                        Console.WriteLine("Enter the ID of the parcel:");
//                        int.TryParse(Console.ReadLine(), out parcelId);
//                        data.ParcelDelivered(parcelId);
//                        break;

//                    case UpdateOptions.Charge:
//                        Console.WriteLine("Pick a station:");
//                        PrintAllAvailableChargeStations(data);
//                        Console.WriteLine("Enter the ID of the drone and the station:");
//                        int.TryParse(Console.ReadLine(), out droneId);
//                        int.TryParse(Console.ReadLine(), out stationId);
//                        DroneCharge d = new DroneCharge
//                        {
//                            DroneId = droneId,
//                            StationId = stationId
//                        };
//                        data.AddDroneCharge(d);
//                        data.UpdateChargeSlots(stationId, -1);
//                        break;

//                    case UpdateOptions.ReleaseCharge:
//                        Console.WriteLine("Enter drone and station ID from the list below:");
//                        PrintAllDronesCharging(data);
//                        int.TryParse(Console.ReadLine(), out droneId);
//                        int.TryParse(Console.ReadLine(), out stationId);
//                        DroneCharge dr = new DroneCharge
//                        {
//                            DroneId = droneId,
//                            StationId = stationId
//                        };
//                        data.RemoveDroneCharge(dr);
//                        break;

//                        //case UpdateOptions.DroneStatus:
//                        //    Console.WriteLine("Enter the drone ID and the status (0 - Available, 1 - Maintenance, 2 - Shipping):");
//                        //    int.TryParse(Console.ReadLine(), out droneId);
//                        //    IDAL.DO.DroneStatuses status;
//                        //    IDAL.DO.DroneStatuses.TryParse(Console.ReadLine(), out status);
//                        //    data.UpdateDroneStatus(droneId, status);
//                        //    break;
//                }
//                Console.WriteLine("\nWhat would you like to update:\n 1 - Assign drone to parcel \n 2 - Drone pick up of parcel\n" +
//                    " 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n" +
//                    " 0 - Exit");
//                UpdateOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        /// <summary>
//        /// Print the menu of display one to the user
//        /// </summary>
//        public static void DisplayOneMenu(IDal data)
//        {
//            int id;
//            Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
//            DisplayOptions choice;
//            DisplayOptions.TryParse(Console.ReadLine(), out choice);
//            while (choice != DisplayOptions.Exit)
//            {
//                switch (choice)
//                {
//                    case DisplayOptions.Station:
//                        Console.WriteLine("Enter the station ID:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        Console.WriteLine(data.GetStation(id));
//                        break;

//                    case DisplayOptions.Drone:
//                        Console.WriteLine("Enter the drone ID:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        Console.WriteLine(data.GetDrone(id));
//                        break;

//                    case DisplayOptions.Customer:
//                        Console.WriteLine("Enter the customer ID:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        Console.WriteLine(data.GetCustomer(id));
//                        break;

//                    case DisplayOptions.Parcel:
//                        Console.WriteLine("Enter the parcel ID:");
//                        int.TryParse(Console.ReadLine(), out id);
//                        Console.WriteLine(data.GetParcel(id));
//                        break;
//                }
//                Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
//                DisplayOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        /// <summary>
//        /// Print the menu of list displayy to user
//        /// </summary>
//        public static void DisplayListMenu(IDal data)
//        {
//            Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n" +
//                " 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
//            DisplayOptions choice;
//            DisplayOptions.TryParse(Console.ReadLine(), out choice);
//            while (choice != DisplayOptions.Exit)
//            {
//                switch (choice)
//                {
//                    case DisplayOptions.Station:
//                        PrintAllStations(data);
//                        break;
//                    case DisplayOptions.Drone:
//                        PrintAllDrones(data);
//                        break;
//                    case DisplayOptions.Customer:
//                        PrintAllCustomers(data);
//                        break;
//                    case DisplayOptions.Parcel:
//                        PrintAllParcels(data);
//                        break;
//                    case DisplayOptions.NoDroneParcel:
//                        PrintAllNoDroneParcels(data);
//                        break;
//                    case DisplayOptions.AvailableChargeStations:
//                        PrintAllAvailableChargeStations(data);
//                        break;
//                }
//                Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n" +
//                    " 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
//                DisplayOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        public static void FindDistanceMenu(IDal data)
//        {
//            Console.WriteLine("\nWhat distance do you want:\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");
//            DistanceOptions choice;
//            DistanceOptions.TryParse(Console.ReadLine(), out choice);
//            while (choice != DistanceOptions.Exit)
//            {
//                Console.WriteLine("Enter longitude and latitude:");
//                double lng = double.Parse(Console.ReadLine());
//                double lat = double.Parse(Console.ReadLine());
//                int id;
//                switch (choice)
//                {
//                    case DistanceOptions.Station:
//                        Console.WriteLine("Enter station ID:");
//                        id = int.Parse(Console.ReadLine());
//                        Console.WriteLine($"The distance is: {string.Format("{0:0.0000}", data.FindDistanceStation(lng, lat, id))} km");
//                        break;
//                    case DistanceOptions.Customer:
//                        Console.WriteLine("Enter customer ID:");
//                        id = int.Parse(Console.ReadLine());
//                        Console.WriteLine($"The distance is: {string.Format("{0:0.0000}", data.FindDistanceCustomer(lng, lat, id))} km");
//                        break;
//                }
//                Console.WriteLine("\nWhat distance do you want:\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");

//                DistanceOptions.TryParse(Console.ReadLine(), out choice);
//            }
//        }

//        /// <summary>
//        /// print all the stations
//        /// </summary>
//        public static void PrintAllStations(IDal data)
//        {
//            List<Station> Stations = (List<Station>)data.GetStationList();
//            Stations.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the drones
//        /// </summary>
//        public static void PrintAllDrones(IDal data)
//        {
//            List<Drone> Drones = (List<Drone>)data.GetDroneList();
//            Drones.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the customers
//        /// </summary>
//        public static void PrintAllCustomers(IDal data)
//        {
//            List<Customer> Customers = (List<Customer>)data.GetCustomerList();
//            Customers.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the parcels
//        /// </summary>
//        public static void PrintAllParcels(IDal data)
//        {
//            List<Parcel> Parcels = (List<Parcel>)data.GetParcelList();
//            Parcels.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the parcels with no drone associated
//        /// </summary>
//        public static void PrintAllNoDroneParcels(IDal data)
//        {
//            //List<Parcel> Parcels = (List<Parcel>)data.GetNoDroneParcels();
//           // Parcels.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the stations with empty charge slots
//        /// </summary>
//        public static void PrintAllAvailableChargeStations(IDal data)
//        {
//            //List<Station> Stations = (List<Station>)data.GetEmptyChargeSlots();
//           // Stations.ForEach(x => { Console.WriteLine(x); });
//        }
//        /// <summary>
//        /// print all the drones that are charging
//        /// </summary>
//        public static void PrintAllDronesCharging(IDal data)
//        {
//            List<DroneCharge> DroneCharges = (List<DroneCharge>)data.GetDroneChargeList();
//            DroneCharges.ForEach(x => { Console.WriteLine(x); });
//        }
//    }
//}
