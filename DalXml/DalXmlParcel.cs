using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalXml
    {
        #region Add parcel
        /// <summary>
        /// adds a parcel to the list of parcels
        /// </summary>
        public int AddParcel(Parcel parcel)
        {
            var customerList = XmlTools.LoadListFromXMLSerializer<Customer>(customersPath);

            if (customerList.Exists(c => c.Id == parcel.SenderId || !c.Active))
                throw new NoIDException($"Customer {parcel.SenderId} doesn't exist.");

            if (customerList.Exists(c => c.Id == parcel.TargetId || !c.Active))
                throw new NoIDException($"Customer {parcel.TargetId} doesn't exist.");

            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            var config = XmlTools.LoadListFromXMLSerializer<string>(configPath);
            int.TryParse(config[5], out int id);
            parcel.Id = id++;
            config[5] = id.ToString();
            XmlTools.SaveListToXMLSerializer(config, configPath);

            parcelList.Add(parcel);
            XmlTools.SaveListToXMLSerializer(parcelList, parcelsPath);

            return id;
        }
        #endregion

        #region Get parcel
        /// <summary>
        /// returns the object Parcel that matches the id
        /// </summary>
        /// <param name="id">parcel id</param>
        /// <returns></returns>
        public Parcel GetParcel(int id)
        {
            var parcelList = XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            if (!parcelList.Exists(p => p.Id == id))
                throw new NoIDException($"Parcel {id} doesn't exist.");

            return parcelList.Find(x => x.Id == id);
        }
        #endregion

        #region Get parcel list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Parcel></Parcel> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> predicate = null)
        {
            if (predicate != null)
                return XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath).Where(x=>predicate(x));
            else
                return XmlTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
        }
    }
}
