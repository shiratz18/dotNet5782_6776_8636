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
        /// Add a customer to the data list of customers
        /// </summary>
        /// <param name="customer">the customer to add</param>
        public void AddCustomer(Customer customer)
        {
            IDAL.DO.Customer temp = new IDAL.DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
            };

            try
            {
                data.AddCustomer(temp);
            }
            catch (IDAL.DO.DoubleIDException ex)
            {
                throw new DoubleIDException(ex.Message);
            }
        }

        /// <summary>
        /// Returns the customer according to ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>Customer object</returns>
        public Customer GetCustomer(int id)
        {
            try
            {
                IDAL.DO.Customer temp = data.GetCustomer(id);

                Customer c = new Customer()
                {
                    Id = temp.Id,
                    Name = temp.Name,
                    Phone = temp.Phone,
                    Location = new Location { Longitude = temp.Longitude, Latitude = temp.Latitude }
                    // FromCustomer = GetFromCustomerParcels(temp.Id),
                    //ToCustomer = GetToCustomerParcels(temp)
                };


                CustomerInParcel cip = new CustomerInParcel() { Id = temp.Id, Name = temp.Name };
                List<Parcel> parcels = (List<Parcel>)(GetParcelList());
                parcels.ForEach(p =>
                {
                    if (p.Sender == cip)
                    {
                        c.FromCustomer.Add(GetParcelAtCustomer(p.Id);
                    }
                });
                for (int i = 0; i < parcels.Count(); i++)
                {
                    if (parcels[i].S)
                }
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Update the name of a cuatomer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="name">The name of the customer</param>
        public void UpdateCustomerName(int id, string name)
        {
            IDAL.DO.Customer c;
            try
            {
                c = data.GetCustomer(id); //trying to get the customer from dll, will throw an exception if customer ID doesnt exist
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            if (name != null)
            {
                c.Name = name;
                try
                {
                    data.UpdateCustomer(c); //send the updates customer to dll
                }
                catch (IDAL.DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
            else
                throw new WrongFormatException("Wrong string format.");
        }

        /// <summary>
        /// Update the phone number of a customer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="phone">The new phone number</param>
        public void UpdateCustomerPhone(int id, string phone)
        {
            IDAL.DO.Customer c;
            try
            {
                c = data.GetCustomer(id); //trying to get the customer from dll, will throw an exception if customer ID doesnt exist
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            if (phone != null)
            {
                c.Phone = phone;
                try
                {
                    data.UpdateCustomer(c); //send the updates customer to dll
                }
                catch (IDAL.DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
            else
                throw new WrongFormatException("Wrong string format.");
        }

        /// <summary>
        /// Removes a customer from the list
        /// </summary>
        /// <param name="customer">The customer to remove</param>
        public void RemoveCustomer(Customer customer)
        {
            IDAL.DO.Customer temp = new IDAL.DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
            };

            try
            {
                data.RemoveCustomer(temp);
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}