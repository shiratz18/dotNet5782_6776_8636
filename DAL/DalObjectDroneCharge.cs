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
        /// send a drone to charge
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

        /// <summary>
        /// returns a list of the drones charging
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargeList()
        {
            return DataSource.DroneChargers;
        }

    }
}
