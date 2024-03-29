﻿using System;
using System.Collections.Generic;
using BO;
using BlApi;

namespace ConsoleUI_BL
{
    class ConsoleUI_BL
    {
        #region Menu options enum
        public enum MenuOptions { Exit, Add, Update, DisplayOne, DisplayList }
        public enum AddOptions { Exit, Station, Drone, Customer, Parcel }
        public enum UpdateOptions { Exit, Drone, Station, Customer, DroneCharge, ReleaseCharge, ParcelToDrone, PickUp, Deliver }
        public enum DisplayOptions { Exit, Station, Drone, Customer, Parcel, NoDroneParcel, AvailableChargeStations }
        public enum DistanceOptions { Exit, Station, Customer }
        #endregion

        static void Main(string[] args)
        {
            IBL myBL = BlFactory.GetBl();

            MainMenu(myBL);
        }

        #region Main menu
        /// <summary>
        /// the main menu to display to the user
        /// </summary>
        /// <param name="myBL">the BL object</param>
        static void MainMenu(IBL myBL)
        {
            Console.WriteLine("What would you like to do:\n 1 - Add an object\n 2 - Update an object\n 3" +
                " - Display an object\n 4 - Display a list of objects\n 0 - Exit");
            MenuOptions.TryParse(Console.ReadLine(), out MenuOptions choice);
            while (choice != MenuOptions.Exit)
            {
                switch (choice)
                {
                    case MenuOptions.Add:
                        AddMenu(myBL);
                        break;
                    case MenuOptions.Update:
                        UpdateMenu(myBL);
                        break;
                    case MenuOptions.DisplayOne:
                        DisplayOneMenu(myBL);
                        break;
                    case MenuOptions.DisplayList:
                        DisplayListMenu(myBL);
                        break;
                }
                Console.WriteLine("\nWhat would you like to do:\n 1 - Add an object\n 2 - Update an object\n" +
                    " 3 - Display an object\n 4 - Display a list of objects\n 0 - Exit");
                MenuOptions.TryParse(Console.ReadLine(), out choice);
            }
        }
        #endregion

        #region Add menu
        public static void AddMenu(IBL myBL)
        {
            Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n" +
                " 4 - Add a parcel\n 0 - Exit");
            AddOptions.TryParse(Console.ReadLine(), out AddOptions choice);

            int id = 0, num = 0;
            string name = "";
            double lng = 0, lat = 0;
            Location loc = new Location();

            while (choice != AddOptions.Exit)
            {
                switch (choice)
                {
                    case AddOptions.Station:
                        Console.WriteLine("Enter the ID, name, location, and number charging slots of the station:");
                        int.TryParse(Console.ReadLine(), out id);
                        name = Console.ReadLine();
                        double.TryParse(Console.ReadLine(), out lat);
                        loc.Latitude = lat;
                        double.TryParse(Console.ReadLine(), out lng);
                        loc.Longitude = lng;
                        int.TryParse(Console.ReadLine(), out num);
                        Station station = new Station()
                        {
                            Id = id,
                            Name = name,
                            Location = loc,
                            AvailableChargeSlots = num,
                            ChargingDrones = new List<ChargingDrone>()
                        };

                        try
                        {
                            myBL.AddStation(station);
                        }
                        catch (InvalidNumberException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DoubleIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case AddOptions.Drone:
                        Console.WriteLine("Enter the ID, model, maximum weight (0-light, 1-mediun, 2-heavy), and station number for the drone:");
                        int.TryParse(Console.ReadLine(), out id);
                        name = Console.ReadLine();
                        WeightCategories.TryParse(Console.ReadLine(), out WeightCategories maxWeight);
                        int.TryParse(Console.ReadLine(), out num);
                        Drone drone = new Drone()
                        {
                            Id = id,
                            Model = name,
                            MaxWeight = maxWeight
                        };

                        try
                        {
                            myBL.AddDrone(drone, num);
                        }
                        catch (InvalidNumberException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DoubleIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (NoAvailableChargeSlotsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case AddOptions.Customer:
                        Console.WriteLine("Enter the  ID, name, phone, location of the customer:");
                        int.TryParse(Console.ReadLine(), out id);
                        name = Console.ReadLine();
                        string phone = Console.ReadLine();
                        double.TryParse(Console.ReadLine(), out lat);
                        loc.Latitude = lat;
                        double.TryParse(Console.ReadLine(), out lng);
                        loc.Longitude = lng;
                        Customer customer = new Customer()
                        {
                            Id = id,
                            Name = name,
                            Phone = phone,
                            Location = loc,
                            FromCustomer = new List<ParcelAtCustomer>(),
                            ToCustomer = new List<ParcelAtCustomer>()
                        };

                        try
                        {
                            myBL.AddCustomer(customer);
                        }
                        catch (InvalidNumberException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (WrongFormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DoubleIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case AddOptions.Parcel:
                        Console.WriteLine("Enter the sender's ID, target's ID, parcel weight (0-light, 1-mediun, 2-heavy)," +
                        " and priority (0-regular, 1-express, 2-urgent) of the parcel:");
                        int.TryParse(Console.ReadLine(), out id); //9 digits, customer exists
                        CustomerInParcel sender = new CustomerInParcel { Id = id };
                        int.TryParse(Console.ReadLine(), out id); //9 didigts, customer exists
                        CustomerInParcel target = new CustomerInParcel { Id = id };
                        WeightCategories.TryParse(Console.ReadLine(), out WeightCategories weight);
                        Priorities.TryParse(Console.ReadLine(), out Priorities priority);
                        Parcel parcel = new Parcel()
                        {
                            Sender = sender,
                            Target = target,
                            Weight = weight,
                            Priority = priority
                        };

                        try
                        {
                            myBL.AddParcel(parcel);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DoubleIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
                Console.WriteLine("\nWhat would you like to add:\n 1 - Add a station \n 2 - Add a drone\n 3 - Add a customer\n" +
               " 4 - Add a parcel\n 0 - Exit");
                AddOptions.TryParse(Console.ReadLine(), out choice);
            }
        }
        #endregion

        #region Update menu
        static void UpdateMenu(IBL myBL)
        {
            Console.WriteLine("\nWhat would you like to update:\n 1 - Update a drone\n 2 - Update a station\n 3 - Update a customer\n" +
                " 4 - Send a drone to charge\n 5 - Release a drone from charge\n 6 - Assign drone to parcel \n 7 - Drone pick up of parcel\n" +
                " 8 - Deliver parcel to customer\n 0 - Exit");
            UpdateOptions.TryParse(Console.ReadLine(), out UpdateOptions choice);

            int id = 0, num = 0;
            string name = "", phone = "";

            while (choice != UpdateOptions.Exit)
            {
                switch (choice)
                {
                    case UpdateOptions.Drone:
                        Console.WriteLine("Enter the ID of the drone and the new name:");
                        int.TryParse(Console.ReadLine(), out id); // drones exists
                        name = Console.ReadLine();
                        try
                        {
                            myBL.UpdateDroneName(id, name);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case UpdateOptions.Station:
                        Console.WriteLine("Enter the station number, the new name and/or the new number of charging slots:");
                        int.TryParse(Console.ReadLine(), out id); // station exists
                        name = Console.ReadLine(); //alphabet, name doesnt exist
                        string number = Console.ReadLine(); //positive,check that there are enough slots for the drones

                        if (!String.IsNullOrEmpty(name)) //if he entered a name to update
                        {
                            try
                            {
                                myBL.UpdateStationName(id, name);
                            }
                            catch (NoIDException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (!String.IsNullOrEmpty(number)) //if he entered a number to update
                        {
                            try
                            {
                                num = Int32.Parse(number); //convert the string to int
                                myBL.UpdateStationChargingSlots(id, num); //update the number of charging slots
                            }
                            catch (NoIDException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (InvalidNumberException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;

                    case UpdateOptions.Customer:
                        Console.WriteLine("Enter the customer ID, the new name and/or the new phone number:");
                        int.TryParse(Console.ReadLine(), out id);
                        name = Console.ReadLine();
                        phone = Console.ReadLine();

                        if (!String.IsNullOrEmpty(phone))
                        {
                            try
                            {
                                myBL.UpdateCustomerName(id, name);
                            }
                            catch (WrongFormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (NoIDException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (!String.IsNullOrEmpty(phone))
                        {
                            try
                            {
                                myBL.UpdateCustomerPhone(id, phone);
                            }
                            catch (WrongFormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (NoIDException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;

                    case UpdateOptions.DroneCharge:
                        Console.WriteLine("Enter the ID of the drone:");
                        int.TryParse(Console.ReadLine(), out id);

                        try
                        {
                            myBL.ChargeDrone(id);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DroneStateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (NoBatteryException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case UpdateOptions.ReleaseCharge:
                        Console.WriteLine("Enter the drone ID and the charging time:");
                        int.TryParse(Console.ReadLine(), out id);
                        TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time);
                        try
                        {
                            myBL.ReleaseDroneCharge(id);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DroneStateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case UpdateOptions.ParcelToDrone:
                        Console.WriteLine("Enter the drone ID:");
                        int.TryParse(Console.ReadLine(), out id); //, drone exists, drone available

                        try
                        {
                            myBL.AssignDroneToParcel(id);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DroneStateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case UpdateOptions.PickUp:
                        Console.WriteLine("Enter the drone ID:");
                        int.TryParse(Console.ReadLine(), out id); // drone exists, drone assigned to parcel

                        try
                        {
                            myBL.DronePickUp(id);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DroneStateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case UpdateOptions.Deliver:
                        Console.WriteLine("Enter the drone ID:");
                        int.TryParse(Console.ReadLine(), out id); //drone exists, drone picked up parcel

                        try
                        {
                            myBL.DroneDeliver(id);
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (DroneStateException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
                Console.WriteLine("\nWhat would you like to update:\n 1 - Update a drone\n 2 - Update a station\n 3 - Update a customer\n" +
               " 4 - Send a drone to charge\n 5 - Release a drone from charge\n 6 - Assign drone to parcel \n 7 - Drone pick up of parcel\n" +
               " 8 - Deliver parcel to customer\n 0 - Exit");
                UpdateOptions.TryParse(Console.ReadLine(), out choice);
            }
        }
        #endregion

        #region Display menu
        static void DisplayOneMenu(IBL myBL)
        {
            Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
            DisplayOptions choice;
            DisplayOptions.TryParse(Console.ReadLine(), out choice);

            int id;

            while (choice != DisplayOptions.Exit)
            {
                switch (choice)
                {
                    case DisplayOptions.Station:
                        Console.WriteLine("Enter the station ID:");
                        int.TryParse(Console.ReadLine(), out id);
                        try
                        {
                            Console.WriteLine(myBL.GetStation(id));
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Drone:
                        Console.WriteLine("Enter the drone ID:");
                        int.TryParse(Console.ReadLine(), out id);
                        try
                        {
                            Console.WriteLine(myBL.GetDrone(id));
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Customer:
                        Console.WriteLine("Enter the customer ID:");
                        int.TryParse(Console.ReadLine(), out id);
                        try
                        {
                            Console.WriteLine(myBL.GetCustomer(id));
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Parcel:
                        Console.WriteLine("Enter the parcel ID:");
                        int.TryParse(Console.ReadLine(), out id);
                        try
                        {
                            Console.WriteLine(myBL.GetParcel(id));
                        }
                        catch (NoIDException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
                Console.WriteLine("\nWhat would you like to display:\n 1 - Station \n 2 - Drone\n 3 - Customer\n 4 - Parcel\n 0 - Exit");
                DisplayOptions.TryParse(Console.ReadLine(), out choice);
            }
        }
        #endregion

        #region Display list menu
        public static void DisplayListMenu(IBL myBL)
        {
            Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n" +
                " 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
            DisplayOptions choice;
            DisplayOptions.TryParse(Console.ReadLine(), out choice);

            while (choice != DisplayOptions.Exit)
            {
                switch (choice)
                {
                    case DisplayOptions.Station:
                        try
                        {
                            foreach (ListStation ls in myBL.GetStationList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Drone:
                        try
                        {
                            foreach (ListDrone ls in myBL.GetDroneList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Customer:
                        try
                        {
                            foreach (ListCustomer ls in myBL.GetCustomerList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.Parcel:
                        try
                        {
                            foreach (ListParcel ls in myBL.GetParcelList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.NoDroneParcel:
                        try
                        {
                            foreach (ListParcel ls in myBL.GetNoDroneParcelList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case DisplayOptions.AvailableChargeStations:
                        try
                        {
                            foreach (ListStation ls in myBL.GetAvailableChargeSlotsStationList())
                            {
                                Console.WriteLine(ls);
                            }
                        }
                        catch (EmptyListException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
                Console.WriteLine("\nWhat would you like to display:\n 1 - Stations \n 2 - Drones\n 3 - Customers\n 4 - Parcels\n" +
                    " 5 - Parcels without drone\n 6 - Stations with available chargers\n 0 - Exit");
                DisplayOptions.TryParse(Console.ReadLine(), out choice);
            }
            #endregion
        }
    }
}