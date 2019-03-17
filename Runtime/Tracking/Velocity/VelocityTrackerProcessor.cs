namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Tracking.Velocity.Collection;

    /// <summary>
    /// A proxy for reporting on velocity data on the first active <see cref="VelocityTracker"/> that is provided in the collection.
    /// </summary>
    public class VelocityTrackerProcessor : VelocityTracker
    {
        /// <summary>
        /// Process the first active <see cref="VelocityTracker"/> found in the collection.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public VelocityTrackerObservableList VelocityTrackers { get; set; }

        /// <summary>
        /// The backing field for holding the value of <see cref="ActiveVelocityTracker"/>.
        /// </summary>
        private VelocityTracker activeVelocityTracker;
        /// <summary>
        /// The current active <see cref="VelocityTracker"/> that is reporting velocities.
        /// </summary>
        public VelocityTracker ActiveVelocityTracker
        {
            get => activeVelocityTracker != null && activeVelocityTracker.IsActive() ? activeVelocityTracker : null;
            protected set
            {
                activeVelocityTracker = value;
            }
        }

        /// <summary>
        /// The reported velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current velocity.</returns>
        protected override Vector3 DoGetVelocity()
        {
            SetActiveVelocityTracker();
            return ActiveVelocityTracker != null ? ActiveVelocityTracker.GetVelocity() : Vector3.zero;
        }

        /// <summary>
        /// The reported angular velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current angular velocity.</returns>
        protected override Vector3 DoGetAngularVelocity()
        {
            SetActiveVelocityTracker();
            return ActiveVelocityTracker != null ? ActiveVelocityTracker.GetAngularVelocity() : Vector3.zero;
        }

        /// <summary>
        /// Sets the active <see cref="VelocityTracker"/> from the <see cref="VelocityTrackers"/> collection.
        /// </summary>
        protected virtual void SetActiveVelocityTracker()
        {
            if (VelocityTrackers == null)
            {
                ActiveVelocityTracker = null;
                return;
            }

            VelocityTracker firstActiveTracker = null;
            foreach (VelocityTracker tracker in VelocityTrackers.NonSubscribableElements)
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