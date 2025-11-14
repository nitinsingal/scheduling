using System.Collections.Generic;
using System.Diagnostics;

namespace Scheduling.Models
{
    /// <summary>
    /// Manages products using a hashmap (Dictionary).
    /// </summary>
    public class Product
    {
        private static readonly Dictionary<string, Product> _products = new Dictionary<string, Product>();
        private readonly string _name;

        private Product(string name)
        {
            _name = name;
        }

        public string Name => _name;

        /// <summary>
        /// Creates a new product if it doesn't already exist.
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <returns>The created or existing product</returns>
        public static Product Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be null or empty.", nameof(name));
            }

            if (!_products.ContainsKey(name))
            {
                _products[name] = new Product(name);
            }

            return _products[name];
        }

        /// <summary>
        /// Removes a product if it exists.
        /// </summary>
        /// <param name="name">The name of the product to remove</param>
        /// <returns>True if the product was removed, false if it didn't exist</returns>
        public static bool Remove(string name)
        {
            return _products.Remove(name);
        }

        /// <summary>
        /// Gets a product by name if it exists.
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <returns>The product if found, null otherwise</returns>
        public static Product? Get(string name)
        {
            _products.TryGetValue(name, out var product);
            return product;
        }

        /// <summary>
        /// Checks if a product exists.
        /// </summary>
        /// <param name="name">The name of the product</param>
        /// <returns>True if the product exists, false otherwise</returns>
        public static bool Exists(string name)
        {
            return _products.ContainsKey(name);
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A collection of all products</returns>
        public static IEnumerable<Product> GetAll()
        {
            return _products.Values;
        }

        /// <summary>
        /// Clears all products. Useful for testing.
        /// </summary>
        public static void Clear()
        {
            _products.Clear();
        }
    }
}

