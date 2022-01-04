using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using DalApi;
using DO;

namespace Dal
{
    partial class DalXml : IDal
    {
        public void AddDrone(Drone drone)
        {
            XElement droneRoot = XmlTools.LoadListFromXMLElement(dronesPath);
            droneRoot.Add(createDrone(drone));
            XmlTools.SaveListToXMLElement(droneRoot, dronesPath);
        }
        XElement createDrone(Drone drone)
        {
            return new XElement("drone",
                    new XElement("id", drone.Id),
                    new XElement("model", drone.Model),
                    new XElement("maxWeight", drone.MaxWeight));
        }
    }
}
