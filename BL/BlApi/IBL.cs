using System;
using System.Collections.Generic;
using BO;

namespace BlApi
{
    public interface IBL
    {
        #region Add functions
        #region Add station
        /// <summary>
        /// Adds a station to the list
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(Station station);
        #endregion

        #region Add drone
        /// <summary>
        /// Adds a drone to the list
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationNum"></param>
        public void AddDrone(Drone drone, int stationNum);
        #endregion

        #region Add customer
        /// <summary>
        /// Adds a customer to the list
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer);
        #endregion

        #region Add parcel
        /// <summary>
        /// Adds a parcel to the list
        /// </summary>
        /// <param name="parcel"></param>
        public void AddParcel(Parcel parcel);
        #endregion
        #endregion

        #region Request functions
        #region Get station
        /// <summary>
        /// Returns a station according to ID
        /// </summary>
        /// <param name="id">The ID of the station</param>
        /// <returns>The object of the station</returns>
        public Station GetStation(int id);
        #endregion

        #region Get list id stations
        /// <summary>
        /// Returns the list of stations for a list
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetStationList();
        #endregion

        #region Get list if names of station
        /// <summary>
        /// Returns the list of the station names
        /// </summary>
        /// <returns>List of strings</returns>
        public IEnumerable<string> GetStationNameList();
        #endregion

        #region Get list of stations with available chargers
        /// <summary>
        /// Returns list of stations with available charge slots
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetAvailableChargeSlotsStationList();
        #endregion

        #region Get drone
        /// <summary>
        /// Returns a drone according to ID
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        /// <returns>The object of the drone</returns>
        public Drone GetDrone(int id);
        #endregion

        #region Get list if drones
        /// <summary>
        /// Returns the list of drones
        /// </summary>
        /// <returns>List of drones</returns>
        public IEnumerable<ListDrone> GetDroneList(WeightCategories? wc = null, DroneStatuses? ds = null);
        #endregion

        #region Get customer
        /// <summary>
        /// Returns the customer according to ID
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>Customer object</returns>
        public Customer GetCustomer(int id);
        #endregion

        #region Get list of customers
        /// <summary>
        /// Returns the list of customers
        /// </summary>
        /// <returns>The list of customers</returns>
        /// 
        public IEnumerable<ListCustomer> GetCustomerList();
        #endregion

        #region Get parcel
        /// <summary>
        /// Get a parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The object Parcel</returns>
        public Parcel GetParcel(int id);
        #endregion

        #region Get list of parcels
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetParcelList(WeightCategories? wc = null, ParcelState? ps = null, Priorities? pr = null);
        #endregion

        #region Get list of parcels without a drone
        /// <summary>
        /// Returns the list of parcels which dont have an assigned drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetNoDroneParcelList();
        #endregion

        #region Get access code
        /// <summary>
        /// Returns the access code for worker interface
        /// </summary>
        /// <returns>The access code</returns>
        public string GetAccessCode();
        #endregion

        #endregion

        #region Update functions

        #region Update drone
        /// <summary>
        /// Update a drone
        /// </summary>
        /// <param name="drone">The updated drone</param>
        public void UpdateDrone(Drone drone);
        #endregion

        #region Update drone name
        /// <summary>
        /// Update the name of the drone
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <param name="name">the new name</param>
        public void UpdateDroneName(int id, string name);
        #endregion

        #region Charge drone
        /// <summary>
        /// Send a drone to charge
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void ChargeDrone(int id);
        #endregion

        #region Release drone charge
        /// <summary>
        /// Releases a drone from charging
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void ReleaseDroneCharge(int id);
        #endregion

        #region Assign drone to parcel
        /// <summary>
        /// Assign a drone to a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void AssignDroneToParcel(int id);
        #endregion

        #region Pick up parcel
        /// <summary>
        /// Update that a drone picked up a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DronePickUp(int id);
        #endregion

        #region Deliver parcel
        /// <summary>
        /// Update that a drone delivered a parcel
        /// </summary>
        /// <param name="id">The ID of the drone</param>
        public void DroneDeliver(int id);
        #endregion

        #region Update parcel
        /// <summary>
        /// Update a parcel
        /// </summary>
        /// <param name="parcel">The updated parcel</param>
        public void UpdateParcel(Parcel parcel);
        #endregion

        #region Update station
        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="station">The updated station</param>
        public void UpdateStation(Station station);
        #endregion

        #region Update station name
        /// <summary>
        /// Update the name of the station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="name">The new name</param>
        public void UpdateStationName(int id, string name);
        #endregion

        #region Update station charge slots
        /// <summary>
        /// Update the number of charge slots in a station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="num">The number to update</param>
        public void UpdateStationChargingSlots(int id, int num);
        #endregion

        #region Update customer
        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customer">The updates customer</param>
        public void UpdateCustomer(Customer customer);
        #endregion

        #region Update customer name
        /// <summary>
        /// Update the name of a cuatomer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="name">The name of the customer</param>
        public void UpdateCustomerName(int id, string name);
        #endregion

        #region Update customer phone
        /// <summary>
        /// Update the phone number of a customer
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <param name="phone">The new phone number</param>
        public void UpdateCustomerPhone(int id, string phone);
        #endregion
        #endregion

        #region Delete functions

        #region Delete station
        /// <summary>
        /// Remove a station from the list
        /// </summary>
        /// <param name="station">The station to remove</param>
        public void RemoveStation(int id);
        #endregion

        #region Delete drone
        /// <summary>
        /// Remove a drone from the list 
        /// </summary>
        /// <param name="drone">The drone to remove</param>
        public void RemoveDrone(int id);
        #endregion

        #region Delete customer
        /// <summary>
        /// Removes a customer from the list
        /// </summary>
        /// <param name="customer">The customer to remove</param>
        public void RemoveCustomer(int id);
        #endregion

        #region Delete parcel
        /// <summary>
        /// Removes a parcel from the list
        /// </summary>
        /// <param name="parcel">The parcel to remove</param>
        public void RemoveParcel(int id);
        #endregion

        #endregion

        #region Simulator
        /// <summary>
        /// activate simulator
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDelegate"></param>
        /// <param name="stopDelegate"></param>
        public void ActivateSimulator(int id, Action updateDelegate, Func<bool> stopDelegate);
        #endregion
    }
}