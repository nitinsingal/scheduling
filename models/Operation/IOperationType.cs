namespace Scheduling.Models
{
    /// <summary>
    /// Interface representing an operation type.
    /// </summary>
    public interface IOperationType
    {
        /// <summary>
        /// Gets the type of operation (Basic, Routing, or Alternate).
        /// </summary>
        /// <returns>The operation type</returns>
        OperationType GetOperationType();
    }
}

