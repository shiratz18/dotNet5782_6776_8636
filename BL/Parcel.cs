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
            str += $"\nParcel {Id}:";
            str += $"\n Sender - {Sender}";
            str += $"\n Target - {Target}";
            str += $"\n Weight - {Weight}";
            str += $"\n Priority - {Priority}";
            str += $"\n Requested time - {Requested}";
            str += $"\n Assigned drone - {AssignedDrone}";
            str += $"\n Scheduled - {Scheduled}";
            str += $"\n Picked up - {PickedUp}";
            str += $"\n Delivered - {Delivered}";
            return str;
        }
    }
}
