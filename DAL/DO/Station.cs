﻿
namespace DO
{
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ChargeSlots { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Station {Id}:\n";
            result += $"Name - {Name},\n";
            result += $"Location - {Location.Lat(Latitude)} {Location.Lng(Longitude)}\n";
            result += $"Available charge slots - {ChargeSlots}\n";
            return result;
        }
    }
}

