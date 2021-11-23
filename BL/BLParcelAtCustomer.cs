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
        /// Returns a ParcelAtCustomer object according to parcel ID and customer ID
        /// </summary>
        /// <param name="parcelId">The ID of the parcel</param>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>The ParcelAtCustomer object</returns>
        internal ParcelAtCustomer getParcelAtCustomer(int parcelId, int customerId)
        {
            Parcel temp = GetParcel(parcelId);

            ParcelAtCustomer p = new ParcelAtCustomer()
            {
                Id = temp.Id,
                Weight = temp.Weight,
                Priority = temp.Priority,
            };

            //if it has been delivered, the state is delivered
            if (temp.Delivered != DateTime.MinValue)
                p.State = ParcelState.Delivered;
            //otherwise if it has been picked up the state is picked up
            else if (temp.PickedUp != DateTime.MinValue)
                p.State = ParcelState.PickedUp;
            //otherwise if it was assigned a drone the state is scheduled
            else if (temp.Scheduled != DateTime.MinValue)
                p.State = ParcelState.Scheduled;
            //otherwise the state is requested
            else
                p.State = ParcelState.Requested;

            //if the customer is the sender, so the other side is the target
            if (temp.Sender.Id == customerId)
                p.OtherSide = temp.Target;
            //otherwise, the other side is the sender
            else
                p.OtherSide = temp.Sender;

            return p;
        }
    }
}
