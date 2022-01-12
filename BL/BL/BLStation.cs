using System.Collections.Generic;
using System.Linq;
using BO;
using System.Runtime.CompilerServices;
namespace BL
{
    partial class BL
    {
        #region Add station
        /// <summary>
        /// Adds a station to the list of station in data
        /// </summary>
        /// <param name="station">The station to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            if (station.Id < 1000 || station.Id > 9999)
                throw new InvalidNumberException($"Station ID must be 4 digits.");
            if (station.Location.Latitude > 31.8830 || station.Location.Latitude < 31.7082)
                throw new InvalidNumberException($"Longitude {station.Location.Longitude} is not in Jerusalem.");
            if ((station.Location.Longitude > 35.2642 || station.Location.Longitude < 35.1252))
                throw new InvalidNumberException($"Latitude {station.Location.Longitude} is not in Jerusalem.");
            if (station.AvailableChargeSlots < 0)
                throw new InvalidNumberException($"Cannot have negative number of charging slots.");

            DO.Station temp = new DO.Station()
            {
                Id = station.Id,
                Name = station.Name,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                ChargeSlots = station.AvailableChargeSlots,
                Active = true
            };

            try
            {
                lock (Data)
                {
                    Data.AddStation(temp);
                }
            }
            catch (DO.DoubleIDException)
            {
                throw new DoubleIDException($"Station {station.Id} already exists.");
            }
        }
        #endregion

        #region Get station
        /// <summary>
        /// Returns a station according to ID
        /// </summary>
        /// <param name="id">The ID of the station</param>
        /// <returns>The object of the station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            lock (Data)
            {
                try
                {

                    DO.Station temp = Data.GetStation(id); //getting the station from the data layer

                    Station s = new Station() //copying the dal station to bll station
                    {
                        Id = temp.Id,
                        Name = temp.Name,
                        Location = new Location() { Longitude = temp.Longitude, Latitude = temp.Latitude },
                        AvailableChargeSlots = temp.ChargeSlots,
                        ChargingDrones = (from dc in Data.GetDroneChargeList(dc => dc.StationId == temp.Id)
                                          select new ChargingDrone
                                          {
                                              Id = dc.DroneId,
                                              Battery = Drones.First(d => d.Id == dc.DroneId).Battery
                                          }).ToList(),
                        Active = temp.Active
                    };

                    return s;
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get the station according to location
        /// </summary>
        /// <param name="loc">The location of the station</param>
        /// <returns>The station</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private Station getStationByLocation(Location loc)
        {
            lock (Data)
            {
                IEnumerable<DO.Station> stations = Data.GetStationList(); //get the list of all the stations

                foreach (DO.Station s in stations)
                {
                    if (s.Longitude == loc.Longitude && s.Latitude == loc.Latitude) //if it is in the same location
                    {
                        Station temp = new Station() //copying the dal station to bll station
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Location = new Location() { Longitude = s.Longitude, Latitude = s.Latitude },
                            AvailableChargeSlots = s.ChargeSlots
                        };

                        Drones.ForEach(d =>
                        {
                            if (d.Status == DroneStatuses.Maintenance) //if the drone is charging
                            {
                                if (d.CurrentLocation == temp.Location) //if it is in the station
                                {
                                    temp.ChargingDrones.Add(new ChargingDrone //adding the drone to the list of charging drones in the station
                                    {
                                        Id = d.Id,
                                        Battery = d.Battery
                                    });
                                }
                            }
                        });

                        return temp;
                    }
                }

                throw new NoIDException($"Station in location {loc} does not exist.");
            }
        }
        #endregion

        #region Get station list
        /// <summary>
        /// Returns the list of stations for a list
        /// </summary>
        /// <returns>The list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ListStation> GetStationList()
        {
            lock (Data)
            {
                return from s in Data.GetStationList()
                       select new ListStation()
                       {
                           Id = s.Id,
                           Name = s.Name,
                           AvailableChargeSlots = s.ChargeSlots,
                           UnavailableChargeSlots = Data.GetDroneChargeList(dc => dc.StationId == s.Id).Count()
                       };
            }
        }

        /// <summary>
        /// Returns the list of the station names
        /// </summary>
        /// <returns>List of strings</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<string> GetStationNameList()
        {
            lock (Data)
            {
                return from s in Data.GetStationList()
                       select s.Name;
            }
        }

        /// <summary>
        /// Returns list of stations with available charge slots
        /// </summary>
        /// <returns>The list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ListStation> GetAvailableChargeSlotsStationList()
        {
            lock (Data)
            {
                return from s in Data.GetStationList(st => st.ChargeSlots > 0)
                       select new ListStation()
                       {
                           Id = s.Id,
                           Name = s.Name,
                           AvailableChargeSlots = s.ChargeSlots,
                           UnavailableChargeSlots = GetStation(s.Id).ChargingDrones.Count
                       };
            }
        }

        /// <summary>
        /// Returns a list of Station type stations, not including the list of drones
        /// </summary>
        /// <returns>The list</returns>

        private IEnumerable<Station> getListOfStations()
        {
            lock (Data)
            {
                return from s in Data.GetStationList()
                       select new Station()
                       {
                           Id = s.Id,
                           Name = s.Name,
                           Location = new Location() { Longitude = s.Longitude, Latitude = s.Latitude },
                           AvailableChargeSlots = s.ChargeSlots,
                           Active = s.Active
                       };
            }
        }

        /// <summary>
        /// Returns the list of stations with available charge slots Station type
        /// </summary>
        /// <returns>The list of stations</returns>

        internal IEnumerable<Station> getListOfAvailableChargeSlotsStations()
        {
            lock (Data)
            {
                return from s in Data.GetStationList(st => st.ChargeSlots > 0)
                       select new Station()
                       {
                           Id = s.Id,
                           Name = s.Name,
                           Location = new Location() { Longitude = s.Longitude, Latitude = s.Latitude },
                           AvailableChargeSlots = s.ChargeSlots,
                           Active = s.Active
                       };
            }
        }
        #endregion

        #region Update station
        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="station">The updated station</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            DO.Station s = new DO.Station()
            {
                Id = station.Id,
                Name = station.Name,
                ChargeSlots = station.AvailableChargeSlots,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                Active = station.Active
            };
            lock (Data)
            {
                Data.UpdateStation(s);
            }
        }

        /// <summary>
        /// Update the name of the station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="name">The new name</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(int id, string name)
        {
            lock (Data)
            {
                try
                {
                    Data.EditStationName(id, name);
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Update the number of charge slots in a station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="num">The number to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationChargingSlots(int id, int number)
        {
            lock (Data)
            {
                DO.Station s;
                try
                {
                    s = Data.GetStation(id); //trying to get the station, will throw an exception if the station id doesnt exist
                }
                catch (DO.NoIDException ex)
                {
                    throw new NoIDException(ex.Message);
                }
                if (!s.Active)
                    throw new NoIDException($"Station {id} is no longer active.");

                if (number < 0)
                    throw new InvalidNumberException($"Cannot have a negative number of charge slots.");

                Location loc = new Location() { Latitude = s.Latitude, Longitude = s.Longitude };
                //counts how many drones are charging in the station
                int count = (from d in Drones
                             where d.Status == DroneStatuses.Maintenance
                             select d).Count();

                if (count > number)//if there are more drones in the station than the new number
                {
                    throw new InvalidNumberException("Too many drones are in the station.");
                }

                s.ChargeSlots = number; //num will be the number of available charging slots
                Data.UpdateStation(s); //send the updates station to the data layer
            }
        }
        #endregion

        #region Remove station
        /// <summary>
        /// Remove a station from the list
        /// </summary>
        /// <param name="station">The station to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveStation(int id)
        {
            lock (Data)
            {
                DO.Station temp = Data.GetStation(id);

                try
                {
                    Data.RemoveStation(temp);
                }
                catch (DO.NoIDException)
                {
                    throw new NoIDException($"Station {id} does not exist.");
                }
            }
        }
        #endregion
    }
}
