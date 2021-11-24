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
        /// adds a station to the list of stations
        /// </summary
        public void AddStation(Station station);
        /// <summary>
        /// adds a drone to the list of drones
        /// </summary>
        public void AddDrone(Drone drone);
        /// <summary>
        /// adds a customer to list of customers
        /// </summary>
        public void AddCustomer(Customer customer);
        /// <summary>
        /// adds a parcel to the list of parcels
        /// </summary>
        public int AddParcel(Parcel parcel);
        /// <summary>
        /// send a drone to charge
        /// </summary>
        /// <param name="d">drone charge object</param>
        public void AddDroneCharge(DroneCharge d);

        //request functions
        /// <summary>
        /// returns the object Station that matches the id
        /// </summary>
        /// <param name="id">station id</param>
        /// <returns></returns>
        public Station GetStation(int id);
        /// <summary>
        /// return the list of stations
        /// </summary>
        /// <returns>list Stations</returns>
        public IEnumerable<Station> GetStationList();
        /// <summary>
        /// returns an array with the list of stations with empty charge slots
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetEmptyChargeSlots();


        /// <summary>
        /// returns the object Parcel that matches the id
        /// </summary>
        /// <param name="id">parcel id</param>
        /// <returns></returns>
        public Parcel GetParcel(int id);
        /// <summary>
        /// returns a list of the parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcelList();
        /// <summary>
        /// returns a list with all the parcels that are not associated to a drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetNoDroneParcels();
        /// <summary>
        /// returns the object Drone that matches the id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns></returns>
        public Drone GetDrone(int id);
        /// <summary>
        /// returns the list of drones
        /// </summary>
        /// <returns>list Drones</returns>
        public IEnumerable<Drone> GetDroneList();
        /// <summary>
        /// function returns an array with info about the electricity consumption of a drone
        /// </summary>
        /// <returns>the array</returns>
        public double[] GetDroneElectricityConsumption();
        /// <summary>
        /// returns the object Customer that matches the id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns></returns>
        public Customer GetCustomer(int id);
        /// <summary>
        /// returns the list of customers
        /// </summary>
        /// <returns>list Customers</returns>
        public IEnumerable<Customer> GetCustomerList();
        /// <summary>
        /// returns a list of the drones charging
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneChargeList();

        //updating functions


        /// <summary>
        /// updates a parcel
        /// </summary>
        /// <param name="parcel">the updated parcel</param>
        public void UpdateParcel(Parcel parcel);
        /// <summary>
        /// updates the station in the list
        /// </summary>
        /// <param name="station">the updated station</param>
        public void UpdateStation(Station station);
        /// <summary>
        /// updates a drone in the list
        /// </summary>
        /// <param name="drone">the updated drone</param>
        public void UpdateDrone(Drone drone);
        /// <summary>
        /// updates a customer in the list
        /// </summary>
        /// <param name="customer">the updated customer</param>
        public void UpdateCustomer(Customer customer);

        //delete functions
        /// <summary>
        /// removes a station from the list
        /// </summary>
        /// <param name="station">the station to remove</param>
        public void RemoveStation(Station station);
        /// <summary>
        /// removes a drone from the list
        /// </summary>
        /// <param name="drone">the drone to remove</param>
        public void RemoveDrone(Drone drone);
        /// <summary>
        /// removes a customer from the list
        /// </summary>
        /// <param name="customer">the customer to remove</param>
        public void RemoveCustomer(Customer customer);
        /// <summary>
        /// removes a parcel from the list
        /// </summary>
        /// <param name="parcel">the parcel to remove</param>
        public void RemoveParcel(Parcel parcel);
        /// <summary>
        /// release a drone from charge
        /// </summary>
        /// <param name="d">drone charge object</param>
        public void RemoveDroneCharge(DroneCharge d);

        //actions on parcels

        /// <summary>
        /// assign a drone to a parcel and update the scheduled time
        /// </summary>
        /// <param name="parcelId">the id of the parcel</param>
        /// <param name="droneId">the id of the drone</param>
        public void AssignDroneToParcel(int parcelId, int droneId);
        /// <summary>
        /// updates the pick up time of the parcel
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public void ParcelPickUp(int id);
        /// <summary>
        /// updates that the parcel was delivered to the target
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        public void ParcelDelivered(int id);

        //actions on stations

        /// <summary>
        /// update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">station id</param>
        /// <param name="num">the number to updat (add 1 or substarct 1)</param>
        public void UpdateChargeSlots(int id, int num);
        /// <summary>
        /// finds the distance from a station
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public double FindDistanceStation(double lng1, double lat1, int id);

        //actions on customers

        /// <summary>
        /// finds the distance from a customer
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public double FindDistanceCustomer(double lng1, double lat1, int id);
    }
}
