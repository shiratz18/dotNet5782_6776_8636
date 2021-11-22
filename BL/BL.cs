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

            List<IDAL.DO.Drone> tempDrones = (List<IDAL.DO.Drone>)data.GetDroneList();
            List<IDAL.DO.Parcel> tempParcels = (List<IDAL.DO.Parcel>)data.GetParcelList();
            List<IDAL.DO.Station> tempStations = (List<IDAL.DO.Station>)data.GetStationList();
            List<IDAL.DO.Customer> tempCustomers = (List<IDAL.DO.Customer>)data.GetCustomerList();

            tempDrones.ForEach(d => //for each drone in the data source copy ID, model and maximum weight to the list of drones
            {
                Drones.Add(new ListDrone
                {
                    Id = d.Id,
                    Model = d.Model,
                    MaxWeight = (WeightCategories)d.MaxWeight,
                    Status = (DroneStatuses)(R.Next(0, 2)) //intialize status to be available or maintenance
                });
            });

            //checking all the parcels to get the drones that are currently shipping parcels
            tempParcels.ForEach(p => //fore each parcel in the list of parcels from data
            {
                if (p.DroneId != 0 && p.Delivered == default) //if the parcel has a drone assigned but was not yet delivered
                {
                    int index = Drones.FindIndex(d => d.Id == p.DroneId); //finding the index of the drone of the parcel in the list of drones

                    Drones[index].Status = DroneStatuses.Shipping; //changing the status of the drone to be in shipping

                    IDAL.DO.Customer sender = data.GetCustomer(p.SenderId); //getting the sender of the package
                    Location senderLoc = new Location { Longitude = sender.Longitude, Latitude = sender.Latitude }; //saving the location of the sender
                    IDAL.DO.Customer target = data.GetCustomer(p.TargetId); //getting the sender of the package
                    Location targetLoc = new Location { Longitude = target.Longitude, Latitude = target.Latitude }; //saving the location of the sender

                    //finding the location of the nearest station to the sender
                    int station1 = Distance.NearestStationId(senderLoc, tempStations); //getting the id of the station
                    Location station1Loc = new Location //saving the location of that station
                    {
                        Longitude = data.GetStation(station1).Longitude,
                        Latitude = data.GetStation(station1).Latitude,
                    };

                    //finding the location of the nearest station to the target
                    int station2 = Distance.NearestStationId(targetLoc, tempStations); //getting the id of the station
                    Location station2Loc = new Location //saving the location of that station
                    {
                        Longitude = data.GetStation(station2).Longitude,
                        Latitude = data.GetStation(station2).Latitude,
                    };

                    //the total distance the drone needs to go for the delivery
                    double totalDIstance = Distance.GetDistance(station1Loc, senderLoc) + Distance.GetDistance(senderLoc, targetLoc) + Distance.GetDistance(senderLoc, station2Loc);

                    //the battery will be a random number between the minimum battery needed to complete the delivery (according to weight and distance), and full battery
                    if ((WeightCategories)p.Weight == WeightCategories.Light)
                    {
                        Drones[index].Battery = (double)R.Next((int)(totalDIstance * LightWeightConsumption), 100);
                    }
                    else if ((WeightCategories)p.Weight == WeightCategories.Medium)
                    {
                        Drones[index].Battery = (double)R.Next((int)(totalDIstance * MediumWeightConsumption), 100);
                    }
                    else
                    {
                        Drones[index].Battery = (double)R.Next((int)(totalDIstance * HeavyWeightConsumption), 100);
                    }

                    //if the parcel hasnt been picked up by a drone, the location is that of the station closest to the sender
                    if (p.PickedUp == default)
                    {
                        Drones[index].CurrentLocation = station1Loc; //saving the location of the nearest station to the sender to be the location of the drone
                    }
                    else //if it was picked up by a drone but not yet delivered to to the target then the location of the drone is the location of the sender
                    {
                        Drones[index].CurrentLocation = senderLoc; //saving the location of the drone to be the location of the sender
                    }
                }
            });

            //updating info about the drones that are not shipping
            Drones.ForEach(d => //for each drone in the list of drones
            {
                int rand;
                //if the drone status is available, the location is a random customer, battery is enough to reach the nearest station-100%
                if (d.Status == DroneStatuses.Available)
                {
                    rand = R.Next(0, tempCustomers.Count + 1);
                    d.CurrentLocation = new Location
                    {
                        Longitude = tempCustomers[rand].Longitude,
                        Latitude = tempCustomers[rand].Latitude
                    };
                    int id = Distance.NearestStationId(d.CurrentLocation, tempStations);
                    Location temp = new Location
                    {
                        Longitude = data.GetStation(id).Longitude,
                        Latitude = data.GetStation(id).Latitude,
                    };
                    d.Battery = R.Next((int)(Distance.GetDistance(d.CurrentLocation, temp) * AvailableConsumption), 100); //battery between the minimum battery needed to get to the nearest station, and full battery
                }

                //if the drone is in maintenance, the location is a random station, the battery is betweer 0-20%
                else if (d.Status == DroneStatuses.Maintenance)
                {
                    rand = R.Next(0, tempStations.Count + 1);
                    d.CurrentLocation = new Location
                    {
                        Longitude = tempStations[rand].Longitude,
                        Latitude = tempStations[rand].Latitude
                    };
                    d.Battery = R.Next(0, 21);
                }
            });
        }

    }
}
