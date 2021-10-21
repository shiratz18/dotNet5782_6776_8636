using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    namespace DalObject
    {
        public class DataSource
        {
            internal static List<Drone> Drones = new List<Drone>(10);
            internal static List<Station> Stations = new List<Station>(5);
            internal static List<Customer> Customers = new List<Customer>(100);
            internal static List<Parcel> Parcels = new List<Parcel>(1000);
            internal class Config
            {
                
            }
            static Random r = new Random();
            static void Initialize()
            {

            }
        }
    }
}
