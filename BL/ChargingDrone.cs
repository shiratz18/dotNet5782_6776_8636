using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ChargingDrone
    {
        public int Id { get; set; }
        public double Battery { get; set; }

        public override string ToString()
        {
            return $"Drone: {Id} Battery - {Battery}";
        }
    }
}
