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
        internal static double Deg2rad(double deg) => deg * (Math.PI / 180);
        /// <summary>
        /// Calculates the distance between two locations
        /// </summary>
        /// <param name="loc1">The first location</param>
        /// <param name="loc2">The second location</param>
        /// <returns>The distance</returns>
        internal static double getDistance(Location loc1, Location loc2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = Deg2rad(loc2.Latitude - loc1.Latitude); // deg2rad below
            var dLon = Deg2rad(loc2.Longitude - loc2.Latitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(Deg2rad(loc1.Latitude)) * Math.Cos(Deg2rad(loc2.Latitude)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d / 100;
        }

        /// <summary>
        /// finds the nearest station to a given location
        /// </summary>
        /// <param name="loc">the given location</param>
        /// <returns>the id of the nearest station</returns>
        private int nearestStationId(Location loc)
        {
            double min = 100000; //no two places in Jerusalem have a greater distance (our company is placed in Jerusalem)
            int minId = 0;

            IEnumerable<Station> stations = getListOfStations(); //get the list of all the stations

            foreach (Station s in stations)
            {
                //if the distance between the location and the nearest station is smaller than the current minimum, so this is the minimum
                if (getDistance(loc, s.Location) < min)
                {
                    min = getDistance(loc, s.Location);
                    minId = s.Id; //the id of the nearest station will be saved here
                }
            }

            return minId;
        }

        /// <summary>
        /// Finds the location of the nearest station to given location
        /// </summary>
        /// <param name="loc">The location</param>
        /// <returns>The location of the station</returns>
        private Location nearestStationLocation(Location loc)
        {
            return GetStation(nearestStationId(loc)).Location;
        }

        /// <summary>
        /// Returns the distance a drone needs to make in order to make a delivery
        /// </summary>
        /// <param name="droneId">The ID of the drone</param>
        /// <param name="senderId">The ID of the sender</param>
        /// <param name="targetId">The ID of the target</param>
        /// <returns>The distance</returns>
        private double distanceForDelivery(int droneId, int senderId, int targetId)
        {
            Drone drone = GetDrone(droneId); //getting the drone from the list
            Customer sender = GetCustomer(senderId); //getting the sender
            Customer target = GetCustomer(targetId); //getting the target

            return getDistance(drone.CurrentLocation, sender.Location) + 
                getDistance(sender.Location, target.Location) + 
                getDistance(target.Location, nearestStationLocation(target.Location));
        }

        /// <summary>
        /// Finds the closest parcel to a drone
        /// </summary>
        /// <param name="droneId">The ID of the drone</param>
        /// <param name="parcels">The list of parcels to search in</param>
        /// <returns>The closest parcel</returns>
        private ParcelInShipping closestParcel(int droneId, List<ParcelInShipping> parcels)
        {
            Drone drone = GetDrone(droneId);
            double min = 100000; //no two places in Jerusalem hava a greater distance (our company is placed in Jerusalem)
            ParcelInShipping parcel = new ParcelInShipping();

            parcels.ForEach(p =>
            {
                if (getDistance(drone.CurrentLocation, p.DeliveryLocation) < min)
                {
                    min = getDistance(drone.CurrentLocation, p.DeliveryLocation);
                    parcel = p;
                }
            });

            return parcel;
        }
    }
}
