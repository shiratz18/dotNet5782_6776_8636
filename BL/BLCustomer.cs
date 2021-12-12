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
        /// Add a customer to the data list of customers
        /// </summary>
        /// <param name="customer">the customer to add</param>
        public void AddCustomer(Customer customer)
        {
            if (customer.Id < 100000000 || customer.Id > 999999999)
                throw new InvalidNumberException($"{customer.Id} is an invalid ID. ID must be 9 digits.");
            if (customer.Location.Latitude > 31.8830 || customer.Location.Latitude < 31.7082)
                throw new InvalidNumberException($"Longitude {customer.Location.Longitude} is not in Jerusalem.");
            if ((customer.Location.Longitude > 35.2642 || customer.Location.Longitude < 35.1252))
                throw new InvalidNumberException($"Latitude {customer.Location.Longitude} is not in Jerusalem.");
            foreach (char c in customer.Name)
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && c != ' ')
                    throw new WrongFormatException($"Customer name must contain alphabet letters only.");
            if (customer.Phone.Length != 10 || !customer.Phone.All(char.IsDigit))
                throw new WrongFormatException($"Customer phone number must be 10 digits.");

            DO.Customer temp = new DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
            };

            try
            {
                Data.AddCustomer(temp);
            }
            catch (DO.DoubleIDException ex)
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
            DO.Customer temp;

            try
            {
                temp = Data.GetCustomer(id); //getting the customer from dal
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            Customer c = new Customer()
            {
                Id = temp.Id,
                Name = temp.Name,
                Phone = temp.Phone,
                Location = new Location { Longitude = temp.Longitude, Latitude = temp.Latitude },
                FromCustomer = new List<ParcelAtCustomer>(),
                ToCustomer = new List<ParcelAtCustomer>()
            };

            IEnumerable<DO.Parcel> parcels = Data.GetParcelList();
            //for each parcel
            foreach (DO.Parcel p in parcels)
            {
                if (p.SenderId == c.Id) //if the sender is the requested customer, so add the parcel to the list of parcels from customer
                    c.FromCustomer.Add(getParcelAtCustomer(p.Id, c.Id));

                if (p.TargetId == c.Id) //if the target is the requsted customer, add the parcel to the list of parcels to customer
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

            foreach (DO.Customer c in Data.GetCustomerList())
            {
                customers.Add(new ListCustomer()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone
                });
            }

            if (customers.Count == 0)
                throw new EmptyListException("No customers to display.");

            foreach (DO.Parcel p in Data.GetParcelList()) //for each parcel
            {
                if (p.Delivered != null) //if it was delivered
                {
                    customers[customers.FindIndex(c => c.Id == p.SenderId)].DeliveredFromCustomer++; //add 1 to the number of delivered from of the sender
                    customers[customers.FindIndex(c => c.Id == p.TargetId)].DeliveredToCustomer++; //add 1 to the umber of parcels delivered to the target
                }
                else
                {
                    customers[customers.FindIndex(c => c.Id == p.SenderId)].NotDeliveredFromCustomer++; //add 1 to the number of not yet delivered from of the sender
                    customers[customers.FindIndex(c => c.Id == p.TargetId)].NotDeliveredToCustomer++; //add 1 to the umber of parcels not yet delivered to the target
                }
            }

            return customers;
        }

        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customer">The updates customer</param>
        public void UpdateCustomer(Customer customer)
        {
            DO.Customer c = new DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
            };
            Data.UpdateCustomer(c);
        }

        /// <summary>
        /// Update the name of a cuatomer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="name">The name of the customer</param>
        public void UpdateCustomerName(int id, string name)
        {

            foreach (char c in name)
                if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && c != ' ')
                    throw new WrongFormatException($"Customer name must contain alphabet letters only.");

            try
            {
                Data.EditCustomerName(id, name);
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Update the phone number of a customer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="phone">The new phone number</param>
        public void UpdateCustomerPhone(int id, string phone)
        {
            if (phone.Length != 10 || !phone.All(char.IsDigit))
                throw new WrongFormatException($"Customer phone number must be 10 digits.");

            try
            {
                Data.EditCustomerPhone(id, phone);
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Removes a customer from the list
        /// </summary>
        /// <param name="customer">The customer to remove</param>
        public void RemoveCustomer(Customer customer)
        {
            DO.Customer temp = new DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
            };

            try
            {
                Data.RemoveCustomer(temp);
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}
