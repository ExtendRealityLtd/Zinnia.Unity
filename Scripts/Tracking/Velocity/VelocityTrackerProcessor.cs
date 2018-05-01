namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// The VelocityTrackerProcessor serves as a proxy for reporting on velocity data on the first active VelocityTracker that is provided in the array.
    /// </summary>
    public class VelocityTrackerProcessor : VelocityTracker
    {
        public VelocityTracker[] velocityTrackers = new VelocityTracker[0];
        protected VelocityTracker cachedTracker;

        /// <summary>
        /// The IsActive method returns the state of whether the component is active.
        /// </summary>
        /// <returns>Returns `true` if the component is considered active.</returns>
        public override bool IsActive()
        {
            return isActiveAndEnabled;
        }

        /// <summary>
        /// The GetVelocity method returns the reported velocity on the first active VelocityTracker.
        /// </summary>
        /// <returns>A Vector3 of the current velocity.</returns>
        public override Vector3 GetVelocity()
        {
            Vector3 currentVelocity = Vector3.zero;
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
        /// The GetAngularVelocity method returns the reported angular velocity on the first active VelocityTracker.
        /// </summary>
        /// <returns>A Vector3 of the current angular velocity.</returns>
        public override Vector3 GetAngularVelocity()
        {
            Vector3 currentAngularVelocity = Vector3.zero;
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
        /// The GetActiveVelocityTracker returns the current active Velocity Tracker that is reporting velocities.
        /// </summary>
        /// <returns>A VelocityTracker that is currently active.</returns>
        public virtual VelocityTracker GetActiveVelocityTracker()
        {
            return (cachedTracker.IsActive() ? cachedTracker : null);
        }
    }
}