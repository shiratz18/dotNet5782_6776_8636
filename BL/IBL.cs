using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //add functions
        /// <summary>
        /// Adds a station to the list
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(Station station);
        /// <summary>
        /// Adds a drone to the list
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationNum"></param>
        public void AddDrone(Drone drone, int stationNum);
        /// <summary>
        /// Adds a customer to the list
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer);
        /// <summary>
        /// Adds a parcel to the list
        /// </summary>
        /// <param name="parcel"></param>
        public void AddParcel(Parcel parcel);

        //request functions
        /// <summary>
        /// Returns a station according to ID
        /// </summary>
        /// <param name="id">The ID of the station</param>
        /// <returns>The object of the station</returns>
        public Station GetStation(int id);
        /// <summary>
        /// Returns the list of stations for a list
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetStationList();
        /// <summary>
        /// Returns list of stations with available charge slots
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetAvailableChargeSlotsStationList();
        /// <summary>
        /// Returns a drone according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object of the drone</returns>
        public Drone GetDrone(int id);
        /// <summary>
        /// Returns the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<ListDrone> GetDroneList();
        /// <summary>
        /// Returns the customer according to ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>Customer object</returns>
        public Customer GetCustomer(int id);
        /// <summary>
        /// Returns the list of customers
        /// </summary>
        /// <returns>The list of customers</returns>
        /// 
        public IEnumerable<ListCustomer> GetCustomerList();
        /// <summary>
        /// Get a parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The object Parcel</returns>
        public Parcel GetParcel(int id);
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetParcelList();
        /// <summary>
        /// Returns the list of parcels which dont have an assigned drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetNoDroneParcelList();

        //update functions 

        /// <summary>
        /// Update a drone
        /// </summary>
        /// <param name="drone">The updated drone</param>
        public void UpdateDrone(Drone drone);
        /// <summary>
        /// Update the name of the drone
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <param name="name">the new name</param>
        public void UpdateDroneName(int id, string name);
        /// <summary>
        /// Send a drone to charge
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void ChargeDrone(int id);
        /// <summary>
        /// Releases a drone from charging
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void ReleaseDroneCharge(int id);
        /// <summary>
        /// Assign a drone to a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void AssignDroneToParcel(int id);
        /// <summary>
        /// Update that a drone picked up a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DronePickUp(int id);
        /// <summary>
        /// Update that a drone delivered a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DroneDeliver(int id);
        /// <summary>
        /// Update a parcel
        /// </summary>
        /// <param name="parcel">The updated parcel</param>
        public void UpdateParcel(Parcel parcel);
        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="station">The updated station</param>
        public void UpdateStation(Station station);
        /// <summary>
        /// Update the name of the station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="name">The new name</param>
        public void UpdateStationName(int id, string name);
        /// <summary>
        /// Update the number of charge slots in a station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="num">The number to update</param>
        public void UpdateStationChargingSlots(int id, int num);
        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customer">The updates customer</param>
        public void UpdateCustomer(Customer customer);
        /// <summary>
        /// Update the name of a cuatomer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="name">The name of the customer</param>
        public void UpdateCustomerName(int id, string name);
        /// <summary>
        /// Update the phone number of a customer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="phone">The new phone number</param>
        public void UpdateCustomerPhone(int id, string phone);
        
        //delete functions

        /// <summary>
        /// Remove a station from the list
        /// </summary>
        /// <param name="station">The station to remove</param>
        public void RemoveStation(Station station);
        /// <summary>
        /// Remove a drone from the list 
        /// </summary>
        /// <param name="drone">The drone to remove</param>
        public void RemoveDrone(Drone drone);
        /// <summary>
        /// Removes a customer from the list
        /// </summary>
        /// <param name="customer">The customer to remove</param>
        public void RemoveCustomer(Customer customer);
        /// <summary>
        /// Removes a parcel from the list
        /// </summary>
        /// <param name="parcel">The parcel to remove</param>
        public void RemoveParcel(Parcel parcel);
    }
}