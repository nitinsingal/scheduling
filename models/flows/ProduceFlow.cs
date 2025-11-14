using System;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents a single produce flow.
    /// </summary>
    public class ProduceFlow : IFlowType
    {
        private Flow _flow;

        /// <summary>
        /// Creates a new ProduceFlow.
        /// </summary>
        /// <param name="flow">The flow to produce</param>
        public ProduceFlow(Flow flow)
        {
            _flow = flow ?? throw new ArgumentNullException(nameof(flow));
        }

        /// <summary>
        /// Creates a new empty ProduceFlow. Use AddFlow to set the flow.
        /// </summary>
        public ProduceFlow()
        {
            _flow = null!;
        }

        /// <summary>
        /// Sets or updates the flow for this ProduceFlow.
        /// </summary>
        /// <param name="productLocation">The product location to produce</param>
        /// <param name="quantityPer">The quantity per unit</param>
        /// <exception cref="ArgumentNullException">Thrown if productLocation is null</exception>
        /// <exception cref="ArgumentException">Thrown if quantityPer is not positive</exception>
        /// <exception cref="InvalidOperationException">Thrown if a different ProductLocation is specified when one already exists</exception>
        /// <returns>True if the flow was set, false if the same ProductLocation was specified again (warning logged)</returns>
        public bool AddFlow(ProductLocation productLocation, double quantityPer)
        {
            if (productLocation == null)
            {
                throw new ArgumentNullException(nameof(productLocation));
            }

            if (quantityPer <= 0)
            {
                throw new ArgumentException("Quantity per unit must be greater than zero.", nameof(quantityPer));
            }

            // Check if flow already exists
            if (_flow != null)
            {
                if (_flow.ProductLocation == productLocation)
                {
                    // Same ProductLocation - warning and return
                    Console.WriteLine($"WARNING: ProductLocation '{productLocation.Key}' already exists as output. Skipping AddFlow.");
                    return false;
                }
                else
                {
                    // Different ProductLocation - error
                    throw new InvalidOperationException(
                        $"Multiple outputs are not supported yet. ProduceFlow already produces '{_flow.ProductLocation.Key}', " +
                        $"cannot add output for '{productLocation.Key}'."
                    );
                }
            }

            // Set the new flow
            _flow = new Flow(productLocation, quantityPer);
            return true;
        }

        /// <summary>
        /// Gets the flow.
        /// </summary>
        public Flow Flow => _flow;

        /// <summary>
        /// Gets the type of flow (always Produce).
        /// </summary>
        public FlowType FlowType => FlowType.Produce;
    }
}

