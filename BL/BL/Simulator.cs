using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;

namespace BL
{
    internal class Simulator
    {
        int timer = 1000;
        int cycle = 1; //one sec in cycle
        double speed = 0.5; //half a km per sec

        Drone drone;

        public Simulator(int id, Action updateDelegate, Func<bool> stopDelegate, BL myBL)
        {
            lock (myBL)
            {
                drone = myBL.GetDrone(id);
            }

            while (!stopDelegate())
            {
                switch (drone.Status)
                {
                    case DroneStatuses.Available:
                        lock (myBL)
                        {
                            try
                            {
                                myBL.AssignDroneToParcel(drone.Id);
                                drone = myBL.GetDrone(drone.Id);
                            }
                            catch (EmptyListException) { }
                            catch (NoBatteryException) //if there is not enough battery to make a delivery for any of the parcels
                            {
                                try
                                {
                                    //get the station that is nearest to the drone, among the stations that have available charge slots
                                    Station s = new Station();
                                    s = (from st in myBL.getListOfAvailableChargeSlotsStations()
                                         orderby getDistance(drone.CurrentLocation, st.Location)
                                         select st).FirstOrDefault();

                                    if (s != null) //if a station was found
                                    {
                                        Thread.Sleep((int)(BL.getDistance(s.Location, drone.CurrentLocation) / speed)); //update the drone only after the drone had enough time to reach the station
                                        myBL.ChargeDrone(drone.Id); //update the drone to charge
                                        drone = myBL.GetDrone(drone.Id);
                                        updateDelegate();
                                    }
                                }
                                //in both cases, the drone will wait for the next cycle and try again to see if there is 
                                //an available station for him to reach
                                //(there might be a case where there was a new parcel that the drone is able to carry)
                                catch (EmptyListException) { } //if there is no available charge slots try again in the next cycle
                                catch (NoBatteryException) { } //if there is not enough battery to get to the nearest station with available charge slots
                            }
                        }
                        break;

                    case DroneStatuses.Maintenance:
                        lock (myBL)
                        {
                            if (drone.Battery == 100)
                            {
                                myBL.ReleaseDroneCharge(drone.Id);
                                drone = myBL.GetDrone(drone.Id);
                            }
                            else
                            {
                                drone.Battery += cycle * myBL.DroneChargingRate;
                                if (drone.Battery > 100)
                                    drone.Battery = 100;
                                myBL.UpdateDrone(drone);
                            }
                        }
                        break;

                    case DroneStatuses.Shipping:
                        lock (myBL) lock (myBL.Data)
                            {
                                Parcel p = myBL.GetParcel(drone.InShipping.Id);

                                if (!drone.InShipping.IsPickedUp)
                                {
                                    if (drone.InShipping.DeliveryDistance / speed < ((TimeSpan)(DateTime.Now - p.Scheduled)).Seconds)
                                    {
                                        drone.Battery -= (speed * cycle) * myBL.AvailableConsumption; //d=v*t , substract from the battery according to distance
                                        drone.CurrentLocation = drone.InShipping.PickUpLocation; //update location
                                        drone.InShipping.IsPickedUp = true;
                                        drone.InShipping.DeliveryDistance = BL.getDistance(drone.InShipping.PickUpLocation, drone.InShipping.DeliveryLocation);
                                        myBL.Data.ParcelPickUp(p.Id); //updating the parcel in dal
                                        myBL.UpdateDrone(drone);
                                    }
                                    else
                                    {
                                        drone.Battery -= (speed * cycle) * myBL.AvailableConsumption; //d=v*t , substract from the battery according to distance
                                        myBL.UpdateDrone(drone);
                                    }
                                }
                                else
                                {
                                    if (drone.InShipping.DeliveryDistance / speed < ((TimeSpan)(DateTime.Now - p.PickedUp)).Seconds)
                                    {
                                        drone.Battery -= (speed * cycle) * myBL.ShippingConsumption[(int)drone.InShipping.Weight]; //d=v*t , substract from the battery according to distance
                                        drone.CurrentLocation = drone.InShipping.PickUpLocation; //update location
                                        drone.Status = DroneStatuses.Available; //make drone available
                                        drone.InShipping.Id = 0;
                                        myBL.Data.ParcelDelivered(p.Id); //updating the parcel in dal
                                        myBL.UpdateDrone(drone);
                                    }
                                    else
                                    {
                                        drone.Battery -= (speed * cycle) * myBL.ShippingConsumption[(int)drone.InShipping.Weight];
                                        myBL.UpdateDrone(drone);
                                    }
                                }
                            }
                        break;
                }
                updateDelegate();
                Thread.Sleep(timer);
            }
        }
    }
}