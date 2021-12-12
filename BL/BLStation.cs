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
        /// Adds a station to the list of station in data
        /// </summary>
        /// <param name="station">The station to add</param>
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
                ChargeSlots = station.AvailableChargeSlots
            };

            try
            {
                Data.AddStation(temp);
            }
            catch (DO.DoubleIDException)
            {
                throw new DoubleIDException($"Station {station.Id} already exists.");
            }
        }

        /// <summary>
        /// Returns a station according to ID
        /// </summary>
        /// <param name="id">The ID of the station</param>
        /// <returns>The object of the station</returns>
        public Station GetStation(int id)
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
                    ChargingDrones = new List<ChargingDrone>()
                };

                Drones.ForEach(d =>
                {
                    if (d.Status == DroneStatuses.Maintenance) //if the drone is charging
                    {
                        if (d.CurrentLocation.Longitude == s.Location.Longitude && d.CurrentLocation.Latitude == s.Location.Latitude) //if it is in the station
                        {
                            s.ChargingDrones.Add(new ChargingDrone //adding the drone to the list of charging drones in the station
                            {
                                Id = d.Id,
                                Battery = d.Battery
                            });
                        }
                    }
                });

                return s;
            }
            catch (DO.NoIDException ex)
            {
                throw new NoIDException(ex.Message);
            }
        }

        /// <summary>
        /// Get the station according to location
        /// </summary>
        /// <param name="loc">The location of the station</param>
        /// <returns>The station</returns>
        private Station getStationByLocation(Location loc)
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

        /// <summary>
        /// Returns the list of stations for a list
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetStationList()
        {
            List<ListStation> stations = new List<ListStation>();

            foreach (DO.Station s in Data.GetStationList())
            {
                stations.Add(new ListStation()
                {
                    Id = s.Id,
                    Name = s.Name,
                    AvailableChargeSlots = s.ChargeSlots,
                    UnavailableChargeSlots = GetStation(s.Id).ChargingDrones.Count
                });
            }

            if (stations.Count == 0) //if no stations were added
                throw new EmptyListException("No stations to display.");

            return stations;
        }

        /// <summary>
        /// Returns the list of the station names
        /// </summary>
        /// <returns>List of strings</returns>
        public IEnumerable<string> GetStationNameList()
        {
            List<string> stations = new List<string>();

            foreach (DO.Station s in Data.GetStationList())
            {
                stations.Add(s.Name);
            }

            if (stations.Count == 0) //if no stations were added
                throw new EmptyListException("No stations to display.");

            return stations;
        }

        /// <summary>
        /// Returns list of stations with available charge slots
        /// </summary>
        /// <returns>The list of stations</returns>
        public IEnumerable<ListStation> GetAvailableChargeSlotsStationList()
        {
            IEnumerable<DO.Station> tempStations = Data.GetStationList(s => { return s.ChargeSlots > 0; });
            if (tempStations.Count() == 0)
                throw new EmptyListException("No stations with avavilable charge slots.");

            List<ListStation> stations = new List<ListStation>();

            foreach (DO.Station s in tempStations)
            {
                stations.Add(new ListStation()
                {
                    Id = s.Id,
                    Name = s.Name,
                    AvailableChargeSlots = s.ChargeSlots,
                    UnavailableChargeSlots = GetStation(s.Id).ChargingDrones.Count
                });
            };

            return stations;
        }

        /// <summary>
        /// Returns a list of Station type stations, not including the list of drones
        /// </summary>
        /// <returns>The list</returns>
        private IEnumerable<Station> getListOfStations()
        {
            List<Station> stations = new List<Station>();

            foreach (DO.Station s in Data.GetStationList())
            {
                stations.Add(new Station()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = new Location() { Longitude = s.Longitude, Latitude = s.Latitude },
                    AvailableChargeSlots = s.ChargeSlots
                });
            }

            return stations;
        }

        /// <summary>
        /// Returns the list of stations with available charge slots Station type
        /// </summary>
        /// <returns>The list of stations</returns>
        private IEnumerable<Station> getListOfAvailableChargeSlotsStations()
        {
            IEnumerable<DO.Station> tempStations = Data.GetStationList(s => { return s.ChargeSlots > 0; });
            if (tempStations.Count() == 0)
                throw new EmptyListException("No stations with avavilable charge slots.");

            List<Station> stations = new List<Station>();
            foreach (DO.Station tmp in tempStations)
            {
                stations.Add(new Station()
                {
                    Id = tmp.Id,
                    Name = tmp.Name,
                    Location = new Location() { Longitude = tmp.Longitude, Latitude = tmp.Latitude },
                    AvailableChargeSlots = tmp.ChargeSlots
                });
            }

            return stations;
        }

        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="station">The updated station</param>
        public void UpdateStation(Station station)
        {
            DO.Station s = new DO.Station()
            {
                Id = station.Id,
                Name = station.Name,
                ChargeSlots = station.AvailableChargeSlots,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude
            };

            Data.UpdateStation(s);
        }

        /// <summary>
        /// Update the name of the station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="name">The new name</param>
        public void UpdateStationName(int id, string name)
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

        /// <summary>
        /// Update the number of charge slots in a station
        /// </summary>
        /// <param name="id">The id of the station</param>
        /// <param name="num">The number to update</param>
        public void UpdateStationChargingSlots(int id, int number)
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

            if (number < 0)
            {
                throw new InvalidNumberException($"Cannot have a negative number of charge slots.");
            }

            Location loc = new Location() { Latitude = s.Latitude, Longitude = s.Longitude };
            int count = 0; //counts how many drones are charging in the station
            Drones.ForEach(d =>
            {
                if (d.Status == DroneStatuses.Maintenance) //if the drone is charging
                    if (d.CurrentLocation == loc) //if it is in the station we want to update
                        count++; //count how many drones are charging in the station
            });

            if (count > number)//if there are more drones in the station than the new number
            {
                throw new InvalidNumberException("Too many drones are in the station.");
            }

            s.ChargeSlots = number - count; //num will be the number of available charging slots
            Data.UpdateStation(s); //send the updates station to the data layer
        }

        /// <summary>
        /// Remove a station from the list
        /// </summary>
        /// <param name="station">The station to remove</param>
        public void RemoveStation(Station station)
        {
            DO.Station temp = new DO.Station()
            {
                Id = station.Id,
                Name = station.Name,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                ChargeSlots = station.AvailableChargeSlots
            };

            try
            {
                Data.RemoveStation(temp);
            }
            catch (DO.NoIDException)
            {
                throw new NoIDException($"Station {station.Id} already exists.");
            }
        }
    }
}
