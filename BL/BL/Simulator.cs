using BlApi;
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
        double speed = 0.5;

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
                                    myBL.ChargeDrone(drone.Id);
                                    var tmp = myBL.GetDrone(drone.Id).CurrentLocation;
                                    Thread.Sleep((int)(BL.getDistance(tmp, drone.CurrentLocation) / speed)); //update the drone only after the drone has reached the station
                                    drone = myBL.GetDrone(drone.Id);
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
                        lock (myBL)
                        {
                            Parcel p = myBL.GetParcel(drone.InShipping.Id);

                            if (!drone.InShipping.IsPickedUp)
                            {
                                if (drone.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.Scheduled)).Seconds)
                                {
                                    myBL.DronePickUp(drone.Id);
                                    drone = myBL.GetDrone(drone.Id);
                                }

                                else
                                {
                                    drone.Battery -= 0.5 * myBL.AvailableConsumption;

                                    ////find the distance of the drone according to the timespan that has passed
                                    //double lat = (drone.InShipping.DeliveryLocation.Latitude - drone.CurrentLocation.Latitude)
                                    //    * (0.5 / BL.getDistance(drone.CurrentLocation, drone.InShipping.DeliveryLocation));
                                    //double lng = (drone.InShipping.DeliveryLocation.Longitude - drone.CurrentLocation.Longitude)
                                    //    * (0.5 / BL.getDistance(drone.CurrentLocation, drone.InShipping.DeliveryLocation));

                                    //drone.CurrentLocation.Latitude = lat;
                                    //drone.CurrentLocation.Longitude = lng;
                                    myBL.UpdateDrone(drone);
                                }
                            }
                            else
                            {
                                if (drone.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.PickedUp)).Seconds)
                                {
                                    myBL.DroneDeliver(drone.Id);
                                    drone = myBL.GetDrone(drone.Id);
                                }

                                else
                                {
                                    drone.Battery -= 0.5 * myBL.ShippingConsumption[(int)drone.InShipping.Weight];

                                    ////find the distance of the drone according to the timespan that has passed
                                    //double lat = (drone.InShipping.DeliveryLocation.Latitude - drone.CurrentLocation.Latitude)
                                    //    * (0.5 / BL.getDistance(drone.CurrentLocation, drone.InShipping.DeliveryLocation));
                                    //double lng = (drone.InShipping.DeliveryLocation.Longitude - drone.CurrentLocation.Longitude)
                                    //    * (0.5 / BL.getDistance(drone.CurrentLocation, drone.InShipping.DeliveryLocation));

                                    //drone.CurrentLocation.Latitude = lat;
                                    //drone.CurrentLocation.Longitude = lng;

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