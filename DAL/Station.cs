using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
            public override string ToString()
            {
                string result = " ";
                result += $"ID: {Id},\n";
                result += $"Name: {Name},\n";
                result += $"Longitude: {Longitude},\n";
                result += $"Latitude: {Latitude},\n";
                result += $"Charge slots: {ChargeSlots}\n";
                return result;
            }
        }
    }
}
