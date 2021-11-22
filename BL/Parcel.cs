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
            str += $"Parcel {Id}:\n";
            str += $"Sender - {Sender},\n";
            str += $"Target - {Target},\n";
            str += $"Weight - {Weight},\n";
            str += $"Rank - {Priority},\n";
            str += $"Requested time - {Requested},\n";
            str += $"Assigned drone ID - {DroneId},\n";
            str += $"Scheduled - {Scheduled},\n";
            str += $"Picked up - {PickedUp},\n";
            str += $"Delivered - {Delivered}\n";
            return str;
        }
    }
}
