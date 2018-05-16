namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Tracked Velocity information.
    /// </summary>
    public abstract class VelocityTracker : MonoBehaviour
    {
        /// <summary>
        /// The state of whether the <see cref="Component"/> is active.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="Component"/> is considered active.</returns>
        public abstract bool IsActive();
        /// <summary>
        /// Gets the current velocity of the <see cref="source"/>.
        /// </summary>
        /// <returns>The current velocity of the <see cref="source"/></returns>
        public abstract Vector3 GetVelocity();
        /// <summary>
        /// Gets the current angular velocity of the <see cref="source"/>.
        /// </summary>
        /// <returns>The current angular velocity of the <see cref="source"/></returns>
        public abstract Vector3 GetAngularVelocity();
    }
}