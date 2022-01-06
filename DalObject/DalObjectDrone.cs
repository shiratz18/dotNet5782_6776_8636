using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;
namespace Dal
{
    partial class DalObject
    {
        #region Add drone
        /// <summary>
        /// adds a drone to the list of drones
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            if (DataSource.Drones.Exists(d => d.Id == drone.Id))
                throw new DoubleIDException($"Drone {drone.Id} already exists.");

            DataSource.Drones.Add(drone);
        }
        #endregion

        #region Update drone
        /// <summary>
        /// updates a drone in the list
        /// </summary>
        /// <param name="drone">the updated drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            if (!DataSource.Drones.Exists(d => d.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");

            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == drone.Id)] = drone;
        }
        #endregion

        #region Edit drone
        /// <summary>
        /// Updates the ID of a drone
        /// </summary>
        /// <param name="currentId">The current drone ID</param>
        /// <param name="newId">The new ID</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EditDroneId(int currentId, int newId)
        {
            if (!DataSource.Drones.Exists(d => d.Id == currentId))
                throw new NoIDException($"Drone {currentId} doesn't exists.");

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == currentId)];
            drone.Id = newId;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == currentId)] = drone;
        }

        /// <summary>
        /// Updates the drone name
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="name">The new name</param>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EditDroneModel(int id, string model)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)];
            drone.Model = model;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)] = drone;
        }

        /// <summary>
        /// Updates the maximum weight of the drone
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="weight">The new maximum weight</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EditDroneMaxWeight(int id, WeightCategories weight)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone drone = DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)];
            drone.MaxWeight = weight;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == id)] = drone;
        }
        #endregion

        #region Remove drone
        /// <summary>
        /// removes a drone from the list
        /// </summary>
        /// <param name="drone">the drone to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(Drone drone)
        {
            if (!DataSource.Drones.Exists(d => d.Id == drone.Id))
                throw new NoIDException($"Drone {drone.Id} doesn't exists.");

            drone.Active = false;
            DataSource.Drones[DataSource.Drones.FindIndex(s => s.Id == drone.Id)] = drone;
        }
        #endregion

        #region Get drone
        /// <summary>
        /// Returns the object Drone that matches the id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            if (!DataSource.Drones.Exists(d => d.Id == id))
                throw new NoIDException($"Drone {id} doesn't exists.");

            Drone d = DataSource.Drones.Find(x => x.Id == id);

            if(!d.Active)
                throw new NoIDException($"Drone {id} doesn't exists.");

            return d;
        }
        #endregion

        #region Get drone list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Drone></Drone> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDroneList(Predicate<Drone> predicate = null)
        {
            if (predicate != null)
                return DataSource.Drones.FindAll(predicate);
            else
                return DataSource.Drones;
        }
        #endregion

        #region Get drone electricity consumption
        /// <summary>
        /// function returns an array with info about the electricity consumption of a drone
        /// </summary>
        /// <returns>the array</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        #endregion
    }
}
