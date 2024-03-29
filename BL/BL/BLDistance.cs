﻿using System;
using System.Collections.Generic;
using System.Linq;
using BO;
namespace BL
{
    partial class BL
    {
        #region Distance between two locations
        /// <summary>
        /// Calculates the distance between two locations
        /// </summary>
        /// <param name="loc1">The first location</param>
        /// <param name="loc2">The second location</param>
        /// <returns>The distance</returns>
        internal static double getDistance(Location loc1, Location loc2)
        {
            var d1 = loc1.Latitude * (Math.PI / 180.0);
            var num1 = loc1.Longitude * (Math.PI / 180.0);
            var d2 = loc2.Latitude * (Math.PI / 180.0);
            var num2 = loc2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
        }
        #endregion

        #region Nearest station to a location in ID
        /// <summary>
        /// finds the nearest station to a given location
        /// </summary>
        /// <param name="loc">the given location</param>
        /// <returns>the id of the nearest station</returns>
        private int nearestStationId(Location loc)
        {
            IEnumerable<Station> stations = from s in getListOfStations()
                                            orderby getDistance(loc, s.Location)
                                            select s; //get the list of all the stations, orderes according to the distance of location

            return stations.First().Id;
        }
        #endregion

        #region Nearest station to a location in Location
        /// <summary>
        /// Finds the location of the nearest station to given location
        /// </summary>
        /// <param name="loc">The location</param>
        /// <returns>The location of the station</returns>
        private Location nearestStationLocation(Location loc)
        {
            return GetStation(nearestStationId(loc)).Location;
        }
        #endregion

        #region Delivery distance for drone
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
        #endregion

        #region Nearest parcel to a drone
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
        #endregion
    }
}
