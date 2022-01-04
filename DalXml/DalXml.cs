using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Reflection;
using System.IO;

namespace Dal
{
    partial class DalXml : IDal
    {
        #region Singleton and lazy
        private static DalXml instance = null;
        private static readonly object padlock = new object();

        public static DalXml Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DalXml();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Paths
        readonly string stationsPath = @"StationsXml.xml";
        readonly string customersPath = @"CustomersXml.xml";
        readonly string dronesPath = @"DronesXml.xml";
        readonly string droneChargePath = @"DroneChargeXml.xml";
        readonly string parcelsPath = @"ParcelsXml.xml";
        readonly string userPath = @"UserXml.xml";
        readonly string configPath;
        public static string localPath;
        #endregion

        #region Constructor
        DalXml()
        {
            string str = Assembly.GetExecutingAssembly().Location;
            localPath = Path.GetDirectoryName(str);
            localPath = Path.GetDirectoryName(localPath);

            localPath += @"\Data";

            dronesPath = localPath + @"\DroneXml.xml";
            droneChargePath = localPath + @"\DroneChargeXml.xml";
            stationsPath = localPath + @"\StationXml.xml";
            customersPath = localPath + @"\CustomerXml.xml";
            parcelsPath = localPath + @"\ParcelXml.xml";
            userPath = localPath + @"\UserXml.xml";
            configPath = localPath + @"\configXml.xml";
        }
        #endregion
    }


}
