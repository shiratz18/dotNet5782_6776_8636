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
        public Simulator(int id, Action updateDelegate, Func<bool> stopDelegate, BL myBL)
        {
            Drone d = myBL.GetDrone(id);


            while (stopDelegate())
            {
                switch (d.Status)
                {
                    case DroneStatuses.Available:
                        try
                        {
                            myBL.AssignDroneToParcel(d.Id);
                        }
                        catch (NoIDException)
                        {

                        }
                        catch (DroneStateException)
                        {
                            myBL.ChargeDrone(d.Id);
                        }
                        catch (EmptyListException)
                        {

                        }
                        break;
                    case DroneStatuses.Maintenance:

                        if (d.Battery == 100)
                            myBL.ReleaseDroneCharge(d.Id);

                        break;
                    case DroneStatuses.Shipping:
 
                        Parcel p = myBL.GetParcel(d.InShipping.Id);
                        
                        if (!d.InShipping.IsPickedUp)
                        {
                            if (d.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.Scheduled)).Seconds)
                                    myBL.DronePickUp(d.Id);
                         
                        }
                        else
                        {
                            if (d.InShipping.DeliveryDistance / speed <= ((TimeSpan)(DateTime.Now - p.PickedUp)).Seconds)
                                myBL.DroneDeliver(d.Id);

                        }
                        d.Battery-=speed*timer*d.InShipping

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