using System;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents a flow of a product at a location with a specific quantity per unit.
    /// </summary>
    public class Flow
    {
        private readonly ProductLocation _productLocation;
        private readonly double _quantityPer;

        /// <summary>
        /// Creates a new Flow.
        /// </summary>
        /// <param name="productLocation">The product location for this flow</param>
        /// <param name="quantityPer">The quantity per unit</param>
        public Flow(ProductLocation productLocation, double quantityPer)
        {
            _productLocation = productLocation ?? throw new ArgumentNullException(nameof(productLocation));

            if (quantityPer <= 0)
            {
                throw new ArgumentException("Quantity per unit must be greater than zero.", nameof(quantityPer));
            }

            _quantityPer = quantityPer;
        }

        /// <summary>
        /// Gets the product location for this flow.
        /// </summary>
        public ProductLocation ProductLocation => _productLocation;

        /// <summary>
        /// Gets the quantity per unit.
        /// </summary>
        public double QuantityPer => _quantityPer;
    }
}

