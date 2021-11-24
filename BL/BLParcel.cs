using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
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
            catch(NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }

            IDAL.DO.Parcel temp = new IDAL.DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            };

            try
            {
                data.AddParcel(temp);
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
            IDAL.DO.Parcel temp = new IDAL.DO.Parcel()
            {
                Id = parcel.Id,
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                Requested = parcel.Requested,
                DroneId = parcel.AssignedDrone.Id,
                Scheduled = parcel.Scheduled,
                PickedUp = parcel.PickedUp,
                Delivered = parcel.Delivered
            };

            data.UpdateParcel(temp);
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
                IDAL.DO.Parcel temp = data.GetParcel(id); //get the parcel from the data layer

                Parcel p = new Parcel() //copy the fields to a bl parel type
                {
                    Id = temp.Id,
                    Sender = GetCustomerInParcel(temp.SenderId),
                    Target = GetCustomerInParcel(temp.TargetId),
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
            catch (IDAL.DO.NoIDException ex)
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
            IEnumerable<IDAL.DO.Parcel> parcels = data.GetParcelList(); //getting the parcels from data layer

            List<ListParcel> listParcels = new List<ListParcel>();

            foreach (IDAL.DO.Parcel p in parcels)
            {
                ListParcel temp = new ListParcel()
                {
                    Id = p.Id,
                    SenderName = data.GetCustomer(p.SenderId).Name,
                    TargetName = data.GetCustomer(p.TargetId).Name,
                    Weight = (WeightCategories)p.Weight,
                    Priority = (Priorities)p.Priority
                };
                if (p.Delivered != DateTime.MinValue)
                    temp.State = ParcelState.Delivered;
                else if (p.PickedUp != DateTime.MinValue)
                    temp.State = ParcelState.PickedUp;
                else if (p.Scheduled != DateTime.MinValue)
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
        public IEnumerable<ListParcel>GetNoDroneParcelList()
        {
            List<ListParcel> parcels = new List<ListParcel>();

            foreach(ListParcel p in GetParcelList())
            {
                if (p.State == ParcelState.Requested)
                    parcels.Add(p);
            }

            if (parcels.Count == 0)
                throw new EmptyListException("No Parcels with no drone to display");

            return parcels;
        }

        /// <summary>
        /// Returns a list of Parcel object
        /// </summary>
        /// <returns>The list of parcels</returns>
        private IEnumerable<Parcel> getListOfParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels = data.GetParcelList(); //getting the parcels from data layer

            List<Parcel> ps = new List<Parcel>();

            foreach (IDAL.DO.Parcel p in parcels)
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

            foreach(Parcel p in getListOfParcels())
            {
                if (p.Scheduled == DateTime.MinValue)
                    parcels.Add(p);
            }

            if (parcels.Count == 0)
                throw new EmptyListException("No Parcels with no drone to display");

            return parcels;
        }

        /// <summary>
        /// Removes a parcel from the list
        /// </summary>
        /// <param name="parcel">The parcel to remove</param>
        public void RemoveParcel(Parcel parcel)
        {
            IDAL.DO.Parcel temp = new IDAL.DO.Parcel()
            {
                SenderId = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                Requested = DateTime.Now,
                DroneId = 0,
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue
            };

            try
            {
                data.RemoveParcel(temp);
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
    }
}