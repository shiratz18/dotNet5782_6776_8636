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
        /// Add a drone to charge
        /// </summary>
        /// <param name="d">drone charge object</param>
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

        /// <summary>
        /// release a drone from charge
        /// </summary>
        /// <param name="d">drone charge object</param>
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

        ///// <summary>
        ///// returns a list of the drones charging
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<DroneCharge> GetDroneChargeList()
        //{
        //    return DataSource.DroneChargers;
        //}

        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<DroneCharge> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null)
        {
            if (predicate != null)
                return DataSource.DroneChargers.FindAll(predicate);
            else
                return DataSource.DroneChargers;
        }


    }
}
