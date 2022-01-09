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
        double speed = 0.75;

        Drone d;

        public Simulator(int id, Action updateDelegate, Func<bool> stopDelegate, BL myBL)
        {
            lock (myBL)
            {
                d = myBL.GetDrone(id);
            }
            while (stopDelegate())
            {
                switch (d.Status)
                {
                    case DroneStatuses.Available:
                        lock (myBL)
                        {
                            try
                            {
                                myBL.AssignDroneToParcel(d.Id);
                            }
                            catch (NoIDException)
                            {

                            }
                            catch (DroneStateException)
                            {
                                try
                                {
                                    myBL.ChargeDrone(d.Id);
                                }
                                catch (EmptyListException)
                                {

                                }
                            }
                            catch (EmptyListException)
                            {

                            }
                        }
                        break;
                    case DroneStatuses.Maintenance:
                        lock (myBL)
                        {
                            if (d.Battery == 100)
                                myBL.ReleaseDroneCharge(d.Id);
                            else
                                d.Battery += timer * myBL.DroneChargingRate;
                        }
                                break;
                  
                    case DroneStatuses.Shipping:
                        lock (myBL)
                        {
                            Parcel p = myBL.GetParcel(d.InShipping.Id);

                            if (!d.InShipping.IsPickedUp)
                            {
                                if (d.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.Scheduled)).Seconds)
                                    myBL.DronePickUp(d.Id);
                                d.Battery -= speed * timer * myBL.AvailableConsumption;

                            }
                            else
                            {
                                if (d.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.PickedUp)).Seconds)
                                    myBL.DroneDeliver(d.Id);
                                d.Battery -= speed * timer * myBL.ShippingConsumption[(int)d.InShipping.Weight];

                            }
                        }
                            //Thread.Sleep((int)(d.InShipping.DeliveryDistance / speed));
                            //myBL.DronePickUp(d.Id);
                            //Thread.Sleep((int)(d.InShipping.DeliveryDistance / speed));
                            //myBL.DroneDeliver(d.Id);
                            break;  
                }
                Thread.Sleep(timer);
            }
        }
    }
}