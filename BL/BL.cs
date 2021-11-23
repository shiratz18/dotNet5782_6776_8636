using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL : IBL
    {
        internal IDAL.IDal data;
        internal double AvailableConsumption, LightWeightConsumption, MediumWeightConsumption, HeavyWeightConsumption, DroneChargingRate;
        internal List<ListDrone> Drones { get; set; }
        internal static Random R = new Random();

        /// <summary>
        /// constructor
        /// </summary>
        public BL()
        {
            //intializing data with a DalObject
            data = new DalObject.DalObject();

            //getting electricity consumption of drones
            double[] temp = new double[5];
            temp = data.GetDroneElectricityConsumption();
            AvailableConsumption = temp[0];
            LightWeightConsumption = temp[1];
            MediumWeightConsumption = temp[2];
            HeavyWeightConsumption = temp[3];
            HeavyWeightConsumption = temp[4];
            DroneChargingRate = temp[5];

            IEnumerable<IDAL.DO.Drone> tempDrones = data.GetDroneList();
            IEnumerable<IDAL.DO.Parcel> tempParcels = data.GetParcelList();
            IEnumerable<IDAL.DO.Station> tempStations = data.GetStationList();
            IEnumerable<IDAL.DO.Customer> tempCustomers = data.GetCustomerList();

            //for each drone in the list, copy ID, model and maximum weight to the list of drones of bll
            foreach (IDAL.DO.Drone d in tempDrones)
            {
                Drones.Add(new ListDrone
                {
                    Id = d.Id,
                    Model = d.Model,
                    MaxWeight = (WeightCategories)d.MaxWeight,
                    Status = (DroneStatuses)(R.Next(0, 2)) //intialize status to be available or maintenance
                });
            }

            //checking all the parcels in dal to update drones that are currently shipping parcels
            foreach (IDAL.DO.Parcel p in tempParcels) //fore each parcel in the list of parcels from dal
            {
                //if the parcel has a drone assigned but was not yet delivered
                if (p.DroneId != 0 && p.Delivered == DateTime.MinValue)
                {
                    int index = Drones.FindIndex(d => d.Id == p.DroneId); //finding the index of the drone of the parcel in the list of drones

                    Drones[index].Status = DroneStatuses.Shipping; //changing the status of the drone to be in shipping

                    IDAL.DO.Customer sender = data.GetCustomer(p.SenderId); //getting the sender of the package
                    Location senderLoc = new Location { Longitude = sender.Longitude, Latitude = sender.Latitude }; //saving the location of the sender
                    IDAL.DO.Customer target = data.GetCustomer(p.TargetId); //getting the sender of the package
                    Location targetLoc = new Location { Longitude = target.Longitude, Latitude = target.Latitude }; //saving the location of the target

                    //finding the location of the nearest station to the sender
                    int station1 = nearestStationId(senderLoc); //getting the id of the nearest station
                    Location station1Loc = new Location //saving the location of that station
                    {
                        Longitude = data.GetStation(station1).Longitude,
                        Latitude = data.GetStation(station1).Latitude,
                    };

                    //finding the location of the nearest station to the target
                    int station2 = nearestStationId(targetLoc); //getting the id of the nearest station
                    Location station2Loc = new Location //saving the location of that station
                    {
                        Longitude = data.GetStation(station2).Longitude,
                        Latitude = data.GetStation(station2).Latitude,
                    };

                    //the total distance the drone needs to go for the delivery
                    double totalDIstance =
                        getDistance(station1Loc, senderLoc) + getDistance(senderLoc, targetLoc) + getDistance(senderLoc, station2Loc);

                    //the battery will be a random number between the minimum battery needed to complete the delivery (according to weight and distance), and full battery
                    switch ((WeightCategories)p.Weight)
                    {
                        case WeightCategories.Light:
                            Drones[index].Battery = (double)R.Next((int)(totalDIstance * LightWeightConsumption), 100);
                            break;
                        case WeightCategories.Medium:
                            Drones[index].Battery = (double)R.Next((int)(totalDIstance * MediumWeightConsumption), 100);
                            break;
                        case WeightCategories.Heavy:
                            Drones[index].Battery = (double)R.Next((int)(totalDIstance * HeavyWeightConsumption), 100);
                            break;
                    }

                    //if the parcel hasnt been picked up by a drone, the location is that of the station closest to the sender
                    if (p.PickedUp == DateTime.MinValue)
                    {
                        Drones[index].CurrentLocation = station1Loc; //saving the location of the nearest station to the sender to be the location of the drone
                    }
                    //otherwise if it was picked up by a drone but not yet delivered to to the target then the location of the drone is the location of the sender
                    else
                    {
                        Drones[index].CurrentLocation = senderLoc; //saving the location of the drone to be the location of the sender
                    }
                }
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
                        Longitude = data.GetStation(id).Longitude,
                        Latitude = data.GetStation(id).Latitude,
                    };
                    d.Battery = R.Next((int)(getDistance(d.CurrentLocation, temp) * AvailableConsumption), 100); //battery between the minimum battery needed to get to the nearest station, and full battery
                }

                //if the drone is in maintenance, the location is a random station, the battery is betweer 0-20%
                else if (d.Status == DroneStatuses.Maintenance)
                {
                    rand = R.Next(0, tempStations.Count() + 1); //a ranodm number in the range of the station list size
                    d.CurrentLocation = new Location
                    {
                        //getting the location of a station from a random index
                        Longitude = tempStations.ElementAt(rand).Longitude,
                        Latitude = tempStations.ElementAt(rand).Latitude
                    };

                    //battery is between 0-20%
                    d.Battery = R.Next(0, 21);
                }
            });
        }

    }
}
