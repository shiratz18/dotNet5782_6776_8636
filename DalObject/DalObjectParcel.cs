using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalObject
    {
        #region Add parcel
        /// <summary>
        /// adds a parcel to the list of parcels
        /// </summary>
        public int AddParcel(Parcel parcel)
        {
            if (!DataSource.Customers.Exists(c => c.Id == parcel.SenderId || !c.Active))
            {
                throw new NoIDException($"Customer {parcel.SenderId} doesn't exist.");
            }

            if (!DataSource.Customers.Exists(c => c.Id == parcel.TargetId || !c.Active))
            {
                throw new NoIDException($"Customer {parcel.TargetId} doesn't exist.");
            }

            parcel.Id = DataSource.Config.IdNumber;
            DataSource.Parcels.Add(parcel);
            DataSource.Config.IdNumber++;
            return DataSource.Config.IdNumber;
        }
        #endregion

        #region Update parcel
        /// <summary>
        /// updates a parcel
        /// </summary>
        /// <param name="parcel">the updated parcel</param>
        public void UpdateParcel(Parcel parcel)
        {
            if (!DataSource.Parcels.Exists(p => p.Id == parcel.Id))
            {
                throw new NoIDException($"Parcel {parcel.Id} doesn't exist.");
            }

            DataSource.Parcels[DataSource.Parcels.FindIndex(p => p.Id == parcel.Id)] = parcel;
        }
        #endregion

        #region Remove parcel
        /// <summary>
        /// removes a parcel from the list
        /// </summary>
        /// <param name="parcel">the parcel to remove</param>
        public void RemoveParcel(Parcel parcel)
        {
            if (!DataSource.Parcels.Remove(parcel))
            {
                throw new NoIDException($"Parcel {parcel.Id} doesn't exist.");
            }
        }
        #endregion

        #region Assign drone to parcel
        /// <summary>
        /// assign a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">the id of the parcel</param>
        /// <param name="droneId">the id of the drone</param>
        public void AssignDroneToParcel(int parcelId, int droneId)
        {
            if (!DataSource.Parcels.Exists(p => p.Id == parcelId))
            {
                throw new NoIDException($"Parcel {parcelId} doesn't exist.");
            }

            if (!DataSource.Drones.Exists(d => d.Id == droneId || !d.Active))
            {
                throw new NoIDException($"Drone {droneId} doesn't exists.");
            }

            Parcel temp = DataSource.Parcels[DataSource.Parcels.FindIndex(p => p.Id == parcelId)];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            UpdateParcel(temp);
        }
        #endregion

        #region Pick up parcel
        /// <summary>
        /// updates the pick up time of the parcel
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public void ParcelPickUp(int id)
        {
            if (!DataSource.Parcels.Exists(p => p.Id == id))
            {
                throw new NoIDException($"Parcel {id} doesn't exist.");
            }

            Parcel temp = DataSource.Parcels[DataSource.Parcels.FindIndex(p => p.Id == id)];
            temp.PickedUp = DateTime.Now;
            UpdateParcel(temp);
        }
        #endregion

        #region Deliver parcel
        /// <summary>
        /// updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public void ParcelDelivered(int id)
        {
            if (!DataSource.Parcels.Exists(p => p.Id == id))
            {
                throw new NoIDException($"Parcel {id} doesn't exist.");
            }

            Parcel temp = DataSource.Parcels[DataSource.Parcels.FindIndex(p => p.Id == id)];
            temp.Delivered = DateTime.Now;
            UpdateParcel(temp);
        }
        #endregion

        #region Get parcel
        /// <summary>
        /// returns the object Parcel that matches the id
        /// </summary>
        /// <param name="id">parcel id</param>
        /// <returns></returns>
        public Parcel GetParcel(int id)
        {
            if (!DataSource.Parcels.Exists(p => p.Id == id))
            {
                throw new NoIDException($"Parcel {id} doesn't exist.");
            }

            return DataSource.Parcels.Find(x => x.Id == id);
        }
        #endregion

        #region Get parcel list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Parcel></Parcel> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate = null)
        {
            if (predicate != null)
                return DataSource.Parcels.FindAll(predicate);
            else
                return DataSource.Parcels;
        }
        #endregion
    }
}
