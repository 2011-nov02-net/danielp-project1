using System;
using System.Collections.Generic;
using System.Text;

namespace MyStore.Store
{
    //singleton
    public class Locations
    {
        private Dictionary<string, Location> _stores;

        public IReadOnlyCollection<Location> Stores 
        {
            get
            {
                return _stores.Values;
            }
        }

        private static Locations _instance;

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

        private Locations()
        {
            _stores = new Dictionary<string, Location>();
            _instance = this;
        }



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
            if(_stores.ContainsKey(newLocation.Where))
            {
                throw new ArgumentException("Location Already Registered");
            } else
            {
                _stores.Add(newLocation.Where, newLocation);
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
                throw new ArgumentException("Error: location not found.");
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
    }
}
