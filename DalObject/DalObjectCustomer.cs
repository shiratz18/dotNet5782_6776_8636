using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalObject
    {
        #region Add customer
        /// <summary>
        /// adds a customer to list of customers
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (DataSource.Customers.Exists(c => c.Id == customer.Id && c.Active)) //if this customer exists and is active
            {
                throw new DoubleIDException($"Customer {customer.Id} already exists.");
            }

            DataSource.Customers.Add(customer);
        }
        #endregion

        #region Update customer
        /// <summary>
        /// updates a customer in the list
        /// </summary>
        /// <param name="customer">the updated customer</param>
        public void UpdateCustomer(Customer customer)
        {
            if (!DataSource.Customers.Exists(c => c.Id == customer.Id))
            {
                throw new NoIDException($"Customer {customer.Id} doesn't exist.");
            }

            DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == customer.Id)] = customer;
        }

        /// <summary>
        /// Updates the name of a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="name">The new name</param>
        public void EditCustomerName(int id, string name)
        {
            if (!DataSource.Customers.Exists(c => c.Id == id))
            {
                throw new NoIDException($"Customer {id} doesn't exist.");
            }

            Customer customer = DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == id)];
            customer.Name = name;
            DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == id)] = customer;
        }

        /// <summary>
        /// Update a customer phone number
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="phone">The new phone</param>
        public void EditCustomerPhone(int id, string phone)
        {
            if (!DataSource.Customers.Exists(c => c.Id == id))
            {
                throw new NoIDException($"Customer {id} doesn't exist.");
            }

            Customer customer = DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == id)];
            customer.Phone = phone;
            DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == id)] = customer;
        }
        #endregion

        #region Remove customer
        /// <summary>
        /// removes a customer from the list
        /// </summary>
        /// <param name="customer">the customer to remove</param>
        public void RemoveCustomer(Customer customer)
        {
            if (!DataSource.Customers.Exists(c => c.Id == customer.Id))
            {
                throw new NoIDException($"Customer {customer.Id} doesn't exist.");
            }

            customer.Active = false;
            DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == customer.Id)] = customer; //changing the customer to be inactive
        }
        #endregion

        #region Get customer
        /// <summary>
        /// returns the object Customer that matches the id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            if (!DataSource.Customers.Exists(c => c.Id == id))
                throw new NoIDException($"Customer {id} doesn't exist.");

            return DataSource.Customers.Find(x => x.Id == id);
        }
        #endregion

        #region Get customer list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Customers> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Customer> GetCustomerList(Predicate<Customer> predicate = null)
        {
            if (predicate != null)
                return DataSource.Customers.FindAll(c => predicate(c) && c.Active);
            else
                return DataSource.Customers.Where(c => c.Active);
        }
        #endregion

        #region Get distance to a customer
        /// <summary>
        /// finds the distance from a customer
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public double FindDistanceCustomer(double lng1, double lat1, int id)
        {
            if (!DataSource.Customers.Exists(c => c.Id == id))
            {
                throw new NoIDException($"Customer {id} doesn't exist.");
            }

            Customer temp = DataSource.Customers.Find(x => x.Id == id);
            Double lat2 = temp.Latitude, lng2 = temp.Longitude;
            return Location.GetDistance(lng1, lat1, lng2, lat2);
        }
        #endregion
    }
}
