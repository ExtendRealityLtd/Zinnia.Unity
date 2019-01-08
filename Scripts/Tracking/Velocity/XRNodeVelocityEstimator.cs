namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using UnityEngine.XR;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Retrieves the velocity estimations for an <see cref="XRNode"/>.
    /// </summary>
    public class XRNodeVelocityEstimator : VelocityTracker
    {
        /// <summary>
        /// The node to query velocity estimations for.
        /// </summary>
        [Tooltip("The node to query velocity estimations for.")]
        public XRNode node = XRNode.LeftHand;

        /// <summary>
        /// A collection of node states.
        /// </summary>
        protected readonly List<XRNodeState> nodesStates = new List<XRNodeState>();

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            Vector3 result = Vector3.zero;
            GetNodeState().TryGetVelocity(out result);
            return result;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            Vector3 result = Vector3.zero;
            GetNodeState().TryGetAngularVelocity(out result);
            return result;
        }

        /// <summary>
        /// Gets the state of <see cref="node"/>.
        /// </summary>
        /// <returns>The associated node state.</returns>
        protected virtual XRNodeState GetNodeState()
        {
            InputTracking.GetNodeStates(nodesStates);
            return nodesStates.FirstOrDefault(state => state.nodeType == node);
        }
    }
}