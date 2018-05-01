namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// The VelocityTracker forms the basis of any derrived class that can provide the velocity information being tracked.
    /// </summary>
    public abstract class VelocityTracker : MonoBehaviour
    {
        /// <summary>
        /// The IsActive method returns the state of whether the component is active.
        /// </summary>
        /// <returns>Returns `true` if the component is considered active.</returns>
        public abstract bool IsActive();
        /// <summary>
        /// The GetVelocity method returns the velocity being tracked.
        /// </summary>
        /// <returns>A Vector3 of the current tracked velocity.</returns>
        public abstract Vector3 GetVelocity();
        /// <summary>
        /// The GetAngularVelocityMethod returns the angular velocity being tracked.
        /// </summary>
        /// <returns>A Vector3 of the current tracked angular velocity.</returns>
        public abstract Vector3 GetAngularVelocity();
    }
}