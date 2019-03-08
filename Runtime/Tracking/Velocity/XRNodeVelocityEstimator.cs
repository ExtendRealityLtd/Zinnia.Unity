namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using UnityEngine.XR;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Retrieves the velocity estimations for an <see cref="XRNode"/>.
    /// </summary>
    public class XRNodeVelocityEstimator : VelocityTracker
    {
        /// <summary>
        /// The node to query velocity estimations for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public XRNode Node { get; set; } = XRNode.LeftHand;

        /// <summary>
        /// A collection of node states.
        /// </summary>
        protected readonly List<XRNodeState> nodesStates = new List<XRNodeState>();

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            GetNodeState().TryGetVelocity(out Vector3 result);
            return result;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            GetNodeState().TryGetAngularVelocity(out Vector3 result);
            return result;
        }

        /// <summary>
        /// Gets the state of <see cref="Node"/>.
        /// </summary>
        /// <returns>The associated node state.</returns>
        protected virtual XRNodeState GetNodeState()
        {
            InputTracking.GetNodeStates(nodesStates);
            foreach (XRNodeState state in nodesStates)
            {
                if (state.nodeType == Node)
                {
                    return state;
                }
            }

            return new XRNodeState();
        }
    }
}