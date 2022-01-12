using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace Dal
{
    partial class DalXml : IDal
    {
        #region Singleton and lazy
        // =null that if we dont need to create a "new bl" it will not create it
        private static DalXml instance = null;
        // for safty. So that if requests come from two places at the same time, it will not create it twice 
        private static readonly object padlock = new object();

        public static DalXml Instance
        {
            get
            {
                //if "instance" hasn`t yet been created, a new one will be created 
                if (instance == null)
                {
                    //stops a request from two places at the same time
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
        readonly string configPath = @"ConfigXml.xml";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private DalXml()
        {
           // DataSource.Initialize(); //intialize files once 
        }
        #endregion
    }
}
