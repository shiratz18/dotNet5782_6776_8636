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
        #region Get parcel
        /// <summary>
        /// Returns a ParcelInShipping object
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private ParcelInShipping getParcelInShipping(int id)
        {
            DO.Parcel temp = Data.GetParcel(id);

            ParcelInShipping parcel = new ParcelInShipping()
            {
                Id = temp.Id,
                Priority = (Priorities)temp.Priority,
                Weight = (WeightCategories)temp.Weight,
                Sender = new CustomerInParcel() { Id = temp.SenderId, Name = Data.GetCustomer(temp.SenderId).Name },
                Target = new CustomerInParcel() { Id = temp.TargetId, Name = Data.GetCustomer(temp.TargetId).Name }
            };

            if (temp.PickedUp == null)
                parcel.IsPickedUp = false;
            else
                parcel.IsPickedUp = true;

            parcel.PickUpLocation = new Location() { Longitude = Data.GetCustomer(temp.SenderId).Longitude, Latitude = Data.GetCustomer(temp.SenderId).Latitude };
            parcel.DeliveryLocation = new Location() { Longitude = Data.GetCustomer(temp.TargetId).Longitude, Latitude = Data.GetCustomer(temp.TargetId).Latitude };

            if (parcel.IsPickedUp)
                parcel.DeliveryDistance = getDistance(parcel.PickUpLocation, parcel.DeliveryLocation);
            else
                parcel.DeliveryDistance = 0;

            return parcel;
        }
        #endregion

        #region Get parcels without a drone
        /// <summary>
        /// Returns the parcels with no drones in ParcelInShipping object
        /// </summary>
        /// <returns>The list of parcels with no drones</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private IEnumerable<ParcelInShipping> getListOfNoDroneParcelsInShipping()
        {
            List<ParcelInShipping> pis = new List<ParcelInShipping>();
            IEnumerable<Parcel> parcels;
            try
            {
                parcels = getListOfNoDroneParcels();
            }
            catch (EmptyListException ex)
            {
                throw new EmptyListException(ex.Message);
            }

            return from p in parcels
                   select new ParcelInShipping
                   {
                       Id = p.Id,
                       IsPickedUp = false,
                       Priority = p.Priority,
                       Weight = p.Weight,
                       Sender = p.Sender,
                       Target = p.Target,
                       PickUpLocation = GetCustomer(p.Sender.Id).Location,
                       DeliveryLocation = GetCustomer(p.Target.Id).Location
                   };
        }
        #endregion
    }
}
