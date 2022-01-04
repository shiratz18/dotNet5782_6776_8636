using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalXml
    {
        #region Add customer
        /// <summary>
        /// adds a customer to list of customers
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath); 

            if (customerList.Exists(c => c.Id == customer.Id))
                throw new DoubleIDException($"Customer {customer.Id} already exists.");

            customerList.Add(customer);

            XmlTools.SaveListToXMLSerializer(customerList, customersPath);
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
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (!customerList.Exists(c => c.Id == id))
                throw new NoIDException($"Customer {id} doesn't exist.");

            return customerList.Find(s => s.Id == id);
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
                return XmlTools.LoadListFromXMLSerializer<Customer>(customersPath).Where(x => predicate(x) && x.Active);
            else
                return XmlTools.LoadListFromXMLSerializer<Customer>(customersPath).Where(x => x.Active);
        }
        #endregion

        #region Update customer
        /// <summary>
        /// updates a customer in the list
        /// </summary>
        /// <param name="customer">the updated customer</param>
        public void UpdateCustomer(Customer customer)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath); 
            
            if (!customerList.Exists(c => c.Id == customer.Id))
                throw new NoIDException($"Customer {customer.Id} doesn't exist.");

            customerList[customerList.FindIndex(c => c.Id == customer.Id)] = customer;

            XmlTools.SaveListToXMLSerializer(customerList, customersPath);
        }
        #endregion

        #region Edit customer name
        /// <summary>
        /// Updates the name of a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="name">The new name</param>
        public void EditCustomerName(int id, string name)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (!customerList.Exists(c => c.Id == id))
                throw new NoIDException($"Customer {id} doesn't exist.");

            Customer customer = customerList.Find(c => c.Id == id);
            customer.Name = name;
            customerList[customerList.FindIndex(c => c.Id == customer.Id)] = customer;

            XmlTools.SaveListToXMLSerializer(customerList, customersPath);
        }
        #endregion

        #region Edit customer phone
        /// <summary>
        /// Update a customer phone number
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="phone">The new phone</param>
        public void EditCustomerPhone(int id, string phone)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (!customerList.Exists(c => c.Id == id))
                throw new NoIDException($"Customer {id} doesn't exist.");

            Customer customer = customerList.Find(c => c.Id == id);
            customer.Phone = phone;
            customerList[customerList.FindIndex(c => c.Id == customer.Id)] = customer;

            XmlTools.SaveListToXMLSerializer(customerList, customersPath);
        }
        #endregion

        #region Remove customer
        /// <summary>
        /// removes a customer from the list
        /// </summary>
        /// <param name="customer">the customer to remove</param>
        public void RemoveCustomer(Customer customer)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (!customerList.Exists(c => c.Id == customer.Id))
                throw new NoIDException($"Customer {customer.Id} doesn't exist.");

            customer.Active = false;

            XmlTools.SaveListToXMLSerializer(customerList, customersPath);
        }
        #endregion
    }
}
