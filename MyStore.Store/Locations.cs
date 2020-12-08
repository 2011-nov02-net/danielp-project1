using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    //singleton
    /// <summary>
    /// This is a contianer to accsess any location in the model
    /// </summary>
    /// <remarks>
    /// This is a singleton class
    /// </remarks>
    public class Locations
    {
        private Dictionary<string, Location> _stores;

        /// <summary>
        /// All the stores currently in the model.
        /// </summary>
        public IReadOnlyCollection<Location> Stores 
        {
            get
            {
                return _stores.Values;
            }
        }


        #region Singleton
        /// <summary>
        /// Accsess the Locations list.
        /// </summary>
        /// <remarks>
        /// Used because this is a singleton class.
        /// </remarks>
        public static Locations Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new Locations();
                }
                return _instance;
            }
        }

        private static Locations _instance;

        /// <summary>
        /// create a new instance of the Locations object
        /// </summary>
        private Locations()
        {
            _stores = new Dictionary<string, Location>();
            _instance = this;
        }
        #endregion


        /// <summary>
        /// Create a location and add it to the list of known locations.
        /// </summary>
        /// <param name="newLocation">The name of a new store location</param>
        /// <returns>The same location passed in, or throws an exception</returns>
        /// <remarks>
        /// This is intended for external use, to create a location, and return a reference too it to 
        /// fill out the stock of the location. 
        /// </remarks>
        public Location RegisterLocation(String locatedAt)
        {
            Location newlocation;
            if (_stores.ContainsKey(locatedAt))
            {
                throw new ArgumentException("Location Already Registered");
            }
            else
            {
                newlocation = new Location(locatedAt);
                _stores.Add(locatedAt, newlocation);
            }

            return newlocation;
        }


        /// <summary>
        /// Add a newly created location to the list of known locations.
        /// </summary>
        /// <param name="newLocation">A Location object that's been created</param>
        /// <returns>The same location passed in, or throws an exception</returns>
        /// <remarks>
        /// This is intended for internal use mostly, since only internal functions can create Locations. 
        /// </remarks>
        public Location RegisterLocation(Location newLocation)
        {
            if(_stores.ContainsKey(newLocation.LocationName))
            {
                throw new ArgumentException("Location Already Registered");
            } else
            {
                _stores.Add(newLocation.LocationName, newLocation);
            }

            return newLocation;
        }



        /// <summary>
        /// Get the Location corasponding to the name
        /// </summary>
        /// <param name="where">the name of the location</param>
        /// <returns>The location from the list of locations with the given key.</returns>
        public Location GetLocation(string where)
        {
            Location loc;
            if( _stores.TryGetValue(where, out loc))
            {
                return loc;
            } else
            {
                throw new LocationNotFoundException($"Error: Location, {where} not found.");
            }          
        }

        /// <summary>
        /// Get the location or register a new one at that location.
        /// </summary>
        /// <param name="where">The name of the location.</param>
        /// <returns>A new or existing Location object.</returns>
        public Location GetOrRegisterLocation(string where)
        {
            Location loc;
            if (_stores.TryGetValue(where, out loc))
            {
                return loc;
            }
            else
            {
                return RegisterLocation(where);
            }
        }

        /// <summary>
        /// Checks if the location is known to the model.
        /// </summary>
        /// <param name="storeName">The name of the location</param>
        /// <returns>True if that location is in the model currently.</returns>
        public bool HasLocation(string storeName)
        {
            return _stores.ContainsKey(storeName);
        }
    }
}
