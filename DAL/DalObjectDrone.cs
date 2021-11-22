using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// adds a drone to the list of drones
        /// </summary>
        public void AddDrone(Drone drone)
        {
            if(DataSource.Drones.Exists(d=>d.Id==drone.Id))
            {
                throw new DoubleIDException($"Drone {drone.Id} already exists.");
            }

            DataSource.Drones.Add(drone);
        }

        /// <summary>
        /// updates a drone in the list
        /// </summary>
        /// <param name="drone">the updated drone</param>
        public void UpdateDrone(Drone drone)
        {
            if (!DataSource.Drones.Exists(d => d.Id == drone.Id))
            {
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");
            }

            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == drone.Id)] = drone;
        }

        /// <summary>
        /// removes a drone from the list
        /// </summary>
        /// <param name="drone">the drone to remove</param>
        public void RemoveDrone(Drone drone)
        {
            if (!DataSource.Drones.Exists(d => d.Id == drone.Id))
            {
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");
            }

            DataSource.Drones.Remove(drone);
        }

        /// <summary>
        /// returns the object Drone that matches the id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
            {
                throw new NoIDException($"Drone {id} doesn't exists.");
            }

            return DataSource.Drones.Find(x => x.Id == id);
        }

        /// <summary>
        /// returns the list of drones
        /// </summary>
        /// <returns>list Drones</returns>
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.Drones;
        }

        /// <summary>
        /// function returns an array with info about the electricity consumption of a drone
        /// </summary>
        /// <returns>the array</returns>
        public double[] GetDroneElectricityConsumption()
        {
            double[] temp = new double[5];
            temp[0] = DataSource.Config.AvailableConsumption;
            temp[1] = DataSource.Config.LightWeightConsumption;
            temp[2] = DataSource.Config.MediumWeightConsumption;
            temp[3] = DataSource.Config.HeavyWeightConsumption;
            temp[4] = DataSource.Config.ChargingRate;
            return temp;
        }
    }
}
