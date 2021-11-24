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
            Parcel temp = GetParcel(id);

            ParcelInShipping parcel = new ParcelInShipping()
            {
                Id = temp.Id,
                Priority = temp.Priority,
                Weight = temp.Weight,
                Sender = temp.Sender,
                Target = temp.Target
            };

            if (temp.PickedUp == DateTime.MinValue)
                parcel.IsPickedUp = false;
            else
                parcel.IsPickedUp = true;

            parcel.PickUpLocation = GetCustomer(temp.Sender.Id).Location;
            parcel.DeliveryLocation = GetCustomer(temp.Target.Id).Location;

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
                p.DeliveryDistance = getDistance(p.PickUpLocation, p.DeliveryLocation)
            }

            return pis;
        }
    }
}
