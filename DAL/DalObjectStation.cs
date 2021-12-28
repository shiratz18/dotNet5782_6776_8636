using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    partial class DalObject
    {
        #region Add station
        /// <summary>
        /// adds a station to the list of stations
        /// </summary>
        public void AddStation(Station station)
        {
            if (DataSource.Stations.Exists(s => s.Id == station.Id))
            {
                throw new DoubleIDException($"Station {station.Id} already exists.");
            }

            DataSource.Stations.Add(station);
        }
        #endregion

        #region Update station
        /// <summary>
        /// updates the station in the list
        /// </summary>
        /// <param name="station">the updated station</param>
        public void UpdateStation(Station station)
        {
            if (!DataSource.Stations.Exists(s => s.Id == station.Id))
                throw new NoIDException($"Station {station.Id} doesn't exist.");


            DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == station.Id)] = station;
        }

        /// <summary>
        /// Updates the name of a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="name">The new name</param>
        public void EditStationName(int id, string name)
        {
            if (!DataSource.Stations.Exists(s => s.Id == id))
            {
                throw new NoIDException($"Station {id} doesn't exist.");
            }

            Station station = DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == id)];
            station.Name = name;
            DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == id)] = station;
        }

        /// <summary>
        /// update the number of available charge slots in a station
        /// </summary>
        /// <param name="id">station id</param>
        /// <param name="num">the number to updat (add 1 or substarct 1)</param>
        public void UpdateChargeSlots(int id, int num)
        {
            if (!DataSource.Stations.Exists(s => s.Id == id))
            {
                throw new NoIDException($"Station {id} doesn't exist.");
            }

            Station temp = DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == id)];
            temp.ChargeSlots -= num;
            UpdateStation(temp);
        }
        #endregion

        #region Remove station
        /// <summary>
        /// removes a station from the list
        /// </summary>
        /// <param name="station">the station to remove</param>
        public void RemoveStation(Station station)
        {
            if (!DataSource.Stations.Exists(s => s.Id == station.Id))
                throw new NoIDException($"Station {station.Id} doesn't exist.");

            station.Active = false;

            DataSource.Stations[DataSource.Stations.FindIndex(x => x.Id == station.Id)] = station;
        }
        #endregion

        #region Get station
        /// <summary>
        /// returns the object Station that matches the id
        /// </summary>
        /// <param name="id">station id</param>
        /// <returns></returns>
        public Station GetStation(int id)
        {
            if (!DataSource.Stations.Exists(s => s.Id == id))
            {
                throw new NoIDException($"Station {id} doesn't exist.");
            }

            return DataSource.Stations.Find(x => x.Id == id);
        }
        #endregion

        #region Get station list
        /// <summary>
        /// Returns list of elements in the list that match the given predicate's condidition.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A IEnumerable<Stations> containing the elements that match the predicate condition if found, otherwise retruns an empty list. If no predicate was given, returns the entire list.</returns>
        public IEnumerable<Station> GetStationList(Predicate<Station> predicate = null)
        {
            if (predicate != null)
                return DataSource.Stations.FindAll(x => predicate(x) && x.Active);
            else
                return DataSource.Stations.FindAll(x => x.Active);
        }
        #endregion

        #region Returns distance from a station
        /// <summary>
        /// finds the distance from a station
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Lng1"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public double FindDistanceStation(double lng1, double lat1, int id)
        {
            if (!DataSource.Stations.Exists(s => s.Id == id))
            {
                throw new NoIDException($"Station {id} doesn't exist.");
            }

            Station temp = DataSource.Stations.Find(x => x.Id == id);
            Double lat2 = temp.Latitude, lng2 = temp.Longitude;
            return Location.GetDistance(lng1, lat1, lng2, lat2);
        }
        #endregion
    }
}
