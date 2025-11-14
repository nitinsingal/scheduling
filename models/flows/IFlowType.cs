namespace Scheduling.Models
{
    /// <summary>
    /// Interface representing a flow type (Consume or Produce).
    /// </summary>
    public interface IFlowType
    {
        /// <summary>
        /// Gets the type of flow (Produce or Consume).
        /// </summary>
        FlowType FlowType { get; }
    }
}

