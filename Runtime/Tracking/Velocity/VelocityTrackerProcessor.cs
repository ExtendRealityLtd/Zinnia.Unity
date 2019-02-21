namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// A proxy for reporting on velocity data on the first active <see cref="VelocityTracker"/> that is provided in the collection.
    /// </summary>
    public class VelocityTrackerProcessor : VelocityTracker
    {
        /// <summary>
        /// Process the first active <see cref="VelocityTracker"/> found in the collection.
        /// </summary>
        [DocumentedByXml]
        public List<VelocityTracker> velocityTrackers = new List<VelocityTracker>();

        private VelocityTracker _activeVelocityTracker;
        /// <summary>
        /// The current active <see cref="VelocityTracker"/> that is reporting velocities.
        /// </summary>
        public VelocityTracker ActiveVelocityTracker
        {
            get => _activeVelocityTracker != null && _activeVelocityTracker.IsActive() ? _activeVelocityTracker : null;
            protected set
            {
                _activeVelocityTracker = value;
            }
        }

        /// <summary>
        /// The reported velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current velocity.</returns>
        protected override Vector3 DoGetVelocity()
        {
            SetActiveVelocityTracker();
            return (ActiveVelocityTracker != null ? ActiveVelocityTracker.GetVelocity() : Vector3.zero);
        }

        /// <summary>
        /// The reported angular velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current angular velocity.</returns>
        protected override Vector3 DoGetAngularVelocity()
        {
            SetActiveVelocityTracker();
            return (ActiveVelocityTracker != null ? ActiveVelocityTracker.GetAngularVelocity() : Vector3.zero);
        }

        /// <summary>
        /// Sets the active <see cref="VelocityTracker"/> from the <see cref="velocityTrackers"/> collection.
        /// </summary>
        protected virtual void SetActiveVelocityTracker()
        {
            VelocityTracker firstActiveTracker = null;
            foreach (VelocityTracker tracker in velocityTrackers)
            {
                if (tracker.IsActive())
                {
                    firstActiveTracker = tracker;
                    break;
                }
            }

            ActiveVelocityTracker = firstActiveTracker;
        }
    }
}