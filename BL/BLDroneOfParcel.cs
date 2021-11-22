using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// Get the drone of a parcel. according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object DroneOfParcel</returns>
        public DroneOfParcel GetDroneOfParcel(int id)
        {
            try
            {
                Drone temp = GetDrone(id); //getting the drone from the list of drones, will throw exception if ID deosnt exist

                DroneOfParcel d = new DroneOfParcel()
                {
                    Id = temp.Id,
                    Battery = temp.Battery,
                    CurrentLocation = temp.CurrentLocation
                };

                return d;
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}
