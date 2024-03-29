﻿using BO;

namespace BL
{
    partial class BL
    {
        #region Get customer in parcel
        /// <summary>
        /// Returns a customer in parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>The CustomerInParcel object</returns>
        private CustomerInParcel getCustomerInParcel(int id)
        {
            try
            {
                DO.Customer temp = Data.GetCustomer(id); //getteing the customer from the data layer

                CustomerInParcel c = new CustomerInParcel()
                {
                    Id = id,
                    Name = temp.Name
                };
                return c;
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
        #endregion
    }
}
