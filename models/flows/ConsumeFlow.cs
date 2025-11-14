using System;

namespace Scheduling.Models
{
    /// <summary>
    /// Represents a single consume flow.
    /// </summary>
    public class ConsumeFlow : IFlowType
    {
        private readonly Flow _flow;

        /// <summary>
        /// Creates a new ConsumeFlow.
        /// </summary>
        /// <param name="flow">The flow to consume</param>
        public ConsumeFlow(Flow flow)
        {
            _flow = flow ?? throw new ArgumentNullException(nameof(flow));
        }

        /// <summary>
        /// Gets the flow.
        /// </summary>
        public Flow Flow => _flow;

        /// <summary>
        /// Gets the type of flow (always Consume).
        /// </summary>
        public FlowType FlowType => FlowType.Consume;
    }
}

