using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status{ get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                string result = " ";
                result += $"Drone {Id}:\n";
                result += $"Model - {Model},\n";
                result += $"Maximum weight: - {MaxWeight},\n";
                result += $"Status - {Status},\n";
                result += $"Battery - {Battery}\n";
                return result;
            }
        }
    }
}
