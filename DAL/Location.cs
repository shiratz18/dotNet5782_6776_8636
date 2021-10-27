using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
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
                double sec = (dif * 3600 - min * 60);
                sec = Math.Round(sec, 4);
                return $"{deg}° {min}' {sec}'' {ch}";
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
                double sec = (dif) * 3600 - min * 60;
                sec = Math.Round(sec, 4);
                return $"{deg}° {min}' {sec}'' {ch}";
            }

            /// <summary>
            /// calculates the distance between 2 locations 
            /// </summary>
            /// <param name="lat1">the latitude of the first location</param>
            /// <param name="lng1">the longitude of the first location</param>
            /// <param name="lat2">the latitude of the second location</param>
            /// <param name="lng2">the longitude of the second location</param>
            /// <returns>the distance in km</returns>
            public static double GetDistanceFromLatLngInKm(double lat1, double lng1, double lat2, double lng2)
            {
                int R = 6371; // Radius of the earth in km
                double dLat = degToRad(lat2 - lat1);  // degToRad below
                double dLon = degToRad(lng2 - lng1);
                double a =
                  Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                  Math.Cos(degToRad(lat1)) * Math.Cos(degToRad(lat2)) *
                  Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double d = R * c; // Distance in km
                return Math.Round(d, 2);
            }

            private static double degToRad(double deg)
            {
                return deg * (Math.PI / 180);
            }

        }

    }
}
