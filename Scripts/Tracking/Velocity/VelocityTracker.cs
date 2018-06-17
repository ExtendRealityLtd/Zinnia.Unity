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
        public virtual Vector3 GetVelocity()
        {
            return (IsActive() ? DoGetVelocity() : Vector3.zero);
        }

        /// <summary>
        /// Gets the current angular velocity of the <see cref="source"/>.
        /// </summary>
        /// <returns>The current angular velocity of the <see cref="source"/></returns>
        public virtual Vector3 GetAngularVelocity()
        {
            return (IsActive() ? DoGetAngularVelocity() : Vector3.zero);
        }

        /// <summary>
        /// Gets the current velocity of the <see cref="source"/>.
        /// </summary>
        /// <returns>The current velocity of the <see cref="source"/></returns>
        protected abstract Vector3 DoGetVelocity();
        /// <summary>
        /// Gets the current angular velocity of the <see cref="source"/>.
        /// </summary>
        /// <returns>The current angular velocity of the <see cref="source"/></returns>
        protected abstract Vector3 DoGetAngularVelocity();
    }
}