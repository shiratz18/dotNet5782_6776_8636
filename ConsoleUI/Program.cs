using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        public enum MenuOptions { Exit, Add, Update, DisplayOne, DisplayList, FindDistance }
        public enum AddOptions { Exit, Station, Drone, Customer, Parcel }
        public enum UpdateOptions { Exit, ParcelToDrone, PickUp, Deliver, Charge, ReleaseCharge, DroneStatus }
        public enum DisplayOptions { Exit, Station, Drone, Customer, Parcel, NoDroneParcel, AvailableChargeStations }
        public enum DistanceOptions { Exit, Station, Customer }

        static void Main(string[] args)
        {
            new DalObject.DalObject();
            MainMenu();
        }

        /// <summary>
        /// Print main menu to user
        /// </summary>
        public static void MainMenu()
        {
            Console.WriteLine("What would you like to do:\n 1 - Add an object\n 2 - Update an object\n 3 - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
            MenuOptions choice = (MenuOptions)int.Parse(Console.ReadLine());
            while (choice != MenuOptions.Exit)
            {
                switch (choice)
                {
                    case MenuOptions.Add:
                        AddMenu();
                        break;
                    case MenuOptions.Update:
                        UpdateMenu();
                        break;
                    case MenuOptions.DisplayOne:
                        DisplayOneMenu();
                        break;
                    case MenuOptions.DisplayList:
                        DisplayListMenu();
                        break;
                    case MenuOptions.FindDistance:
                        FindDistanceMenu();
                        break;
                }
                Console.WriteLine("\nWhat would you like to do:\n 1 - Add an object\n 2 - Update an object\n 3 - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
                choice = (MenuOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print add menu to user
        /// </summary>
        public static void AddMenu()
        {
            Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n 4 - Add a parcel\n 0 - Exit");
            AddOptions choice = (AddOptions)int.Parse(Console.ReadLine());
            string name = "";
            int id = 0;
            double lng = 0, lat = 0;
            while (choice != AddOptions.Exit)
            {
                switch (choice)
                {
                    case AddOptions.Station:
                        Console.WriteLine("Enter the ID, name, location, and number of available charging slots of the station:");
                        id = int.Parse(Console.ReadLine());
                        name = Console.ReadLine();
                        lng = double.Parse(Console.ReadLine());
                        lat = double.Parse(Console.ReadLine());
                        int num = int.Parse(Console.ReadLine());
                        DalObject.DalObject.AddStation(id, name, lng, lat, num);
                        break;

                    case AddOptions.Drone:
                        Console.WriteLine("Enter the ID, model, maximum weight (0-light, 1-mediun, 2-heavy), and battery of the drone:");
                        id = int.Parse(Console.ReadLine());
                        string model = Console.ReadLine();
                        IDAL.DO.WeightCategories max = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                        double battery = double.Parse(Console.ReadLine());
                        DalObject.DalObject.AddDrone(id, model, max, battery);
                        break;

                    case AddOptions.Customer:
                        Console.WriteLine("Enter the  ID, name, phone, location of the customer:");
                        id = int.Parse(Console.ReadLine());
                        name = Console.ReadLine();
                        string phone = Console.ReadLine();
                        lng = double.Parse(Console.ReadLine());
                        lat = double.Parse(Console.ReadLine());
                        DalObject.DalObject.AddCustomer(id, name, phone, lng, lat);
                        break;

                    case AddOptions.Parcel:
                        Console.WriteLine("Enter the sender's ID, target's ID, parcel weight (0-light, 1-mediun, 2-heavy), and priority (0-regular, 1-express, 2-urgent) of the parcel:");
                        int senderId = int.Parse(Console.ReadLine());
                        int targetId = int.Parse(Console.ReadLine());
                        IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                        IDAL.DO.Priorities priority = (IDAL.DO.Priorities)int.Parse(Console.ReadLine());
                        DalObject.DalObject.AddParcel(senderId, targetId, weight, priority);
                        break;
                }
                Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n 4 - Add a parcel\n 0 - Exit");
                choice = (AddOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the update menu to the user
        /// </summary>
        public static void UpdateMenu()
        {
            Console.WriteLine("\nWhat would you like to update:\n 1 - Assign drone to parcel \n 2 - Drone pick up of parcel\n 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n 6 - Update drone status\n 0 - Exit");
            UpdateOptions choice = (UpdateOptions)int.Parse(Console.ReadLine());
            int droneId = 0, parcelId = 0, stationId = 0;
            while (choice != UpdateOptions.Exit)
            {
                switch (choice)
                {
                    case UpdateOptions.ParcelToDrone:
                        Console.WriteLine("Pick a parcel:");
                        PrintAllNoDroneParcels();
                        Console.WriteLine("Enter the ID of the parcel and of the drone:");
                        parcelId = int.Parse(Console.ReadLine());
                        droneId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelDroneUpdate(parcelId, droneId);
                        break;

                    case UpdateOptions.PickUp:
                        Console.WriteLine("Enter the ID of the parcel:");
                        parcelId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelPickUp(parcelId);
                        break;

                    case UpdateOptions.Deliver:
                        Console.WriteLine("Enter the ID of the parcel:");
                        parcelId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelDelivered(parcelId);
                        break;

                    case UpdateOptions.Charge:
                        Console.WriteLine("Pick a station:");
                        PrintAllAvailableChargeStations();
                        Console.WriteLine("Enter the ID of the drone and the station:");
                        droneId = int.Parse(Console.ReadLine());
                        stationId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ChargeDrone(droneId, stationId);
                        DalObject.DalObject.UpdateChargeSlots(stationId, -1);
                        break;

                    case UpdateOptions.ReleaseCharge:
                        Console.WriteLine("Enter drone and station ID from the list below:");
                        PrintAllDronesCharging();
                        droneId = int.Parse(Console.ReadLine());
                        stationId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ReleaseDroneCharge(droneId, stationId);
                        break;

                    case UpdateOptions.DroneStatus:
                        Console.WriteLine("Enter the drone ID and the status (0 - Available, 1 - Maintenance, 2 - Shipping):");
                        droneId = int.Parse(Console.ReadLine());
                        IDAL.DO.DroneStatuses status = (IDAL.DO.DroneStatuses)int.Parse(Console.ReadLine());
                        DalObject.DalObject.UpdateDroneStatus(droneId, status);
                        break;
                }
                Console.WriteLine("\nWhat would you like to update:\n 1 - Assign drone to parcel \n 2 - Drone pick up of parcel\n 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n 6 - Update drone status\n 0 - Exit");
                choice = (UpdateOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the menu of display one to the user
        /// </summary>
        public static void DisplayOneMenu()
        {
            int id;
            Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
            DisplayOptions choice = (DisplayOptions)int.Parse(Console.ReadLine());
            while (choice != DisplayOptions.Exit)
            {
                switch (choice)
                {
                    case DisplayOptions.Station:
                        Console.WriteLine("Enter the station ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayStation(id));
                        break;

                    case DisplayOptions.Drone:
                        Console.WriteLine("Enter the drone ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayDrone(id));
                        break;

                    case DisplayOptions.Customer:
                        Console.WriteLine("Enter the customer ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayCustomer(id));
                        break;

                    case DisplayOptions.Parcel:
                        Console.WriteLine("Enter the parcel ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayParcel(id));
                        break;
                }
                Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
                choice = (DisplayOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the menu of list displayy to user
        /// </summary>
        public static void DisplayListMenu()
        {
            Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
            DisplayOptions choice = (DisplayOptions)int.Parse(Console.ReadLine());
            while (choice != DisplayOptions.Exit)
            {
                switch (choice)
                {
                    case DisplayOptions.Station:
                        PrintAllStations();
                        break;
                    case DisplayOptions.Drone:
                        PrintAllDrones();
                        break;
                    case DisplayOptions.Customer:
                        PrintAllCustomers();
                        break;
                    case DisplayOptions.Parcel:
                        PrintAllParcels();
                        break;
                    case DisplayOptions.NoDroneParcel:
                        PrintAllNoDroneParcels();
                        break;
                    case DisplayOptions.AvailableChargeStations:
                        PrintAllAvailableChargeStations();
                        break;
                }
                Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
                choice = (DisplayOptions)int.Parse(Console.ReadLine());
            }
        }

        public static void FindDistanceMenu()
        {
            Console.WriteLine("\nWhat distance do you want:\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");
            DistanceOptions choice = (DistanceOptions)int.Parse(Console.ReadLine());
            while (choice != DistanceOptions.Exit)
            {
                Console.WriteLine("Enter latitude and longitude:");
                double lat = double.Parse(Console.ReadLine());
                double lng = double.Parse(Console.ReadLine());
                int id;
                switch (choice)
                {
                    case DistanceOptions.Station:
                        Console.WriteLine("Enter station ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine($"The distance is: {DalObject.DalObject.FindDistanceStation(lat, lng, id)} km");
                        break;
                    case DistanceOptions.Customer:
                        Console.WriteLine("Enter customer ID:");
                        id = int.Parse(Console.ReadLine());
                        Console.WriteLine($"The distance is: {DalObject.DalObject.FindDistanceCustomer(lat, lng, id)} km");
                        break;
                }
                Console.WriteLine("\nWhat distance do you want:\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");
                choice = (DistanceOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// print all the stations
        /// </summary>
        public static void PrintAllStations()
        {
            List<Station> Stations = DalObject.DalObject.StationsDisplay();
            Stations.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the drones
        /// </summary>
        public static void PrintAllDrones()
        {
            List<Drone> Drones = DalObject.DalObject.DronesDisplay();
            Drones.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the customers
        /// </summary>
        public static void PrintAllCustomers()
        {
            List<Customer> Customers = DalObject.DalObject.CustomersDisplay();
            Customers.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the parcels
        /// </summary>
        public static void PrintAllParcels()
        {
            List<Parcel> Parcels = DalObject.DalObject.ParcelsDisplay();
            Parcels.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the parcels with no drone associated
        /// </summary>
        public static void PrintAllNoDroneParcels()
        {
            List<Parcel> Parcels = DalObject.DalObject.NoDroneParcels();
            Parcels.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the stations with empty charge slots
        /// </summary>
        public static void PrintAllAvailableChargeStations()
        {
            List<Station> Stations = DalObject.DalObject.EmptyChargeSlots();
            Stations.ForEach(x => { Console.WriteLine(x); });
        }
        /// <summary>
        /// print all the drones that are charging
        /// </summary>
        public static void PrintAllDronesCharging()
        {
            List<DroneCharge> DroneCharges = DalObject.DalObject.DroneChargeDisplay();
            DroneCharges.ForEach(x => { Console.WriteLine(x); });
        }
    }
}
