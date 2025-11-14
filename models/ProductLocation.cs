using System;
using System.Collections.Generic;

namespace Scheduling.Models
{
    /// <summary>
    /// Manages product-location relationships.
    /// Keyed as "product@location". Automatically creates product and location if they don't exist.
    /// </summary>
    public class ProductLocation
    {
        private static readonly Dictionary<string, ProductLocation> _productLocations = new Dictionary<string, ProductLocation>();
        private readonly string _key;
        private readonly Product _product;
        private readonly Location _location;
        private readonly InventoryProfile _inventoryProfile;
        private IOperationType? _producingOperation;

        private ProductLocation(string productName, string locationName)
        {
            // Create product and location if they don't exist
            _product = Product.Create(productName);
            _location = Location.Create(locationName);
            _key = $"{productName}@{locationName}";
            _inventoryProfile = new InventoryProfile();
            _producingOperation = null;
        }

        public string Key => _key;
        public Product Product => _product;
        public Location Location => _location;
        public string ProductName => _product.Name;
        public string LocationName => _location.Name;
        public InventoryProfile InventoryProfile => _inventoryProfile;
        public IOperationType? ProducingOperation => _producingOperation;

        /// <summary>
        /// Creates or gets a ProductLocation. Automatically creates the product and location if they don't exist.
        /// </summary>
        /// <param name="productName">The name of the product</param>
        /// <param name="locationName">The name of the location</param>
        /// <returns>The created or existing ProductLocation</returns>
        public static ProductLocation Create(string productName, string locationName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
            }

            if (string.IsNullOrWhiteSpace(locationName))
            {
                throw new ArgumentException("Location name cannot be null or empty.", nameof(locationName));
            }

            var key = $"{productName}@{locationName}";

            if (!_productLocations.ContainsKey(key))
            {
                _productLocations[key] = new ProductLocation(productName, locationName);
            }

            return _productLocations[key];
        }

        /// <summary>
        /// Creates or gets a ProductLocation using Product and Location objects.
        /// </summary>
        /// <param name="product">The Product object</param>
        /// <param name="location">The Location object</param>
        /// <returns>The created or existing ProductLocation</returns>
        public static ProductLocation Create(Product product, Location location)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            var key = $"{product.Name}@{location.Name}";

            if (!_productLocations.ContainsKey(key))
            {
                _productLocations[key] = new ProductLocation(product.Name, location.Name);
            }

            return _productLocations[key];
        }

        /// <summary>
        /// Removes a ProductLocation if it exists.
        /// </summary>
        /// <param name="productName">The name of the product</param>
        /// <param name="locationName">The name of the location</param>
        /// <returns>True if the ProductLocation was removed, false if it didn't exist</returns>
        public static bool Remove(string productName, string locationName)
        {
            var key = $"{productName}@{locationName}";
            return _productLocations.Remove(key);
        }

        /// <summary>
        /// Gets a ProductLocation by product and location names if it exists.
        /// </summary>
        /// <param name="productName">The name of the product</param>
        /// <param name="locationName">The name of the location</param>
        /// <returns>The ProductLocation if found, null otherwise</returns>
        public static ProductLocation? Get(string productName, string locationName)
        {
            var key = $"{productName}@{locationName}";
            _productLocations.TryGetValue(key, out var productLocation);
            return productLocation;
        }

        /// <summary>
        /// Gets a ProductLocation by key if it exists.
        /// </summary>
        /// <param name="key">The key in format "product@location"</param>
        /// <returns>The ProductLocation if found, null otherwise</returns>
        public static ProductLocation? GetByKey(string key)
        {
            _productLocations.TryGetValue(key, out var productLocation);
            return productLocation;
        }

        /// <summary>
        /// Checks if a ProductLocation exists.
        /// </summary>
        /// <param name="productName">The name of the product</param>
        /// <param name="locationName">The name of the location</param>
        /// <returns>True if the ProductLocation exists, false otherwise</returns>
        public static bool Exists(string productName, string locationName)
        {
            var key = $"{productName}@{locationName}";
            return _productLocations.ContainsKey(key);
        }

        /// <summary>
        /// Gets all ProductLocations.
        /// </summary>
        /// <returns>A collection of all ProductLocations</returns>
        public static IEnumerable<ProductLocation> GetAll()
        {
            return _productLocations.Values;
        }

        /// <summary>
        /// Gets all ProductLocations for a specific product.
        /// </summary>
        /// <param name="productName">The name of the product</param>
        /// <returns>A collection of ProductLocations for the product</returns>
        public static IEnumerable<ProductLocation> GetByProduct(string productName)
        {
            foreach (var productLocation in _productLocations.Values)
            {
                if (productLocation.ProductName == productName)
                {
                    yield return productLocation;
                }
            }
        }

        /// <summary>
        /// Gets all ProductLocations for a specific location.
        /// </summary>
        /// <param name="locationName">The name of the location</param>
        /// <returns>A collection of ProductLocations for the location</returns>
        public static IEnumerable<ProductLocation> GetByLocation(string locationName)
        {
            foreach (var productLocation in _productLocations.Values)
            {
                if (productLocation.LocationName == locationName)
                {
                    yield return productLocation;
                }
            }
        }

        // Inventory Management Methods

        /// <summary>
        /// Adds inventory at a specific time.
        /// </summary>
        /// <param name="time">The time when inventory is added</param>
        /// <param name="quantity">The quantity to add</param>
        public void AddInventory(DateTime time, double quantity)
        {
            _inventoryProfile.AddInventory(time, quantity);
        }

        /// <summary>
        /// Removes inventory at a specific time.
        /// </summary>
        /// <param name="time">The time when inventory is removed</param>
        /// <param name="quantity">The quantity to remove</param>
        public void RemoveInventory(DateTime time, double quantity)
        {
            _inventoryProfile.RemoveInventory(time, quantity);
        }

        /// <summary>
        /// Updates inventory change at a specific time.
        /// </summary>
        /// <param name="time">The time of the inventory change</param>
        /// <param name="netChange">The net change in inventory</param>
        public void UpdateInventory(DateTime time, double netChange)
        {
            _inventoryProfile.UpdateInventory(time, netChange);
        }

        /// <summary>
        /// Gets the cumulative inventory level at a specific time.
        /// </summary>
        /// <param name="time">The time to query</param>
        /// <returns>The cumulative inventory level</returns>
        public double GetCumulativeInventory(DateTime time)
        {
            return _inventoryProfile.GetCumulativeInventory(time);
        }

        /// <summary>
        /// Gets the inventory change at a specific time.
        /// </summary>
        /// <param name="time">The time to query</param>
        /// <returns>The net change at that time</returns>
        public double GetInventoryChangeAtTime(DateTime time)
        {
            return _inventoryProfile.GetInventoryChangeAtTime(time);
        }

        /// <summary>
        /// Gets all inventory changes for this product location.
        /// </summary>
        /// <returns>List of all inventory changes</returns>
        public List<(DateTime Time, double NetChange)> GetAllInventoryChanges()
        {
            return _inventoryProfile.GetAllChanges();
        }

        /// <summary>
        /// Gets inventory changes within a time range.
        /// </summary>
        /// <param name="startTime">Start time (inclusive)</param>
        /// <param name="endTime">End time (inclusive)</param>
        /// <returns>List of inventory changes in the range</returns>
        public List<(DateTime Time, double NetChange)> GetInventoryChangesInRange(DateTime startTime, DateTime endTime)
        {
            return _inventoryProfile.GetChangesInRange(startTime, endTime);
        }

        /// <summary>
        /// Sets the producing operation for this product location.
        /// </summary>
        /// <param name="operation">The operation that produces this product at this location, or null to clear</param>
        public void SetProducingOperation(IOperationType? operation)
        {
            _producingOperation = operation;
        }

        /// <summary>
        /// Clears all product locations. Useful for testing.
        /// </summary>
        public static void Clear()
        {
            _productLocations.Clear();
        }
    }
}

