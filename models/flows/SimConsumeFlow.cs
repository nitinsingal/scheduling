using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents a simultaneous consume flow - multiple flows that are consumed together.
    /// </summary>
    public class SimConsumeFlow : IFlowType
    {
        private readonly List<Flow> _flows;

        /// <summary>
        /// Creates a new SimConsumeFlow with the specified flows.
        /// </summary>
        /// <param name="flows">The flows that are consumed simultaneously</param>
        /// <exception cref="ArgumentNullException">Thrown if flows is null</exception>
        /// <exception cref="ArgumentException">Thrown if flows is empty</exception>
        public SimConsumeFlow(IEnumerable<Flow> flows)
        {
            if (flows == null)
            {
                throw new ArgumentNullException(nameof(flows));
            }

            _flows = new List<Flow>(flows);

            if (_flows.Count == 0)
            {
                throw new ArgumentException("SimConsumeFlow must contain at least one flow.", nameof(flows));
            }
        }

        /// <summary>
        /// Creates a new empty SimConsumeFlow. Use AddFlow to add flows.
        /// </summary>
        public SimConsumeFlow()
        {
            _flows = new List<Flow>(2);
        }

        /// <summary>
        /// Adds a flow to this SimConsumeFlow.
        /// </summary>
        /// <param name="productLocation">The product location to consume</param>
        /// <param name="quantityPer">The quantity per unit</param>
        /// <exception cref="ArgumentNullException">Thrown if productLocation is null</exception>
        /// <exception cref="ArgumentException">Thrown if quantityPer is not positive</exception>
        /// <returns>True if the flow was added, false if the ProductLocation already exists (warning logged)</returns>
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

            // Check if ProductLocation already exists
            if (_flows.Any(f => f.ProductLocation == productLocation))
            {
                Console.WriteLine($"WARNING: ProductLocation '{productLocation.Key}' already exists in SimConsumeFlow. Skipping AddFlow.");
                return false;
            }

            // Add the new flow
            _flows.Add(new Flow(productLocation, quantityPer));
            return true;
        }

        /// <summary>
        /// Gets the flows in this SimConsumeFlow.
        /// </summary>
        public IReadOnlyList<Flow> Flows => _flows;

        /// <summary>
        /// Gets the type of flow (always Consume).
        /// </summary>
        public FlowType FlowType => FlowType.Consume;

        /// <summary>
        /// Gets the count of flows in this SimConsumeFlow.
        /// </summary>
        public int Count => _flows.Count;
    }
}

