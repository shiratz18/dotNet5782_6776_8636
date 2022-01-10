using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using System.Runtime.CompilerServices;
namespace Dal
{
    partial class DalXml
    {
        #region Add Drone charge
        /// <summary>
        /// Add a drone to charge
        /// </summary>
        /// <param name="d">drone charge object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge d)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            if (!droneList.Exists(dr => dr.Id == d.DroneId))
                throw new NoIDException($"Drone {d.DroneId} doesn't exist.");

            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);
            if (!stationList.Exists(s => s.Id == d.StationId))
                throw new NoIDException($"Station {d.StationId} doesn't exist.");

            XElement droneChargeRoot = XmlTools.LoadListFromXMLElement(droneChargePath);

            droneChargeRoot.Add(new XElement("DroneCharge",
                    new XElement("DroneId", d.DroneId),
                    new XElement("StationId", d.StationId),
                    new XElement("ChargingBegin", d.ChargingBegin)));

            XmlTools.SaveListToXMLElement(droneChargeRoot, droneChargePath);
        }
        #endregion

        #region Remove Drone charge
        /// <summary>
        /// release a drone from charge
        /// </summary>
        /// <param name="d">drone charge object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(DroneCharge d)
        {
            XElement droneChargeRoot = XmlTools.LoadListFromXMLElement(droneChargePath);

            XElement x = (from dc in droneChargeRoot.Elements()
                          where int.Parse(dc.Element("DroneId").Value) == d.DroneId
                          select dc
                              ).FirstOrDefault();

            if (x != null)
            {
                x.Remove();
                XmlTools.SaveListToXMLElement(droneChargeRoot, droneChargePath);
            }
            else
            {
                throw new NoIDException($"Drone {d.DroneId} does not exist or is not currently charging");
            }
        }
        #endregion

        #region Get DroneCharge
        /// <summary>
        /// Returns the object Dronecharge that matches the id
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <returns>The DroneCharge</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int id)
        {
            XElement droneChargeRoot = XmlTools.LoadListFromXMLElement(droneChargePath);

            DroneCharge? x = (from dc in droneChargeRoot.Elements()
                              where int.Parse(dc.Element("DroneId").Value) == id
                              select new DroneCharge()
                              {
                                  DroneId = int.Parse(dc.Element("DroneId").Value),
                                  StationId = int.Parse(dc.Element("StationId").Value),
                                  ChargingBegin = DateTime.Parse(dc.Element("ChargingBegin").Value),
                              }).FirstOrDefault();

            if (x != null)
                return (DroneCharge)x;
            else
                throw new NoIDException($"Drone {id} doesn't exist or is not currently charging.");
        }
        #endregion

        #region Get DroneCharge list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<DroneCharge> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)
        {
            try
            {
                XElement droneChargeRoot = XmlTools.LoadListFromXMLElement(droneChargePath);

                if (predicate != null)
                {
                    var temp = from dc in droneChargeRoot.Elements()
                               where int.Parse(dc.Element("DroneId").Value) != 0
                               select new DroneCharge()
                               {
                                   DroneId = int.Parse(dc.Element("DroneId").Value),
                                   StationId = int.Parse(dc.Element("StationId").Value)
                               };

                    return from t in temp
                           where predicate(t)
                           select t;
                }

                return from dc in droneChargeRoot.Elements()
                       where int.Parse(dc.Element("DroneId").Value) != 0
                       select new DroneCharge()
                       {
                           DroneId = int.Parse(dc.Element("DroneId").Value),
                           StationId = int.Parse(dc.Element("StationId").Value)
                       };
            }
            catch { return null; }
        }
        #endregion
    }
}
