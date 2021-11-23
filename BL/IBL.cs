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
        //copy the teud

        //add functions
        public void AddStation(Station station);
        /// <summary>
        /// Adds a drone to the list
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationNum"></param>
        public void AddDrone(Drone drone, int stationNum);
        public void AddCustomer(Customer customer);
        public void AddParcel(Parcel parcel);

        //request functions
        public Station GetStation(int id);
        public IEnumerable<ListStation> GetStationList();
        public Drone GetDrone(int id);
        public IEnumerable<ListDrone> GetDroneList();
        public Customer GetCustomer(int id);
        public IEnumerable<ListCustomer> GetCustomerList();
        public Parcel GetParcel(int id);
        public IEnumerable<ListParcel> GetParcelList();
        public IEnumerable<ListParcel> GetNoDroneParcelList();
        public IEnumerable<ListStation> GetAvailableChargeSlotsStationList();

        //update functions 
        public void UpdateDroneName(int id, string name);
        public void UpdateStationName(int id, string name);
        public void UpdateStationChargingSlots(int id, int num);
        public void UpdateCustomerName(int id, string name);
        public void UpdateCustomerPhone(int id, string phone);
        public void ChargeDrone(int id);
        public void ReleaseDroneCharge(int id, double time);
        public void AssignDroneToParcel(int id);
        public void DronePickUp(int id);
        public void DroneDeliver(int id);


        //delete functions
        public void RemoveStation(Station station);
        public void RemoveDrone(Drone drone);
        public void RemoveCustomer(Customer customer);
        public void RemoveParcel(Parcel parcel);
    }
}