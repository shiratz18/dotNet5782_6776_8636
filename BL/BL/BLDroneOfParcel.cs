using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;
namespace BL
{
    partial class BL
    {
        #region Get drone of a parcel
        /// <summary>
        /// Get the drone of a parcel. according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object DroneOfParcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private DroneOfParcel getDroneOfParcel(int id)
        {
            ListDrone temp = Drones.FirstOrDefault(drone => drone.Id == id); //getting the drone from the list of drones, will throw exception if ID deosnt exist

            if (temp == null)
                throw new NoIDException($"Drone {id} does not exist.");

            DroneOfParcel d = new DroneOfParcel()
            {
                Id = temp.Id,
                Battery = temp.Battery,
                CurrentLocation = temp.CurrentLocation,
                Active = temp.Active
            };

            return d;

        }
        #endregion
    }
}
