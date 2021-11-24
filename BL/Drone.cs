using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public ParcelInShipping InShipping { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            string str = $"\nDrone {Id}:";
            str += $"\n Model - {Model}";
            str += $"\n Maximum weight - {MaxWeight}";
            str += $"\n Battery - {Battery}%";
            str += $"\n Status - {Status}";
            if (InShipping.Id != 0)
                str += $"\n Parcel in shipping - {InShipping.Id}";
            str += $"\n Current location - {CurrentLocation}";
            return str;
        }
    }
}
