using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        /// <summary>
        /// constructor for DalObject class
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

        ///// <summary>
        ///// updates the status of a drone
        ///// </summary>
        ///// <param name="id">the id of the status</param>
        ///// <param name="status">the status to update</param>
        //public void UpdateDroneStatus(int id, DroneStatuses status)
        //{
        //    Drone temp = new Drone();
        //    foreach (Drone x in DataSource.Drones)
        //    {
        //        if (x.Id == id)
        //        {
        //            temp = x;
        //            temp.Status = status;
        //            DataSource.Drones.Remove(x);
        //            DataSource.Drones.Add(temp);
        //            break;
        //        }
        //    }
        //}

    }
}


