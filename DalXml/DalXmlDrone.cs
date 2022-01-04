using System;
using System.Collections.Generic;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalXml
    {
        #region Add drone
        /// <summary>
        /// Add drone to list in dronesXML
        /// </summary>
        /// <param name="station">The station to add</param>
        public void AddDrone(Drone drone)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (droneList.Exists(d => d.Id == drone.Id))
                throw new DoubleIDException($"Drone {drone.Id} already exists.");
            droneList.Add(drone);

            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);
        }
        #endregion

        #region Get drone
        /// <summary>
        /// Returns the object Drone that matches the id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (!droneList.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone d = droneList.Find(x => x.Id == id);

            if (!d.Active)
                throw new NoIDException($"Drone {id} doesn't exists.");
            return d;
        }
        #endregion

        #region Get drone list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition. (or the entire list if no predicate was given)
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The list</returns>
        public IEnumerable<Drone> GetDroneList(Predicate<Drone> predicate = null)
        {
            if (predicate != null)
                return XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath).Where(x => predicate(x));
            else
                return XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);
        }
        #endregion

        #region Remove drone
        /// <summary>
        /// Mark a drone as deleted
        /// </summary>
        /// <param name="drone">The drone to remove</param>
        public void RemoveDrone(Drone drone)
        {

            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (!droneList.Exists(d => d.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");

            drone.Active = false;
            droneList[droneList.FindIndex(s => s.Id == drone.Id)] = drone;
            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);
        }
        #endregion

        #region Get drone electricity consumption
        /// <summary>
        /// function returns an array with info about the electricity consumption of a drone
        /// </summary>
        /// <returns>the array</returns>
        public double[] GetDroneElectricityConsumption()
        {
            List<string> config = XmlTools.LoadListFromXMLSerializer<string>(@"DalConfig.xml");
            double[] temp = new double[5];
            temp[0] = double.Parse(config[0]);
            temp[1] = double.Parse(config[1]);
            temp[2] = double.Parse(config[2]);
            temp[3] = double.Parse(config[3]);
            temp[4] = double.Parse(config[4]);
            return temp;

        }
        #endregion

        #region Edit drone
        /// <summary>
        /// Updates the ID of a drone
        /// </summary>
        /// <param name="currentId">The current drone ID</param>
        /// <param name="newId">The new ID</param>
        public void EditDroneId(int currentId, int newId)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (!droneList.Exists(d => d.Id == currentId))
                throw new NoIDException($"Drone {currentId} doesn't exists.");

            Drone drone = droneList.Find(s => s.Id == currentId);
            drone.Id = newId;
            droneList[droneList.FindIndex(s => s.Id == currentId)] = drone;
            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);

        }
        /// <summary>
        /// Updates the drone name
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="name">The new name</param>
        public void EditDroneModel(int id, string model)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (!droneList.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone drone = droneList.Find(s => s.Id == id);
            drone.Model = model;
            droneList[droneList.FindIndex(s => s.Id == id)] = drone;
            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);
        }

        /// <summary>
        /// Updates the maximum weight of the drone
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="weight">The new maximum weight</param>
        public void EditDroneMaxWeight(int id, WeightCategories weight)
        {

            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file

            if (!droneList.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone drone = droneList.Find(s => s.Id == id);
            drone.MaxWeight = weight;
            droneList[droneList.FindIndex(s => s.Id == id)] = drone;
            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);

        }
        #endregion

        #region Update drone
        /// <summary>
        /// updates a drone in the list
        /// </summary>
        /// <param name="drone">the updated drone</param>
        public void UpdateDrone(Drone drone)
        {
            var droneList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath); //getting drone list from xml file


            if (!droneList.Exists(d => d.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");

            droneList[droneList.FindIndex(s => s.Id == drone.Id)] = drone;
            XmlTools.SaveListToXMLSerializer(droneList, dronesPath);

        }
        #endregion
    }
}