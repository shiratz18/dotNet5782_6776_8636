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
            if (DataSource.Drones.Exists(d => d.Id == drone.Id))
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
        /// Updates the ID of a drone
        /// </summary>
        /// <param name="currentId">The current drone ID</param>
        /// <param name="newId">The new ID</param>
        public void EditDroneId(int currentId, int newId)
        {
            if (!DataSource.Drones.Exists(d => d.Id == currentId))
            {
                throw new NoIDException($"Drone {currentId} doesn't exists.");
            }

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == currentId)];
            drone.Id = newId;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == currentId)] = drone;
        }

        /// <summary>
        /// Updates the drone name
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="name">The new name</param>
        public void EditDroneModel(int id, string model)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
            {
                throw new NoIDException($"Drone {id} doesn't exists.");
            }

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)];
            drone.Model = model;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)] = drone;
        }

        /// <summary>
        /// Updates the maximum weight of the drone
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="weight">The new maximum weight</param>
        public void EditDroneMaxWeight(int id,WeightCategories weight)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
            {
                throw new NoIDException($"Drone {id} doesn't exists.");
            }

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)];
            drone.MaxWeight = weight;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)] = drone;
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
