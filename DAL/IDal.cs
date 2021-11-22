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
        public void AddStation(Station station);
        public void AddDrone(Drone drone);
        public void AddCustomer(Customer customer);
        public int AddParcel(Parcel parcel);
        public void AddDroneCharge(DroneCharge d);

        //request functions
        public Station GetStation(int id);
        public IEnumerable<Station> GetStationList();
        public IEnumerable<Station> GetEmptyChargeSlots();
        public Parcel GetParcel(int id);
        public IEnumerable<Parcel> GetParcelList();
        public IEnumerable<Parcel> GetNoDroneParcels();
        public Drone GetDrone(int id);
        public IEnumerable<Drone> GetDroneList();
        public double[] GetDroneElectricityConsumption();
        public Customer GetCustomer(int id);
        public IEnumerable<Customer> GetCustomerList();
        public IEnumerable<DroneCharge> GetDroneChargeList();

        //updating functions
        public void UpdateParcel(Parcel parcel);
        public void UpdateStation(Station station);
        public void UpdateDrone(Drone drone);
        public void UpdateCustomer(Customer customer);

        //delete functions
        public void RemoveStation(Station station);
        public void RemoveDrone(Drone drone);
        public void RemoveCustomer(Customer customer);
        public void RemoveParcel(Parcel parcel);
        public void RemoveDroneCharge(DroneCharge d);

        //actions on parcels
        public void AssignDroneToParcel(int parcelId, int droneId);
        public void ParcelPickUp(int id);
        public void ParcelDelivered(int id);

        //actions on stations
        public void UpdateChargeSlots(int id, int num);
        public double FindDistanceStation(double lng1, double lat1, int id);

        //actions on customers
        public double FindDistanceCustomer(double lng1, double lat1, int id);
    }
}
