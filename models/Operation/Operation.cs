using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents an operation that can consume and/or produce inventory.
    /// </summary>
    public class Operation : IOperationType
    {
        private static readonly Dictionary<string, Operation> _operations = new Dictionary<string, Operation>();
        private readonly string _key;
        private readonly int _priority;
        private IFlowType? _consumeFlow;
        private IFlowType? _produceFlow;

        /// <summary>
        /// Creates a new Operation and adds it to the operations dictionary.
        /// </summary>
        /// <param name="key">Unique key for this operation</param>
        /// <param name="priority">Priority of this operation (lower number = higher priority)</param>
        /// <param name="consumeFlow">A consume flow (ConsumeFlow or SimConsumeFlow), or null if no consumption</param>
        /// <param name="produceFlow">A produce flow (ProduceFlow), or null if no production</param>
        /// <exception cref="ArgumentException">Thrown if key is null/empty, if operation with key already exists, if consume flow is not Consume type, or if produce flow is not Produce type</exception>
        public Operation(string key, int priority, IFlowType? consumeFlow = null, IFlowType? produceFlow = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Operation key cannot be null or empty.", nameof(key));
            }

            if (_operations.ContainsKey(key))
            {
                throw new ArgumentException($"Operation with key '{key}' already exists.", nameof(key));
            }

            // Validate consume flow type
            if (consumeFlow != null && consumeFlow.FlowType != FlowType.Consume)
            {
                throw new ArgumentException("consumeFlow must be of type Consume.", nameof(consumeFlow));
            }

            // Validate produce flow type
            if (produceFlow != null && produceFlow.FlowType != FlowType.Produce)
            {
                throw new ArgumentException("produceFlow must be of type Produce.", nameof(produceFlow));
            }

            _key = key;
            _priority = priority;
            _consumeFlow = consumeFlow;
            _produceFlow = produceFlow;

            // Add to dictionary
            _operations[key] = this;
        }

        /// <summary>
        /// Gets the unique key of this operation.
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Gets the priority of this operation (lower number = higher priority).
        /// </summary>
        public int Priority => _priority;

        /// <summary>
        /// Gets the consume flow, or null if not set.
        /// Can be ConsumeFlow or SimConsumeFlow.
        /// </summary>
        public IFlowType? ConsumeFlow => _consumeFlow;

        /// <summary>
        /// Gets the produce flow, or null if not set.
        /// </summary>
        public IFlowType? ProduceFlow => _produceFlow;

        /// <summary>
        /// Checks if this operation has any consumption.
        /// </summary>
        public bool HasConsumption => _consumeFlow != null;

        /// <summary>
        /// Checks if this operation has production.
        /// </summary>
        public bool HasProduction => _produceFlow != null;

        /// <summary>
        /// Checks if the consume flow is a single ConsumeFlow (not SimConsumeFlow).
        /// </summary>
        public bool IsConsumeFlowSingle => _consumeFlow is ConsumeFlow;

        /// <summary>
        /// Checks if the consume flow is a SimConsumeFlow (not single ConsumeFlow).
        /// </summary>
        public bool IsConsumeFlowSimFlow => _consumeFlow is SimConsumeFlow;

        /// <summary>
        /// Gets all consume flows (from either ConsumeFlow or SimConsumeFlow).
        /// </summary>
        /// <returns>Collection of Flow objects, or empty if no consumption</returns>
        public IEnumerable<Flow> GetConsumeFlows()
        {
            if (_consumeFlow is ConsumeFlow singleConsumeFlow)
            {
                yield return singleConsumeFlow.Flow;
            }
            else if (_consumeFlow is SimConsumeFlow simConsumeFlow)
            {
                foreach (var flow in simConsumeFlow.Flows)
                {
                    yield return flow;
                }
            }
        }

        /// <summary>
        /// Gets the produce flow as a Flow object, or null if not set.
        /// </summary>
        public Flow? GetProduceFlow()
        {
            return _produceFlow is ProduceFlow produceFlow ? produceFlow.Flow : null;
        }

        /// <summary>
        /// Gets the type of operation (always Basic for Operation class).
        /// </summary>
        /// <returns>The operation type</returns>
        public OperationType GetOperationType()
        {
            return OperationType.Basic;
        }

        /// <summary>
        /// Gets an operation by key if it exists.
        /// </summary>
        /// <param name="key">The operation key</param>
        /// <returns>The operation if found, null otherwise</returns>
        public static Operation? Get(string key)
        {
            _operations.TryGetValue(key, out var operation);
            return operation;
        }

        /// <summary>
        /// Checks if an operation exists with the given key.
        /// </summary>
        /// <param name="key">The operation key</param>
        /// <returns>True if operation exists, false otherwise</returns>
        public static bool Exists(string key)
        {
            return _operations.ContainsKey(key);
        }

        /// <summary>
        /// Gets all operations.
        /// </summary>
        /// <returns>Collection of all operations</returns>
        public static IEnumerable<Operation> GetAll()
        {
            return _operations.Values;
        }

        /// <summary>
        /// Removes an operation by key if it exists.
        /// </summary>
        /// <param name="key">The operation key</param>
        /// <returns>True if operation was removed, false if it didn't exist</returns>
        public static bool Remove(string key)
        {
            return _operations.Remove(key);
        }

        /// <summary>
        /// Clears all operations. Useful for testing.
        /// </summary>
        public static void Clear()
        {
            _operations.Clear();
        }

        /// <summary>
        /// Adds an input (consume flow) to this operation.
        /// Creates a SimConsumeFlow if not already present, or adds to existing SimConsumeFlow.
        /// </summary>
        /// <param name="productLocation">The product location to consume</param>
        /// <param name="quantityPer">The quantity per unit</param>
        public void AddInput(ProductLocation productLocation, double quantityPer)
        {
            // Create or update SimConsumeFlow
            if (_consumeFlow == null)
            {
                // No consume flow yet - create new SimConsumeFlow
                var simConsumeFlow = new SimConsumeFlow();
                simConsumeFlow.AddFlow(productLocation, quantityPer);
                _consumeFlow = simConsumeFlow;
            }
            else if (_consumeFlow is ConsumeFlow singleConsumeFlow)
            {
                // Convert single ConsumeFlow to SimConsumeFlow
                var existingFlow = singleConsumeFlow.Flow;
                var simConsumeFlow = new SimConsumeFlow(new[] { existingFlow });
                simConsumeFlow.AddFlow(productLocation, quantityPer);
                _consumeFlow = simConsumeFlow;
            }
            else if (_consumeFlow is SimConsumeFlow simConsumeFlow)
            {
                // Add to existing SimConsumeFlow
                simConsumeFlow.AddFlow(productLocation, quantityPer);
            }
        }

        /// <summary>
        /// Adds an output (produce flow) to this operation.
        /// Creates a ProduceFlow if not already present.
        /// </summary>
        /// <param name="productLocation">The product location to produce</param>
        /// <param name="quantityPer">The quantity per unit</param>
        public void AddOutput(ProductLocation productLocation, double quantityPer)
        {
            if (_produceFlow == null)
            {
                // No produce flow yet - create new ProduceFlow
                var produceFlow = new ProduceFlow();
                produceFlow.AddFlow(productLocation, quantityPer);
                _produceFlow = produceFlow;
            }
            else if (_produceFlow is ProduceFlow existingProduceFlow)
            {
                // Add to existing ProduceFlow
                existingProduceFlow.AddFlow(productLocation, quantityPer);
            }
        }

        /// <summary>
        /// Processes all operations and creates AlternateOperations for ProductLocations that have multiple producing operations.
        /// Sets the AlternateOperation as the producingOperation for those ProductLocations.
        /// </summary>
        public static void ProcessAlternateOperations()
        {
            // Group operations by the ProductLocation key (string) they produce into
            // Using string key instead of ProductLocation instance to avoid reference equality issues
            var operationsByProductLocationKey = new Dictionary<string, (ProductLocation ProductLocation, List<OperationWithPriority> Operations)>();

            // Create a snapshot of operations to avoid "Collection was modified" exception
            // if operations are added/removed during iteration
            var operationsSnapshot = _operations.Values.ToList();

            foreach (var operation in operationsSnapshot)
            {
                if (!operation.HasProduction)
                {
                    continue; // Skip operations that don't produce anything
                }

                var produceFlow = operation.GetProduceFlow();
                if (produceFlow == null)
                {
                    continue;
                }

                var productLocation = produceFlow.ProductLocation;
                var key = productLocation.Key;

                if (!operationsByProductLocationKey.ContainsKey(key))
                {
                    operationsByProductLocationKey[key] = (productLocation, new List<OperationWithPriority>());
                }

                operationsByProductLocationKey[key].Operations.Add(
                    new OperationWithPriority(operation, operation.Priority)
                );
            }

            // Create AlternateOperations for ProductLocations with multiple producing operations
            foreach (var kvp in operationsByProductLocationKey)
            {
                var productLocation = kvp.Value.ProductLocation;
                var operations = kvp.Value.Operations;

                // Use the ProductLocation instance from the flow
                // Since ProductLocation.Create() uses a singleton pattern, this should be the same instance
                // that the test is checking. However, to be safe, we'll also verify it exists in the dictionary
                // and use the dictionary instance if available (to handle cases where Clear() was called)
                var currentProductLocation = ProductLocation.GetByKey(productLocation.Key) ?? productLocation;

                if (operations.Count > 1)
                {
                    // Multiple operations produce into this ProductLocation - create AlternateOperation
                    var alternateOperation = new AlternateOperation(operations);
                    currentProductLocation.SetProducingOperation(alternateOperation);
                }
                else if (operations.Count == 1)
                {
                    // Single operation - set it directly
                    currentProductLocation.SetProducingOperation(operations[0].Operation);
                }
            }
        }
    }
}

