using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalXml
    {
        #region Add station
        /// <summary>
        /// Add station to list in StationsXML
        /// </summary>
        /// <param name="station">The station to add</param>
        public void AddStation(Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<Station>(stationsPath); //getting station list from xml file

            if (stationList.Exists(s => s.Id == station.Id))
                throw new DoubleIDException($"Station {station.Id} already exists.");

            stationList.Add(station);

            XmlTools.SaveListToXMLSerializer(stationList, stationsPath);
        }
        #endregion

        #region Get station
        /// <summary>
        /// Get the station that matches the ID
        /// </summary>
        /// <param name="id">ID to match</param>
        /// <returns>The station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);

            if (!stationList.Exists(s => s.Id == id))
                throw new NoIDException($"Station {id} doesn't exist.");

            return stationList.Find(s => s.Id == id);
        }
        #endregion

        #region Get station list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition. (or the entire list if no predicate was given)
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The list</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStationList(Predicate<Station> predicate = null)
        {
            if (predicate != null)
                return XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath).Where(x => predicate(x) && x.Active);
            else
                return XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath).Where(x=>x.Active);
        }
        #endregion

        #region Update station
        /// <summary>
        /// Updates station in the list
        /// </summary>
        /// <param name="station">The updates station</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);

            if (!stationList.Exists(s => s.Id == station.Id))
                throw new NoIDException($"Station {station.Id} doesn't exist.");


            stationList[stationList.FindIndex(s => s.Id == station.Id)] = station;

            XmlTools.SaveListToXMLSerializer(stationList, stationsPath);
        }
        #endregion

        #region Edit station name
        /// <summary>
        /// Updates the name of a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="name">The new name</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EditStationName(int id, string name)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);

            if (!stationList.Exists(s => s.Id == id))
                throw new NoIDException($"Station {id} doesn't exist.");

            Station station = stationList.Find(s => s.Id == id);
            station.Name = name;
            stationList[stationList.FindIndex(s => s.Id == station.Id)] = station;
            XmlTools.SaveListToXMLSerializer(stationList, stationsPath);
        }
        #endregion

        #region Update charge slots
        /// <summary>
        /// Update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="num">The number to substract</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateChargeSlots(int id, int num)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);

            if (!stationList.Exists(s => s.Id == id))
                throw new NoIDException($"Station {id} doesn't exist.");

            Station station = stationList.Find(s => s.Id == id);
            station.ChargeSlots -= num;
            stationList[stationList.FindIndex(s => s.Id == station.Id)] = station;
            XmlTools.SaveListToXMLSerializer(stationList, stationsPath);
        }
        #endregion

        #region Remove station
        /// <summary>
        /// Mark a station as deleted
        /// </summary>
        /// <param name="station">The station to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationsPath);

            if (!stationList.Exists(s => s.Id == station.Id))
                throw new NoIDException($"Station {station.Id} doesn't exist.");

            station.Active = false;
            stationList[stationList.FindIndex(s => s.Id == station.Id)] = station;
            XmlTools.SaveListToXMLSerializer(stationList, stationsPath);
        }
        #endregion
    }
}


