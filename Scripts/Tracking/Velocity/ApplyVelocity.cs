namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Applies the velocity data from the given <see cref="VelocityTracker"/> to the given <see cref="Rigidbody"/>.
    /// </summary>
    public class ApplyVelocity : MonoBehaviour
    {
        /// <summary>
        /// The target <see cref="Rigidbody"/> to apply the source velocity data to.
        /// </summary>
        [Tooltip("The target Rigidbody to apply the source velocity data to.")]
        public Rigidbody target;
        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [Tooltip("The source VelocityTracker to receive the velocity data from.")]
        public VelocityTracker source;

        /// <summary>
        /// Appies the velocity data to the target <see cref="Rigidbody"/>.
        /// </summary>
        public virtual void Apply()
        {
            target.velocity = source.GetVelocity();
            target.angularVelocity = source.GetAngularVelocity();
        }
    }
}