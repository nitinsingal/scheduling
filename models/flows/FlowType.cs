namespace Scheduling.Models
{
    /// <summary>
    /// Represents the type of flow: Produce or Consume.
    /// </summary>
    public enum FlowType
    {
        /// <summary>
        /// Produces/creates inventory
        /// </summary>
        Produce,

        /// <summary>
        /// Consumes/uses inventory
        /// </summary>
        Consume,
        /// <summary>
        /// Consumes capacity from a Resource
        /// </summary>
        ConsumeCapacity
    }
}

