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
        public enum UpdateOptions { Exit, ParcelToDrone, PickUp, Deliver, Charge, ReleaseCharge }
        public enum DisplayOptions { Exit, Station, Drone, Customer, Parcel, NoDroneParcel, AvailableChargeStations }
        public enum DistanceOptions { Exit, Station, Customer }

        static void Main(string[] args)
        {
            new DalObject.DalObject();
            ShowMenu();
        }

        /// <summary>
        /// Print main menu to user
        /// </summary>
        public static void ShowMenu()
        {
            Console.WriteLine("What would you like to do\n 1 - Add an object\n 2 - Update an object\n 3 - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
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
                Console.WriteLine("\nWhat would you like to do\n 1 - Add an object\n 2 - Update an object\n 3 - Display an object\n 4 - Display a list of objects\n 5 - Find distance\n 0 - Exit");
                choice = (MenuOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print add menu to user
        /// </summary>
        public static void AddMenu()
        {
            Console.WriteLine("\nWhat would you like to do\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n 4 - Add a parcel\n 0 - Exit");
            AddOptions choice = (AddOptions)int.Parse(Console.ReadLine());
            while (choice != AddOptions.Exit)
            {
                switch (choice)
                {
                    case AddOptions.Station:
                        Console.WriteLine("Enter the ID, name, location, and number of available charging slots of the station:");
                        int StationId = int.Parse(Console.ReadLine());
                        string Name = Console.ReadLine();
                        double Lng = double.Parse(Console.ReadLine());
                        double Lat = double.Parse(Console.ReadLine());
                        int Num = int.Parse(Console.ReadLine());
                        DalObject.DalObject.AddStation(StationId, Name, Lng, Lat, Num);
                        break;
                    case AddOptions.Drone:
                        Console.WriteLine("Enter the ID, model, maximum weight (0-light, 1-mediun, 2-heavy), and battery of the drone:");
                        int DroneId = int.Parse(Console.ReadLine());
                        string Model = Console.ReadLine();
                        IDAL.DO.WeightCategories Max = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                        double Battery = double.Parse(Console.ReadLine());
                        DalObject.DalObject.AddDrone(DroneId, Model, Max, Battery);
                        break;
                    case AddOptions.Customer:
                        Console.WriteLine("Enter the  ID, name, phone, location of the customer:");
                        int CustomerId = int.Parse(Console.ReadLine());
                        string CustomerName = Console.ReadLine();
                        string Phone = Console.ReadLine();
                        double CustomerLongitude = double.Parse(Console.ReadLine());
                        double CustomerLatitude = double.Parse(Console.ReadLine());
                        DalObject.DalObject.AddCustomer(CustomerId, CustomerName, Phone, CustomerLongitude, CustomerLatitude);
                        break;
                    case AddOptions.Parcel:
                        Console.WriteLine("Enter the sender's ID, target's ID, parcel weight (0-light, 1-mediun, 2-heavy), and priority (0-regular, 1-express, 2-urgent) of the parcel:");
                        int SenderId = int.Parse(Console.ReadLine());
                        int TargetId = int.Parse(Console.ReadLine());
                        IDAL.DO.WeightCategories Weight = (IDAL.DO.WeightCategories)int.Parse(Console.ReadLine());
                        IDAL.DO.Priorities Priority = (IDAL.DO.Priorities)int.Parse(Console.ReadLine());
                        DalObject.DalObject.AddParcel(SenderId, TargetId, Weight, Priority);
                        break;
                }
                Console.WriteLine("\nWhat would you like to do\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n 4 - Add a parcel\n 0 - Exit");
                choice = (AddOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the update menu to the user
        /// </summary>
        public static void UpdateMenu()
        {
            Console.WriteLine("\nWhat would you like to do\n 1 - Associate drone to parcel \n 2 - Drone pick up of parcel\n 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n 0 - Exit");
            UpdateOptions choice = (UpdateOptions)int.Parse(Console.ReadLine());
            int DroneId = 0, ParcelId = 0, StationId = 0;
            while (choice != UpdateOptions.Exit)
            {
                switch (choice)
                {
                    case UpdateOptions.ParcelToDrone:
                        Console.WriteLine("Pick a parcel:");
                        PrintAllNoDroneParcels();
                        Console.WriteLine("Enter the ID of the parcel and of the drone:");
                        ParcelId = int.Parse(Console.ReadLine());
                        DroneId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelDroneUpdate(ParcelId, DroneId);
                        break;
                    case UpdateOptions.PickUp:
                        Console.WriteLine("Enter the ID of the parcel:");
                        ParcelId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelPickUp(ParcelId);
                        break;
                    case UpdateOptions.Deliver:
                        Console.WriteLine("Enter the ID of the parcel:");
                        ParcelId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ParcelDelivered(ParcelId);
                        break;
                    case UpdateOptions.Charge:
                        Console.WriteLine("Pick a station:");
                        PrintAllAvailableChargeStations();
                        Console.WriteLine("Enter the ID of the drone and the station:");
                        DroneId = int.Parse(Console.ReadLine());
                        StationId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.DroneCharge(DroneId, StationId);
                        break;
                    case UpdateOptions.ReleaseCharge:
                        Console.WriteLine("Enter the ID of the drone and the station:");
                        DroneId = int.Parse(Console.ReadLine());
                        StationId = int.Parse(Console.ReadLine());
                        DalObject.DalObject.ReleaseDroneCharge(DroneId, StationId);
                        break;
                }
                Console.WriteLine("\nWhat would you like to do\n 1 - Associate drone to parcel \n 2 - Drone pick up of parcel\n 3 - Deliver parcel to customer\n 4 - Send a drone to charge\n 5 - Release a drone from charge\n 0 - Exit");
                choice = (UpdateOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the menu of display one to the user
        /// </summary>
        public static void DisplayOneMenu()
        {
            int Id;
            Console.WriteLine("\nWhat would you like to display\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
            DisplayOptions choice = (DisplayOptions)int.Parse(Console.ReadLine());
            while (choice != DisplayOptions.Exit)
            {
                switch (choice)
                {
                    case DisplayOptions.Station:
                        Console.WriteLine("Enter the station ID:");
                        Id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayStation(Id));
                        break;
                    case DisplayOptions.Drone:
                        Console.WriteLine("Enter the drone ID:");
                        Id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayDrone(Id));
                        break;
                    case DisplayOptions.Customer:
                        Console.WriteLine("Enter the customer ID:");
                        Id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayCustomer(Id));
                        break;
                    case DisplayOptions.Parcel:
                        Console.WriteLine("Enter the parcel ID:");
                        Id = int.Parse(Console.ReadLine());
                        Console.WriteLine(DalObject.DalObject.DisplayParcel(Id));
                        break;
                }
                Console.WriteLine("\nWhat would you like to display\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
                choice = (DisplayOptions)int.Parse(Console.ReadLine());
            }
        }

        /// <summary>
        /// Print the menu of list displayy to user
        /// </summary>
        public static void DisplayListMenu()
        {
            Console.WriteLine("\nWhat would you like to display\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
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
                Console.WriteLine("\nWhat would you like to display\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
                choice = (DisplayOptions)int.Parse(Console.ReadLine());
            }
        }

        public static void FindDistanceMenu()
        {
            Console.WriteLine("\nWhat distance do you want?\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");
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
                        DalObject.DalObject.FindDistanceStation(lat, lng, id);
                        break;
                    case DistanceOptions.Customer:
                        Console.WriteLine("Enter customer ID:");
                        id = int.Parse(Console.ReadLine());
                        DalObject.DalObject.FindDistanceCustomer(lat, lng, id);
                        break;
                }
                Console.WriteLine("\nWhat distance do you want?\n 1 - Location and station\n 2 - Location and customer\n 0 - Exit");
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
    }
}
