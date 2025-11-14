using System.Collections.Generic;

namespace Scheduling.Models
{
    /// <summary>
    /// Manages locations using a hashmap (Dictionary).
    /// </summary>
    public class Location
    {
        private static readonly Dictionary<string, Location> _locations = new Dictionary<string, Location>();
        private readonly string _name;

        private Location(string name)
        {
            _name = name;
        }

        public string Name => _name;

        /// <summary>
        /// Creates a new location if it doesn't already exist.
        /// </summary>
        /// <param name="name">The name of the location</param>
        /// <returns>The created or existing location</returns>
        public static Location Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Location name cannot be null or empty.", nameof(name));
            }

            if (!_locations.ContainsKey(name))
            {
                _locations[name] = new Location(name);
            }

            return _locations[name];
        }

        /// <summary>
        /// Removes a location if it exists.
        /// </summary>
        /// <param name="name">The name of the location to remove</param>
        /// <returns>True if the location was removed, false if it didn't exist</returns>
        public static bool Remove(string name)
        {
            return _locations.Remove(name);
        }

        /// <summary>
        /// Gets a location by name if it exists.
        /// </summary>
        /// <param name="name">The name of the location</param>
        /// <returns>The location if found, null otherwise</returns>
        public static Location? Get(string name)
        {
            _locations.TryGetValue(name, out var location);
            return location;
        }

        /// <summary>
        /// Checks if a location exists.
        /// </summary>
        /// <param name="name">The name of the location</param>
        /// <returns>True if the location exists, false otherwise</returns>
        public static bool Exists(string name)
        {
            return _locations.ContainsKey(name);
        }

        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <returns>A collection of all locations</returns>
        public static IEnumerable<Location> GetAll()
        {
            return _locations.Values;
        }

        /// <summary>
        /// Clears all locations. Useful for testing.
        /// </summary>
        public static void Clear()
        {
            _locations.Clear();
        }
    }
}

