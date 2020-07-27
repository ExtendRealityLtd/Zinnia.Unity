namespace Zinnia.Tracking.Velocity
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;

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
        /// An optional object to consider the source relative to when estimating the velocities.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject RelativeTo { get; set; }

        /// <summary>
        /// A collection of node states.
        /// </summary>
        protected readonly List<XRNodeState> nodesStates = new List<XRNodeState>();

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            GetNodeState().TryGetVelocity(out Vector3 result);
            result = (RelativeTo != null ? RelativeTo.transform.rotation : Quaternion.identity) * result;
            return result;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            GetNodeState().TryGetAngularVelocity(out Vector3 result);
            GetNodeState().TryGetRotation(out Quaternion rotation);
            result = (RelativeTo != null ? RelativeTo.transform.rotation : Quaternion.identity) * rotation * result;
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