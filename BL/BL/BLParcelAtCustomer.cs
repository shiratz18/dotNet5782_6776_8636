using BO;

namespace BL
{
    partial class BL
    {
        #region Get parcal at customer
        /// <summary>
        /// Returns a ParcelAtCustomer object according to parcel ID and customer ID
        /// </summary>
        /// <param name="parcelId">The ID of the parcel</param>
        /// <param name="customerId">The ID of the customer</param>
        /// <returns>The ParcelAtCustomer object</returns>
        internal ParcelAtCustomer getParcelAtCustomer(int parcelId, int customerId)
        {
            DO.Parcel temp = Data.GetParcel(parcelId);

            ParcelAtCustomer p = new ParcelAtCustomer()
            {
                Id = temp.Id,
                Weight = (WeightCategories)temp.Weight,
                Priority = (Priorities)temp.Priority,
            };

            //if it has been delivered, the state is delivered
            if (temp.Delivered != null)
                p.State = ParcelState.Delivered;
            //otherwise if it has been picked up the state is picked up
            else if (temp.PickedUp != null)
                p.State = ParcelState.PickedUp;
            //otherwise if it was assigned a drone the state is scheduled
            else if (temp.Scheduled != null)
                p.State = ParcelState.Scheduled;
            //otherwise the state is requested
            else
                p.State = ParcelState.Requested;

            //if the customer is the sender, so the other side is the target
            if (temp.SenderId == customerId)
                p.OtherSide = getCustomerInParcel(temp.TargetId);
            //otherwise, the other side is the sender
            else
                p.OtherSide = getCustomerInParcel(temp.SenderId);

            return p;
        }
        #endregion
    }
}
