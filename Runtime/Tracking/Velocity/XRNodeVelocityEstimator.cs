namespace Zinnia.Tracking.Velocity
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;
    using Zinnia.Extension;

    /// <summary>
    /// Retrieves the velocity estimations for an <see cref="XRNode"/>.
    /// </summary>
    public class XRNodeVelocityEstimator : VelocityTracker
    {
        [Tooltip("The node to query velocity estimations for.")]
        [SerializeField]
        private XRNode node = XRNode.LeftHand;
        /// <summary>
        /// The node to query velocity estimations for.
        /// </summary>
        public XRNode Node
        {
            get
            {
                return node;
            }
            set
            {
                node = value;
            }
        }

        [Tooltip("An optional object to consider the source relative to when estimating the velocities.")]
        [SerializeField]
        private GameObject relativeTo;
        /// <summary>
        /// An optional object to consider the source relative to when estimating the velocities.
        /// </summary>
        public GameObject RelativeTo
        {
            get
            {
                return relativeTo;
            }
            set
            {
                relativeTo = value;
            }
        }

        /// <summary>
        /// A collection of node states.
        /// </summary>
        protected readonly List<XRNodeState> nodesStates = new List<XRNodeState>();

        /// <summary>
        /// Clears <see cref="RelativeTo"/>.
        /// </summary>
        public virtual void ClearRelativeTo()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RelativeTo = default;
        }

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