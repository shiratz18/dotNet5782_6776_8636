using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// Add a parcel to the list
        /// </summary>
        /// <param name="parcel">The parcel to add</param>
        public void AddParcel(Parcel parcel)
        {
            try //checking if the customers in the parcel exist
            {
                //get function will throw an exception if the customers do not exist
                GetCustomer(parcel.Sender.Id);
                GetCustomer(parcel.Target.Id);
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            DO.Parcel temp = new DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (DO.WeightCategories)parcel.Weight,
                Priority = (DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            };

            try
            {
                Data.AddParcel(temp);
            }
            catch (DoubleIDException ex)
            {
                throw new DoubleIDException(ex.Message);
            }
        }

        /// <summary>
        /// Update a parcel
        /// </summary>
        /// <param name="parcel">The updated parcel</param>
        public void UpdateParcel(Parcel parcel)
        {
            DO.Parcel temp = new DO.Parcel()
            {
                Id = parcel.Id,
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (DO.WeightCategories)parcel.Weight,
                Priority = (DO.Priorities)parcel.Priority,
                Requested = parcel.Requested,
                DroneId = parcel.AssignedDrone.Id,
                Scheduled = parcel.Scheduled,
                PickedUp = parcel.PickedUp,
                Delivered = parcel.Delivered
            };

            Data.UpdateParcel(temp);
        }

        /// <summary>
        /// Get a parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The object Parcel</returns>
        public Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel temp = Data.GetParcel(id); //get the parcel from the data layer

                Parcel p = new Parcel() //copy the fields to a bl parel type
                {
                    Id = temp.Id,
                    Sender = getCustomerInParcel(temp.SenderId),
                    Target = getCustomerInParcel(temp.TargetId),
                    Weight = (WeightCategories)temp.Weight,
                    Priority = (Priorities)temp.Priority,
                    AssignedDrone = getDroneOfParcel(temp.DroneId),
                    Requested = temp.Requested,
                    Scheduled = temp.Scheduled,
                    PickedUp = temp.PickedUp,
                    Delivered = temp.Delivered
                };

                return p;
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetParcelList()
        {
            IEnumerable<DO.Parcel> parcels = Data.GetParcelList(); //getting the parcels from data layer

            List<ListParcel> listParcels = new List<ListParcel>();

            foreach (DO.Parcel p in parcels)
            {
                ListParcel temp = new ListParcel()
                {
                    Id = p.Id,
                    SenderName = Data.GetCustomer(p.SenderId).Name,
                    TargetName = Data.GetCustomer(p.TargetId).Name,
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority
                };
                if (p.Delivered != null)
                    temp.State = ParcelState.Delivered;
                else if (p.PickedUp != null)
                    temp.State = ParcelState.PickedUp;
                else if (p.Scheduled != null)
                    temp.State = ParcelState.PickedUp;
                else
                    temp.State = ParcelState.Requested;

                listParcels.Add(temp);
            }

            return listParcels;
        }

        /// <summary>
        /// Returns the list of parcels which dont have an assigned drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        public IEnumerable<ListParcel> GetNoDroneParcelList()
        {
            IEnumerable<DO.Parcel> temps = Data.GetParcelList(p => { return p.DroneId == 0; });
            if (temps.Count() == 0)
                throw new EmptyListException("No Parcels with no drone to display");

            List<ListParcel> parcels = new List<ListParcel>();

            foreach (DO.Parcel p in temps)
            {
                ListParcel temp = new ListParcel()
                {
                    Id = p.Id,
                    SenderName = Data.GetCustomer(p.SenderId).Name,
                    TargetName = Data.GetCustomer(p.TargetId).Name,
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority
                };
                if (p.Delivered != null)
                    temp.State = ParcelState.Delivered;
                else if (p.PickedUp != null)
                    temp.State = ParcelState.PickedUp;
                else if (p.Scheduled != null)
                    temp.State = ParcelState.PickedUp;
                else
                    temp.State = ParcelState.Requested;

                parcels.Add(temp);
            }
            return parcels;
        }

        /// <summary>
        /// Returns a list of Parcel object
        /// </summary>
        /// <returns>The list of parcels</returns>
        private IEnumerable<Parcel> getListOfParcels()
        {
            IEnumerable<DO.Parcel> parcels = Data.GetParcelList(); //getting the parcels from data layer

            List<Parcel> ps = new List<Parcel>();

            foreach (DO.Parcel p in parcels)
            {
                Parcel temp = new Parcel()
                {
                    Id = p.Id,
                    Sender = getCustomerInParcel(p.SenderId),
                    Target = getCustomerInParcel(p.TargetId),
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority,
                    Requested = p.Requested,
                    Scheduled = p.Scheduled,
                    PickedUp = p.PickedUp,
                    Delivered = p.Delivered
                };
                if (p.DroneId != 0)
                    temp.AssignedDrone = getDroneOfParcel(p.DroneId);

                ps.Add(temp);
            }

            return ps;
        }

        /// <summary>
        /// Returns a list of parcels without a drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        private IEnumerable<Parcel> getListOfNoDroneParcels()
        {
            List<Parcel> parcels = new List<Parcel>();

            IEnumerable<DO.Parcel> tempParcels = Data.GetParcelList(p => { return p.Scheduled == null; });

            if (tempParcels.Count() == 0)
                throw new EmptyListException("No parcels without a drone.");

            foreach (DO.Parcel p in tempParcels)
            {
                parcels.Add(new Parcel()
                {
                    Id = p.Id,
                    Sender = getCustomerInParcel(p.SenderId),
                    Target = getCustomerInParcel(p.TargetId),
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority,
                    Requested = p.Requested,
                    Scheduled = p.Scheduled,
                    PickedUp = p.PickedUp,
                    Delivered = p.Delivered
                });
            }

            return parcels;
        }

        /// <summary>
        /// Removes a parcel from the list
        /// </summary>
        /// <param name="parcel">The parcel to remove</param>
        public void RemoveParcel(Parcel parcel)
        {
            DO.Parcel temp = new DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (DO.WeightCategories)parcel.Weight,
                Priority = (DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            };

            try
            {
                Data.RemoveParcel(temp);
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}