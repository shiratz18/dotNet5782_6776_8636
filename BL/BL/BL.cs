using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using BlApi;

namespace BL
{
    partial class BL : IBL
    {
        internal IDal Data;
        internal double AvailableConsumption, DroneChargingRate;
        internal double[] ShippingConsumption;
        internal List<ListDrone> Drones { get; set; }
        internal static Random R = new Random();

        private static BL instance = null;
        private static readonly object padlock = new object();

        public static BL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new BL();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        private BL()
        {
            //intializing data with a DalObject
            Data = DalFactory.GetDal();

            //getting electricity consumption of drones
            double[] temp = new double[5];
            temp = Data.GetDroneElectricityConsumption();
            AvailableConsumption = temp[0];
            ShippingConsumption = new double[3];
            ShippingConsumption[0] = temp[1];
            ShippingConsumption[1] = temp[2];
            ShippingConsumption[2] = temp[3];
            DroneChargingRate = temp[4];

            //intializing drone list
            Drones = new List<ListDrone>();

            //getting the list of all the drones
            IEnumerable<DO.Drone> tempDrones = Data.GetDroneList();
            //getting the list of all the parcels that have a drone but were not yet delivered
            IEnumerable<DO.Parcel> tempParcels = Data.GetParcelList(p => { return p.DroneId != 0 && p.Delivered == null; });
            //getting the lisat of all the stations
            IEnumerable<DO.Station> tempStations = Data.GetStationList();
            //getting the list if all the customers
            IEnumerable<DO.Customer> tempCustomers = Data.GetCustomerList();
            //getting the list of all drone charges
            IEnumerable<DO.DroneCharge> DroneCharges = Data.GetDroneChargeList();

            //for each drone in the list, copy ID, model and maximum weight to the list of drones of bll
            foreach (DO.Drone d in tempDrones)
            {
                Drones.Add(new ListDrone
                {
                    Id = d.Id,
                    Model = d.Model,
                    MaxWeight = (WeightCategories)d.MaxWeight,
                    Status = DroneStatuses.Available,
                    Active = true
                });
            }

            //checking all the parcels in dal to update drones that are currently shipping parcels
            foreach (DO.Parcel p in tempParcels) //fore each parcel in the list of parcels from dal
            {
                var drone = Drones.Find(d => d.Id == p.DroneId); //finding the index of the drone of the parcel in the list of drones
                if (drone == null)
                    throw new NoIDException($"Drone {p.DroneId} does not exist.");

                drone.Status = DroneStatuses.Shipping; //changing the status of the drone to be in shipping
                drone.ParcelId = p.Id; //updating the parcel of the drone to be this parcel

                DO.Customer sender = Data.GetCustomer(p.SenderId); //getting the sender of the package
                Location senderLoc = new Location { Longitude = sender.Longitude, Latitude = sender.Latitude }; //saving the location of the sender
                DO.Customer target = Data.GetCustomer(p.TargetId); //getting the sender of the package
                Location targetLoc = new Location { Longitude = target.Longitude, Latitude = target.Latitude }; //saving the location of the target

                //finding the location of the nearest station to the sender
                int station1 = nearestStationId(senderLoc); //getting the id of the nearest station
                Location station1Loc = new Location //saving the location of that station
                {
                    Longitude = Data.GetStation(station1).Longitude,
                    Latitude = Data.GetStation(station1).Latitude,
                };

                //finding the location of the nearest station to the target
                int station2 = nearestStationId(targetLoc); //getting the id of the nearest station
                Location station2Loc = new Location //saving the location of that station
                {
                    Longitude = Data.GetStation(station2).Longitude,
                    Latitude = Data.GetStation(station2).Latitude,
                };

                //the total distance the drone needs to go for the delivery
                double totalDIstance =
                    getDistance(station1Loc, senderLoc) + getDistance(senderLoc, targetLoc) + getDistance(senderLoc, station2Loc);

                //the battery will be a random number between the minimum battery needed to complete the delivery (according to weight and distance), and full battery
                drone.Battery = (double)R.Next((int)(totalDIstance * ShippingConsumption[(int)p.Weight]), 100);

                //if the parcel hasnt been picked up by a drone, the location is that of the station closest to the sender
                if (p.PickedUp == null)
                {
                    drone.CurrentLocation = station1Loc; //saving the location of the nearest station to the sender to be the location of the drone
                }
                //otherwise if it was picked up by a drone but not yet delivered to to the target then the location of the drone is the location of the sender
                else
                {
                    drone.CurrentLocation = senderLoc; //saving the location of the drone to be the location of the sender
                }
            }

            //checking all the drones that are charging to update their status and location
            foreach (DO.DroneCharge dc in DroneCharges)
            {
                ListDrone drone = Drones.Find(d => d.Id == dc.DroneId);
                if (drone == null)
                    throw new NoIDException($"Drone {dc.DroneId} does not exist.");

                drone.Status = DroneStatuses.Maintenance; //update the status to be maintenance
                drone.Battery = (DateTime.Now - dc.ChargingBegin).Seconds * DroneChargingRate < 100 ?
                    (DateTime.Now - dc.ChargingBegin).Seconds * DroneChargingRate : 100;

                DO.Station station = Data.GetStation(dc.StationId);
                drone.CurrentLocation = new Location() //updating the drone location
                {
                    Longitude = station.Longitude,
                    Latitude = station.Latitude
                };
            }

            //updating info about the drones that are not shipping
            Drones.ForEach(d => //for each drone in the list of drones
            {
                int rand;

                //if the drone status is available, the location is a random customer, battery is enough to reach the nearest station-100%
                if (d.Status == DroneStatuses.Available)
                {
                    //the location is that of a random customer that has had a parcel dlievered to him
                    rand = R.Next(0, tempCustomers.Count() + 1); //get a random number in the range of the size of the customer list
                    while (!tempParcels.Any(p => p.TargetId == tempCustomers.ElementAt(rand).Id)) //while not any of the parcel's target ID is the same as the customer
                        rand = R.Next(0, tempCustomers.Count() + 1); //get the next random numbwe
                    d.CurrentLocation = new Location
                    {
                        //getting location of a customer from a random index, which has had a parcel delivered to him
                        Longitude = tempCustomers.ElementAt(rand).Longitude,
                        Latitude = tempCustomers.ElementAt(rand).Latitude
                    };

                    //the battery is between minimum battery to get to a charging station and 100%
                    int id = nearestStationId(d.CurrentLocation);
                    Location temp = new Location
                    {
                        Longitude = Data.GetStation(id).Longitude,
                        Latitude = Data.GetStation(id).Latitude,
                    };
                    d.Battery = R.Next((int)(getDistance(d.CurrentLocation, temp) * AvailableConsumption), 100); //battery between the minimum battery needed to get to the nearest station, and full battery
                }
            });
        }

    }
}
