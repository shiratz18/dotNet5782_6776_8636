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
            string str = $"Drone {Id}:";
            str += $"\nModel - {Model}";
            str += $"\nMaximum weight - {MaxWeight}";
            str += $"\nBattery - {Battery}";
            str += $"\nStatus - {Status}";
            str += $"\nParcel in shipping - {InShipping.Id}";
            str += $"\nCurrent location - {CurrentLocation}";
            return base.ToString();
        }
    }
}
