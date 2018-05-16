namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using System;

    /// <summary>
    /// A proxy for reporting on velocity data on the first active <see cref="VelocityTracker"/> that is provided in the array.
    /// </summary>
    public class VelocityTrackerProcessor : VelocityTracker
    {
        /// <summary>
        /// Process the first active <see cref="VelocityTracker"/> found in the array.
        /// </summary>
        [Tooltip("Process the first active VelocityTracker found in the array.")]
        public VelocityTracker[] velocityTrackers = Array.Empty<VelocityTracker>();
        protected VelocityTracker cachedTracker;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return isActiveAndEnabled;
        }

        /// <summary>
        /// The reported velocity on the first active <see cref="VelocityTracker"/>.
        /// </summary>
        /// <returns>The current velocity.</returns>
        public override Vector3 GetVelocity()
        {
            Vector3 currentVelocity = Vector3.zero;
            cachedTracker = null;
            foreach (VelocityTracker currentTracker in velocityTrackers)
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
        public override Vector3 GetAngularVelocity()
        {
            Vector3 currentAngularVelocity = Vector3.zero;
            cachedTracker = null;
            foreach (VelocityTracker currentTracker in velocityTrackers)
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

        /// <summary>
        /// The current active <see cref="VelocityTracker"/> that is reporting velocities.
        /// </summary>
        /// <returns>The current active <see cref="VelocityTracker"/>.</returns>
        public virtual VelocityTracker GetActiveVelocityTracker()
        {
            return (cachedTracker != null && cachedTracker.IsActive() ? cachedTracker : null);
        }
    }
}