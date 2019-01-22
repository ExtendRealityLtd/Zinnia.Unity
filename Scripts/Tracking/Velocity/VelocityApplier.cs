namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Applies the velocity data from the given <see cref="VelocityTracker"/> to the given <see cref="Rigidbody"/>.
    /// </summary>
    public class VelocityApplier : MonoBehaviour
    {
        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [Tooltip("The source VelocityTracker to receive the velocity data from.")]
        public VelocityTracker source;
        /// <summary>
        /// The target <see cref="Rigidbody"/> to apply the source velocity data to.
        /// </summary>
        [Tooltip("The target Rigidbody to apply the source velocity data to.")]
        public Rigidbody target;

        /// <summary>
        /// Applies the velocity data to the target <see cref="Rigidbody"/>.
        /// </summary>
        public virtual void Apply()
        {
            if (source == null || target == null)
            {
                return;
            }

            target.velocity = source.GetVelocity();
            target.angularVelocity = source.GetAngularVelocity();
        }
    }
}