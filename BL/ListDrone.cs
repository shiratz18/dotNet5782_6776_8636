using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ListDrone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; }

        public override string ToString()
        {
            string str = $"Drone {Id}:";
            str += $"\nModel - {Model}";
            str += $"\n Maximum weight - {MaxWeight}";
            str += $"\nBattery - {Battery}";
            str += $"Status - {Status}";
            str += $"Current location - {CurrentLocation}";
            if (Status == DroneStatuses.Shipping)
                str += $"Carrying parcel - {ParcelId}";
            return str;
        }
    }
}
