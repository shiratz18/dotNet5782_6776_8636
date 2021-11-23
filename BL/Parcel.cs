using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneOfParcel AssignedDrone { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }

        public override string ToString()
        {
            string str = "";
            str += $"Parcel {Id}:";
            str += $"\nSender - {Sender}";
            str += $"\nTarget - {Target}";
            str += $"\nWeight - {Weight}";
            str += $"\nPriority - {Priority}";
            str += $"\nRequested time - {Requested}";
            str += $"\nAssigned drone - {AssignedDrone}";
            str += $"\nScheduled - {Scheduled}";
            str += $"\nPicked up - {PickedUp}";
            str += $"\nDelivered - {Delivered}";
            return str;
        }
    }
}
