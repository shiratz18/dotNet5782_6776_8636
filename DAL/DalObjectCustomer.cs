﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// adds a customer to list of customers
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (DataSource.Customers.Exists(c => c.Id == customer.Id))
            {
                throw new DoubleIDException($"Customer {customer.Id} already exists.");
            }

            DataSource.Customers.Add(customer);
        }

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

            DataSource.Customers[DataSource.Customers.FindIndex(c => c.Id == customer.Id)]= customer;
        }

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

            DataSource.Customers.Remove(customer);
        }

        /// <summary>
        /// returns the object Customer that matches the id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            if (!DataSource.Customers.Exists(c => c.Id == id))
            {
                throw new NoIDException($"Customer {id} doesn't exist.");
            }

            return DataSource.Customers.Find(x => x.Id == id);
        }

        /// <summary>
        /// returns the list of customers
        /// </summary>
        /// <returns>list Customers</returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.Customers;
        }

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
    }
}