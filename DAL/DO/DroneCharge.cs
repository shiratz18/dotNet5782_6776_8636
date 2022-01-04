using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }
        public DateTime ChargingBegin { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Drone ID: {DroneId},\n";
            result += $"Station ID: {StationId}\n";
            return result;
        }
    }
}

