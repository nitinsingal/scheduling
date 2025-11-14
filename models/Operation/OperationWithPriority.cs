namespace Scheduling.Models
{
    /// <summary>
    /// Represents an Operation with its priority.
    /// </summary>
    public struct OperationWithPriority
    {
        /// <summary>
        /// The operation.
        /// </summary>
        public Operation Operation { get; }

        /// <summary>
        /// The priority of the operation.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Creates a new OperationWithPriority.
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <param name="priority">The priority</param>
        public OperationWithPriority(Operation operation, int priority)
        {
            Operation = operation ?? throw new System.ArgumentNullException(nameof(operation));
            Priority = priority;
        }
    }
}

