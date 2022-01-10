using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;
namespace BL
{
    partial class BL
    {
        #region Add parcel
        /// <summary>
        /// Add a parcel to the list
        /// </summary>
        /// <param name="parcel">The parcel to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            //will throw an exception if the customers in the parcel do not exist
            try
            {
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
                Scheduled = null,
                PickedUp = null,
                Delivered = null
            };

            try
            {
                lock (Data)
                {
                    Data.AddParcel(temp);
                }
            }
            catch (DoubleIDException ex)
            {
                throw new DoubleIDException(ex.Message);
            }
        }
        #endregion

        #region Update parcel
        /// <summary>
        /// Update a parcel
        /// </summary>
        /// <param name="parcel">The updated parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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

            lock (Data)
            {
                Data.UpdateParcel(temp);
            }
        }
        #endregion

        #region Get parcel
        /// <summary>
        /// Get a parcel according to ID
        /// </summary>
        /// <param name="id">The ID of the parcel</param>
        /// <returns>The object Parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            lock (Data)
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
                        Requested = temp.Requested,
                        Scheduled = temp.Scheduled,
                        PickedUp = temp.PickedUp,
                        Delivered = temp.Delivered
                    };

                    if (p.Scheduled != null)
                        p.AssignedDrone = getDroneOfParcel(temp.DroneId);

                    return p;
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
        }
        #endregion

        #region Get parcel list
        /// <summary>
        /// Returns the list of parcels
        /// </summary>
        /// <returns>The list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ListParcel> GetParcelList(WeightCategories? wc = null, ParcelState? ps = null, Priorities? pr = null)
        {
            lock (Data)
            {
                List<ListParcel> listParcels = new List<ListParcel>();

                if (wc != null)
                {
                    if (ps != null)
                    {
                        if (pr != null)
                        {
                            foreach (DO.Parcel p in Data.GetParcelList(x => x.Weight == (DO.WeightCategories)wc && x.Priority == (DO.Priorities)pr))
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

                                if (temp.State == ps)
                                    listParcels.Add(temp);
                            }
                        }
                    }
                    else if (pr != null)
                    {
                        foreach (DO.Parcel p in Data.GetParcelList(x => x.Weight == (DO.WeightCategories)wc && x.Priority == (DO.Priorities)pr))
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
                    }
                    else
                    {
                        foreach (DO.Parcel p in Data.GetParcelList(x => x.Weight == (DO.WeightCategories)wc))
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
                    }
                }

                else if (ps != null)
                {
                    if (pr != null)
                    {
                        foreach (DO.Parcel p in Data.GetParcelList(x => x.Priority == (DO.Priorities)pr))
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

                            if (temp.State == ps)
                                listParcels.Add(temp);
                        }
                    }
                    else
                    {
                        foreach (DO.Parcel p in Data.GetParcelList())
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

                            if (temp.State == ps)
                                listParcels.Add(temp);
                        }
                    }
                }
                else if (pr != null)
                {
                    foreach (DO.Parcel p in Data.GetParcelList(x => x.Priority == (DO.Priorities)pr))
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
                }
                else
                {
                    foreach (DO.Parcel p in Data.GetParcelList())
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
                }

                return listParcels;
            }
        }

        #region Get parcels with no drone
        /// <summary>
        /// Returns the list of parcels which dont have an assigned drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ListParcel> GetNoDroneParcelList()
        {
            lock (Data)
            {
                IEnumerable<DO.Parcel> temps = Data.GetParcelList(p => p.DroneId == 0);

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
        }
        #endregion


        /// <summary>
        /// Returns a list of Parcel object
        /// </summary>
        /// <returns>The list of parcels</returns>

        private IEnumerable<Parcel> getListOfParcels()
        {
            lock (Data)
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
        }

        /// <summary>
        /// Returns a list of parcels without a drone
        /// </summary>
        /// <returns>The list of parcels</returns>
        private IEnumerable<Parcel> getListOfNoDroneParcels()
        {
            lock (Data)
            {
                List<Parcel> parcels = new List<Parcel>();

                IEnumerable<DO.Parcel> tempParcels = Data.GetParcelList(p => p.Scheduled == null);


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
        }
        #endregion

        #region Remove parcel
        /// <summary>
        /// Removes a parcel from the list
        /// </summary>
        /// <param name="parcel">The parcel to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            try
            {
                Parcel parcel = GetParcel(id);

                if (parcel.Scheduled == null)
                {
                    DO.Parcel temp = new DO.Parcel()
                    {
                        Id = id,
                        SenderId = parcel.Sender.Id,
                        TargetId = parcel.Target.Id,
                        Weight = (DO.WeightCategories)parcel.Weight,
                        Priority = (DO.Priorities)parcel.Priority,
                        Requested = parcel.Requested,
                        Scheduled = parcel.Scheduled,
                        PickedUp = parcel.PickedUp,
                        Delivered = parcel.Delivered
                    };

                    lock (Data)
                    {
                        Data.RemoveParcel(temp);
                    }
                }
                else
                    throw new CannotDeleteException("Cannot delete this parcel since it has already been assigned to a drone.");
            }
            catch (NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }
        #endregion
    }
}