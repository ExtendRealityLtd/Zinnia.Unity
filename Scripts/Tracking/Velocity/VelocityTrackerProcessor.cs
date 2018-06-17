namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// A proxy for reporting on velocity data on the first active <see cref="VelocityTracker"/> that is provided in the collection.
    /// </summary>
    public class VelocityTrackerProcessor : VelocityTracker
    {
        /// <summary>
        /// Process the first active <see cref="VelocityTracker"/> found in the collection.
        /// </summary>
        [Tooltip("Process the first active VelocityTracker found in the array.")]
        public List<VelocityTracker> velocityTrackers = new List<VelocityTracker>();
        protected VelocityTracker cachedTracker;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return isActiveAndEnabled;
        }

        /// <summary>
        /// The current active <see cref="VelocityTracker"/> that is reporting velocities.
        /// </summary>
        /// <returns>The current active <see cref="VelocityTracker"/>.</returns>
        public virtual VelocityTracker GetActiveVelocityTracker()
        {
            return (cachedTracker != null && cachedTracker.IsActive() ? cachedTracker : null);
        }

        /// <summary>
        /// The reported velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current velocity.</returns>
        protected override Vector3 DoGetVelocity()
        {
            Vector3 currentVelocity = Vector3.zero;
            cachedTracker = null;
            foreach (VelocityTracker currentTracker in velocityTrackers.EmptyIfNull())
            {
                if (currentTracker != null && currentTracker.IsActive())
                {
                    currentVelocity = currentTracker.GetVelocity();
                    cachedTracker = currentTracker;
                    break;
                }
            }
            return currentVelocity;
        }

        /// <summary>
        /// The reported angular velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current angular velocity.</returns>
        protected override Vector3 DoGetAngularVelocity()
        {
            Vector3 currentAngularVelocity = Vector3.zero;
            cachedTracker = null;
            foreach (VelocityTracker currentTracker in velocityTrackers.EmptyIfNull())
            {
                if (currentTracker != null && currentTracker.IsActive())
                {
                    currentAngularVelocity = currentTracker.GetAngularVelocity();
                    cachedTracker = currentTracker;
                    break;
                }
            }
            return currentAngularVelocity;
        }
    }
}