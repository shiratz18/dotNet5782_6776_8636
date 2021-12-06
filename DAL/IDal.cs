using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        //create functions

        /// <summary>
        /// Adds a station to the list of stations 
        /// </summary>
        /// <param name="station">The station to add</param>
        public void AddStation(Station station);
        /// <summary>
        /// Adds a drone to the list of drones
        /// </summary>
        /// <param name="drone">The drone to add</param>
        public void AddDrone(Drone drone);
        /// <summary>
        /// Adds a customer to list of customers
        /// </summary>
        /// <param name="customer">The customer to add</param>
        public void AddCustomer(Customer customer);
        /// <summary>
        /// Adds a parcel to the list of parcels
        /// </summary>
        /// <param name="parcel">The parcel to add</param>
        /// <returns></returns>
        public int AddParcel(Parcel parcel);
        /// <summary>
        /// Add a drone to charge
        /// </summary>
        /// <param name="d">Drone charge object</param>
        public void AddDroneCharge(DroneCharge d);

        //request functions

        /// <summary>
        /// Returns the object Station that matches the ID
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <returns>The station</returns>
        public Station GetStation(int id);
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Stations> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Station> GetStationList(Predicate<Station> predicate = null);
        /// <summary>
        /// Returns the object Parcel that matches the ID
        /// </summary>
        /// <param name="id">The parcel ID</param>
        /// <returns>The parcel</returns>
        public Parcel GetParcel(int id);
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Parcel></Parcel> containing the elements that match the predicate condition if found, otherwisr retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate = null);
        /// <summary>
        /// Returns the object Drone that matches the ID
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <returns>The drone</returns>
        public Drone GetDrone(int id);
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Drone></Drone> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Drone> GetDroneList(Predicate<Drone> predicate = null);
        /// <summary>
        /// Returns an array with info about the electricity consumption of a drone
        /// </summary>
        /// <returns>The array</returns>
        public double[] GetDroneElectricityConsumption();
        /// <summary>
        /// Returns the object Customer that matches the id
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>The customer</returns>
        public Customer GetCustomer(int id);
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Customers> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Customer> GetCustomerList(Predicate<Customer> predicate = null);
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<DroneCharge> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate = null);

        //updating functions

        /// <summary>
        /// Updates a parcel in the list
        /// </summary>
        /// <param name="parcel">The updated parcel</param>
        public void UpdateParcel(Parcel parcel);
        /// <summary>
        /// Updates a station in the list
        /// </summary>
        /// <param name="station">The updated station</param>
        public void UpdateStation(Station station);
        /// <summary>
        /// Updates the name of a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="name">The new name</param>
        public void EditStationName(int id, string name);
        /// <summary>
        /// Update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="num">The number to update</param>
        public void UpdateChargeSlots(int id, int num);
        /// <summary>
        /// Updates a drone in the list
        /// </summary>
        /// <param name="drone">The updated drone</param>
        public void UpdateDrone(Drone drone);
        /// <summary>
        /// Updates the ID of a drone
        /// </summary>
        /// <param name="currentId">The current drone ID</param>
        /// <param name="newId">The new ID</param>
        public void EditDroneId(int currentId, int newId);
        /// <summary>
        /// Updates the drone name
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="name">The new name</param>
        public void EditDroneModel(int id, string model);
        /// <summary>
        /// Updates the maximum weight of the drone
        /// </summary>
        /// <param name="id">The drone ID</param>
        /// <param name="weight">The new maximum weight</param>
        public void EditDroneMaxWeight(int id, WeightCategories weight);
        /// <summary>
        /// Updates a customer in the list
        /// </summary>
        /// <param name="customer">The updated customer</param>
        public void UpdateCustomer(Customer customer);
        /// <summary>
        /// Updates the name of a customer
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="name">The new name</param>
        public void EditCustomerName(int id, string name);
        /// <summary>
        /// Update a customer phone number
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <param name="phone">The new phone</param>
        public void EditCustomerPhone(int id, string phone);

        //delete functions

        /// <summary>
        /// Removes a station from the list
        /// </summary>
        /// <param name="station">The station to remove</param>
        public void RemoveStation(Station station);
        /// <summary>
        /// Removes a drone from the list
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
        /// <summary>
        /// Release a drone from charge
        /// </summary>
        /// <param name="d">Drone charge object</param>
        public void RemoveDroneCharge(DroneCharge d);

        //actions on parcels

        /// <summary>
        /// Assign a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">The ID of the parcel</param>
        /// <param name="droneId">The ID of the drone</param>
        public void AssignDroneToParcel(int parcelId, int droneId);
        /// <summary>
        /// Updates the pick up time of the parcel
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        public void ParcelPickUp(int id);
        /// <summary>
        /// Updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        public void ParcelDelivered(int id);

        //actions on stations

        /// <summary>
        /// Finds the distance from a station
        /// </summary>
        /// <param name="Lat1">Latitude of the location</param>
        /// <param name="Lng1">Longitude of the location</param>
        /// <param name="Id">The station ID</param>
        /// <returns>The distance between the location and the station</returns>
        public double FindDistanceStation(double lng1, double lat1, int id);

        //actions on customers

        /// <summary>
        /// Finds the distance from a customer
        /// </summary>
        /// <param name="Lat1">Latitude of the location</param>
        /// <param name="Lng1">Longitude of the location</param>
        /// <param name="Id">The customer ID</param>
        /// <returns>The distance between the location and the customer</returns>
        public double FindDistanceCustomer(double lng1, double lat1, int id);
    }
}
