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
            //bonus methods to display sexsigsimal coordination
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
                int min = (int)(lat - deg) * 60;
                double sec = (lat - deg) * 3600;
                return $"{deg}° {min}' {sec}'' {ch}";
            }
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
                int min = (int)(lng - deg) * 60;
                double sec = (lng - deg) * 3600;
                return $"{deg}° {min}' {sec}'' {ch}";
            }
        }

    }
}
