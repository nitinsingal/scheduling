using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents an alternate operation - a collection of operations with priorities that can produce the same product.
    /// </summary>
    public class AlternateOperation : IOperationType
    {
        private readonly List<OperationWithPriority> _operations;

        /// <summary>
        /// Creates a new AlternateOperation with the specified operations and priorities.
        /// </summary>
        /// <param name="operations">The operations with their priorities</param>
        /// <exception cref="ArgumentNullException">Thrown if operations is null</exception>
        /// <exception cref="ArgumentException">Thrown if operations is empty</exception>
        public AlternateOperation(IEnumerable<OperationWithPriority> operations)
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            _operations = new List<OperationWithPriority>(operations);

            if (_operations.Count == 0)
            {
                throw new ArgumentException("AlternateOperation must contain at least one operation.", nameof(operations));
            }
        }

        /// <summary>
        /// Gets the operations with their priorities in this AlternateOperation.
        /// </summary>
        public IReadOnlyList<OperationWithPriority> Operations => _operations;

        /// <summary>
        /// Gets the type of operation (always Alternate).
        /// </summary>
        /// <returns>The operation type</returns>
        public OperationType GetOperationType()
        {
            return OperationType.Alternate;
        }

        /// <summary>
        /// Gets the count of operations in this AlternateOperation.
        /// </summary>
        public int Count => _operations.Count;

        /// <summary>
        /// Gets operations sorted by priority (lower priority number = higher priority).
        /// </summary>
        /// <returns>Operations sorted by priority</returns>
        public IEnumerable<OperationWithPriority> GetOperationsByPriority()
        {
            return _operations.OrderBy(op => op.Priority);
        }
    }
}

