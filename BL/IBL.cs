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
        public void AddStation(Station station);
        public void AddDrone(Drone drone, int stationNum);
        public void AddCustomer(Customer customer);
        public void AddParcel(Parcel parcel);

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
