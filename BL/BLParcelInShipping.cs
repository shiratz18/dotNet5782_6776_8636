using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL : IBL
    {
        /// <summary>
        /// Returns a ParcelInShipping object
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The parcel</returns>
        private ParcelInShipping getParcelInShipping(int id)
        {
            IDAL.DO.Parcel temp = data.GetParcel(id);

            ParcelInShipping parcel = new ParcelInShipping()
            {
                Id = temp.Id,
                Priority = (Priorities)temp.Priority,
                Weight = (WeightCategories)temp.Weight,
                Sender = new CustomerInParcel() { Id=temp.SenderId,Name=data.GetCustomer(temp.SenderId).Name},
                Target = new CustomerInParcel() { Id = temp.TargetId, Name = data.GetCustomer(temp.TargetId).Name }
            };

            if (temp.PickedUp == DateTime.MinValue)
                parcel.IsPickedUp = false;
            else
                parcel.IsPickedUp = true;

            parcel.PickUpLocation = new Location() { Longitude=data.GetCustomer(temp.SenderId).Longitude, Latitude = data.GetCustomer(temp.SenderId).Latitude };
            parcel.DeliveryLocation = new Location() { Longitude = data.GetCustomer(temp.TargetId).Longitude, Latitude = data.GetCustomer(temp.TargetId).Latitude };

            parcel.DeliveryDistance = getDistance(parcel.PickUpLocation, parcel.DeliveryLocation);

            return parcel;
        }

        /// <summary>
        /// Returns the parcels with no drones in ParcelInShipping object
        /// </summary>
        /// <returns>The list of parcels with no drones</returns>
        private IEnumerable<ParcelInShipping> getListOfNoDroneParcelsInShipping()
        {
            List<ParcelInShipping> pis = new List<ParcelInShipping>();

            foreach (Parcel p in getListOfNoDroneParcels())
            {
                pis.Add(new ParcelInShipping
                {
                    Id = p.Id,
                    IsPickedUp = false,
                    Priority = p.Priority,
                    Weight = p.Weight,
                    Sender = p.Sender,
                    Target = p.Target,
                    PickUpLocation = GetCustomer(p.Sender.Id).Location,
                    DeliveryLocation = GetCustomer(p.Target.Id).Location
                });
            }

            foreach (ParcelInShipping p in pis)
            {
                p.DeliveryDistance = getDistance(p.PickUpLocation, p.DeliveryLocation);
            }

            return pis;
        }
    }
}
