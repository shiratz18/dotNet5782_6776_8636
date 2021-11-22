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
        /// Returns a customer in parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>The CustomerInParcel object</returns>
        public CustomerInParcel GetCustomerInParcel(int id)
        {
            try
            {
                IDAL.DO.Customer temp = data.GetCustomer(id); //getteing the customer from the data layer
                
                CustomerInParcel c = new CustomerInParcel() 
                {
                    Id = id,
                    Name = temp.Name
                };
                return c;
            }
            catch(IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
    }
}
