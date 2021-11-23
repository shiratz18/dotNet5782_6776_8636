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
            if (customer.Id < 100000000 || customer.Id > 999999999)
                throw new InvalidNumberException($"{customer.Id} is an invalid ID. ID must be 9 digits.");
            

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
            IDAL.DO.Customer temp;

            try
            {
                temp = data.GetCustomer(id); //getting the customer from dal
            }
            catch (IDAL.DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            Customer c = new Customer()
            {
                Id = temp.Id,
                Name = temp.Name,
                Phone = temp.Phone,
                Location = new Location { Longitude = temp.Longitude, Latitude = temp.Latitude }
            };

            IEnumerable<Parcel> parcels = getListOfParcels();
            //for each parcel
            foreach (Parcel p in parcels)
            {
                if (p.Sender.Id == c.Id) //if the sender is the requested customer, so add the parcel to the list of parcels from customer
                    c.FromCustomer.Add(getParcelAtCustomer(p.Id, c.Id));

                if (p.Target.Id == c.Id) //if the target is the requsted customer, add the parcel to the list of parcels to customer
                    c.ToCustomer.Add(getParcelAtCustomer(p.Id, c.Id));
            }

            return c;
        }

        /// <summary>
        /// Returns the list of customers
        /// </summary>
        /// <returns>The list of customers</returns>
        public IEnumerable<ListCustomer> GetCustomerList()
        {
            List<ListCustomer> customers = new List<ListCustomer>();

            foreach (IDAL.DO.Customer c in data.GetCustomerList())
            {
                customers.Add(new ListCustomer()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone
                });
            }

            if (customers.Count == 0)
                throw new EmptyListException("No customers t display.");

            foreach (ListParcel lp in GetParcelList()) //for each parcl
            {
                if (lp.State == ParcelState.Delivered) //if it was delivered
                {
                    customers[customers.FindIndex(c => c.Name == lp.SenderName)].DeliveredFromCustomer++; //add 1 to the number of delivered from of the sender
                    customers[customers.FindIndex(c => c.Name == lp.TargetName)].DeliveredToCustomer++; //add 1 to the umber of parcels delivered to the target
                }
                else
                {
                    customers[customers.FindIndex(c => c.Name == lp.SenderName)].NotDeliveredFromCustomer++; //add 1 to the number of not yet delivered from of the sender
                    customers[customers.FindIndex(c => c.Name == lp.TargetName)].NotDeliveredToCustomer++; //add 1 to the umber of parcels not yet delivered to the target
                }
            }

            return customers;
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
