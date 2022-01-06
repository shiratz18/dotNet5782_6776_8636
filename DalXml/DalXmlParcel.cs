using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;
namespace Dal
{
    partial class DalXml
    {
        #region Add parcel
        /// <summary>
        /// adds a parcel to the list of parcels
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel parcel)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (customerList.Exists(c => c.Id == parcel.SenderId || !c.Active))
                throw new NoIDException($"Customer {parcel.SenderId} doesn't exist.");

            if (customerList.Exists(c => c.Id == parcel.TargetId || !c.Active))
                throw new NoIDException($"Customer {parcel.TargetId} doesn't exist.");

            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            var config = XmlTools.LoadListFromXMLSerializer<string>(configPath);
            int.TryParse(config[5], out int id);
            parcel.Id = id++;
            config[5] = id.ToString();
            XmlTools.SaveListToXMLSerializer(config, configPath);

            parcelList.Add(parcel);
            XmlTools.SaveListToXMLSerializer(parcelList, parcelsPath);

            return id;
        }
        #endregion

        #region Get parcel
        /// <summary>
        /// returns the object Parcel that matches the id
        /// </summary>
        /// <param name="id">parcel id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Exists(p => p.Id == id))
                throw new NoIDException($"Parcel {id} doesn't exist.");

            return parcelList.Find(x => x.Id == id);
        }
        #endregion

        #region Get parcel list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Parcel></Parcel> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate = null)
        {
            if (predicate != null)
                return XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath).Where(x => predicate(x));
            else
                return XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        }
        #endregion

        #region Update parcel
        /// <summary>
        /// updates a parcel
        /// </summary>
        /// <param name="parcel">the updated parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Exists(p => p.Id == parcel.Id))
                throw new NoIDException($"Parcel {parcel.Id} doesn't exist.");

            parcelList[parcelList.FindIndex(p => p.Id == parcel.Id)] = parcel;

            XmlTools.SaveListToXMLSerializer(parcelList, parcelsPath);
        }
        #endregion

        #region Assign drone to parcel
        /// <summary>
        /// assign a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">the id of the parcel</param>
        /// <param name="droneId">the id of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignDroneToParcel(int parcelId, int droneId)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            var dronelList = XmlTools.LoadListFromXMLSerializer<Drone>(dronesPath);

            if (!parcelList.Exists(p => p.Id == parcelId))
                throw new NoIDException($"Parcel {parcelId} doesn't exist.");

            if (!dronelList.Exists(d => d.Id == droneId || !d.Active))
                throw new NoIDException($"Drone {droneId} doesn't exists.");

            Parcel temp = parcelList[parcelList.FindIndex(p => p.Id == parcelId)];
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelPickUp(int id)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Exists(p => p.Id == id))
                throw new NoIDException($"Parcel {id} doesn't exist.");

            Parcel temp = parcelList[parcelList.FindIndex(p => p.Id == id)];
            temp.PickedUp = DateTime.Now;
            UpdateParcel(temp);
        }
        #endregion

        #region Deliver parcel
        /// <summary>
        /// updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelDelivered(int id)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Exists(p => p.Id == id))
                throw new NoIDException($"Parcel {id} doesn't exist.");

            Parcel temp = parcelList[parcelList.FindIndex(p => p.Id == id)];
            temp.Delivered = DateTime.Now;
            UpdateParcel(temp);
        }
        #endregion

        #region Remove parcel
        /// <summary>
        /// removes a parcel from the list
        /// </summary>
        /// <param name="parcel">the parcel to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(Parcel parcel)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Remove(parcel))
                throw new NoIDException($"Parcel {parcel.Id} doesn't exist.");

            XmlTools.SaveListToXMLSerializer(parcelList, parcelsPath);
        }
        #endregion
    }
}
