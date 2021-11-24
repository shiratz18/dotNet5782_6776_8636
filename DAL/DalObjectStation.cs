using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {
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

        /// <summary>
        /// updates the station in the list
        /// </summary>
        /// <param name="station">the updated station</param>
        public void UpdateStation(Station station)
        {
            if (!DataSource.Stations.Exists(s => s.Id == station.Id))
            {
                throw new NoIDException($"Station {station.Id} doesn't exist.");
            }

            DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == station.Id)] = station;
        }

        /// <summary>
        /// Updates the name of a station
        /// </summary>
        /// <param name="id">The station ID</param>
        /// <param name="name">The new name</param>
        public void EditStationName(int id,string name)
        {
            if(!DataSource.Stations.Exists(s => s.Id == id))
            {
                throw new NoIDException($"Station {id} doesn't exist.");
            }

            Station station=DataSource.Stations[DataSource.Stations.FindIndex(s => s.Id == id)];
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

        /// <summary>
        /// removes a station from the list
        /// </summary>
        /// <param name="station">the station to remove</param>
        public void RemoveStation(Station station)
        {
            if (!DataSource.Stations.Exists(s => s.Id == station.Id))
            {
                throw new NoIDException($"Station {station.Id} doesn't exist.");
            }

            DataSource.Stations.Remove(station);
        }     

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

        /// <summary>
        /// return the list of stations
        /// </summary>
        /// <returns>list Stations</returns>
        public IEnumerable<Station> GetStationList()
        {
            return DataSource.Stations;
        }

        /// <summary>
        /// returns an array with the list of stations with empty charge slots
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetEmptyChargeSlots()
        {
            List<Station> temp = new List<Station>();
            DataSource.Stations.ForEach(x => { if (x.ChargeSlots > 0) temp.Add(x); });
            return temp;
        }

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
    }
}
