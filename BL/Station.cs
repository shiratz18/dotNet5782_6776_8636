using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargeSlots { get; set; }
        public List<ChargingDrone> ChargingDrones { get; set; }

        public override string ToString()
        {
            string str = $"Station {Id}:";
            str += $"\nName - {Name}";
            str += $"\nLocation - {Location}";
            str += $"\nAvaialble charge slots - {AvailableChargeSlots}";
            str += $"\nCharging drones - ";
            ChargingDrones.ForEach(d =>
            {
                str += $"\n{d}";
            });
            return str;
        }
    }
}
