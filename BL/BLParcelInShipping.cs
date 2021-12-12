using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// Returns a ParcelInShipping object
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The parcel</returns>
        private ParcelInShipping getParcelInShipping(int id)
        {
            DO.Parcel temp = Data.GetParcel(id);

            ParcelInShipping parcel = new ParcelInShipping()
            {
                Id = temp.Id,
                Priority = (Priorities)temp.Priority,
                Weight = (WeightCategories)temp.Weight,
                Sender = new CustomerInParcel() { Id=temp.SenderId,Name=Data.GetCustomer(temp.SenderId).Name},
                Target = new CustomerInParcel() { Id = temp.TargetId, Name = Data.GetCustomer(temp.TargetId).Name }
            };

            if (temp.PickedUp == null)
                parcel.IsPickedUp = false;
            else
                parcel.IsPickedUp = true;

            parcel.PickUpLocation = new Location() { Longitude=Data.GetCustomer(temp.SenderId).Longitude, Latitude = Data.GetCustomer(temp.SenderId).Latitude };
            parcel.DeliveryLocation = new Location() { Longitude = Data.GetCustomer(temp.TargetId).Longitude, Latitude = Data.GetCustomer(temp.TargetId).Latitude };

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
            IEnumerable<Parcel> parcels;
            try
            {
                parcels = getListOfNoDroneParcels();
            }
            catch(EmptyListException ex)
            {
                throw new EmptyListException(ex.Message);
            }

            foreach (Parcel p in parcels)
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
