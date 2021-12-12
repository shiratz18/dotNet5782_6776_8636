using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class Location
    {
        //bonus methods to display sexasegimal coordination and find distance between ocations

        /// <summary>
        /// finds sexasegiaml value of latitude
        /// </summary>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static string Lat(double lat)
        {
            string ch;
            if (lat < 0)
            {
                ch = "S";
                lat *= -1;
            }
            else
                ch = "N";
            int deg = (int)lat;
            double dif = lat - deg;
            int min = (int)(dif * 60);
            double sec = dif * 3600 - min * 60;
            sec = Math.Round(sec, 4);
            return $"{deg}° {min}' {string.Format("{0:0.0000}", sec)}'' {ch}";
        }
        /// <summary>
        /// finds sexasegimal value of longitude
        /// </summary>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static string Lng(double lng)
        {
            string ch;
            if (lng < 0)
            {
                ch = "W";
                lng *= -1;
            }
            else
                ch = "E";
            int deg = (int)lng;
            double dif = lng - deg;
            int min = (int)(dif * 60);
            double sec = dif * 3600 - min * 60;
            sec = Math.Round(sec, 4);
            return $"{deg}° {min}' {string.Format("{0:0.0000}", sec)}'' {ch}";
        }

        /// <summary>
        /// calculates the distance between 2 locations 
        /// </summary>
        /// <param name="lat1">the latitude of the first location</param>
        /// <param name="lng1">the longitude of the first location</param>
        /// <param name="lat2">the latitude of the second location</param>
        /// <param name="lng2">the longitude of the second location</param>
        /// <returns>the distance in km</returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            var d1 = lat1 * (Math.PI / 180.0);
            var num1 = lng1 * (Math.PI / 180.0);
            var d2 = lat2 * (Math.PI / 180.0);
            var num2 = lng2 * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
        }
    }

}

