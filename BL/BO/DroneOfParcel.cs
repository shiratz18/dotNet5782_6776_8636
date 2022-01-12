﻿
namespace BO
{
    public class DroneOfParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            string str = $"Drone {Id}: Battery - {Battery} Current location - {CurrentLocation}";
            return str;
        }
    }
}