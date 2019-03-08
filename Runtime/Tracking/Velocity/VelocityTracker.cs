namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Tracked Velocity information.
    /// </summary>
    public abstract class VelocityTracker : MonoBehaviour
    {
        /// <summary>
        /// The state of whether the <see cref="Behaviour"/> is active.
        /// </summary>
        /// <returns>Whether the <see cref="Behaviour"/> is considered active.</returns>
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }

        /// <summary>
        /// Gets the current velocity of the source.
        /// </summary>
        /// <returns>The current velocity of the source</returns>
        public virtual Vector3 GetVelocity()
        {
            return IsActive() ? DoGetVelocity() : Vector3.zero;
        }

        /// <summary>
        /// Gets the current angular velocity of the source.
        /// </summary>
        /// <returns>The current angular velocity of the source</returns>
        public virtual Vector3 GetAngularVelocity()
        {
            return (IsActive() ? DoGetAngularVelocity() : Vector3.zero);
        }

        /// <summary>
        /// Gets the current velocity of the source.
        /// </summary>
        /// <returns>The current velocity of the source</returns>
        protected abstract Vector3 DoGetVelocity();
        /// <summary>
        /// Gets the current angular velocity of the source.
        /// </summary>
        /// <returns>The current angular velocity of the source</returns>
        protected abstract Vector3 DoGetAngularVelocity();
    }
}