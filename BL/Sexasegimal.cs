using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Sexasegimal
    {
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
            double sec = (dif) * 3600 - min * 60;
            sec = Math.Round(sec, 4);
            return $"{deg}° {min}' {string.Format("{0:0.0000}", sec)}'' {ch}";
        }
    }
}
