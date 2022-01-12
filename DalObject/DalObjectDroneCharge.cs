using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        #region Add Drone charge
        /// <summary>
        /// Add a drone to charge
        /// </summary>
        /// <param name="d">drone charge object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge d)
        {
            if (!DataSource.Drones.Exists(drone => d.DroneId == drone.Id))
            {
                throw new NoIDException($"Drone {d.DroneId} doesn't exists.");
            }

            if (!DataSource.Stations.Exists(station => d.StationId == station.Id))
            {
                throw new NoIDException($"Station {d.StationId} doesn't exists.");
            }

            DataSource.DroneChargers.Add(d);
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
            if (!DataSource.Drones.Exists(drone => d.DroneId == drone.Id))
            {
                throw new NoIDException($"Drone {d.DroneId} doesn't exists.");
            }

            if (!DataSource.Stations.Exists(station => d.StationId == station.Id))
            {
                throw new NoIDException($"Station {d.StationId} doesn't exists.");
            }

            DataSource.DroneChargers.Remove(d);
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
            if (!DataSource.DroneChargers.Exists(dc => dc.DroneId == id))
                throw new NoIDException($"Drone {id} doesn't exist or is not currently charging.");

            return DataSource.DroneChargers.Find(dc => dc.DroneId == id);
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
            if (predicate != null)
                return DataSource.DroneChargers.Where(d => predicate(d)).Select(item => item);
            else
                return DataSource.DroneChargers.Select(item => item);
        }
        #endregion
    }
}
