namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Applies the velocity data from the given <see cref="VelocityTracker"/> to the given <see cref="Rigidbody"/>.
    /// </summary>
    public class VelocityApplier : MonoBehaviour
    {
        [Tooltip("The source VelocityTracker  to receive the velocity data from.")]
        [SerializeField]
        private VelocityTracker source;
        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        public VelocityTracker Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("The target Rigidbody to apply the source velocity data to.")]
        [SerializeField]
        private Rigidbody target;
        /// <summary>
        /// The target <see cref="Rigidbody"/> to apply the source velocity data to.
        /// </summary>
        public Rigidbody Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Applies the velocity data to the <see cref="Target"/>.
        /// </summary>
        public virtual void Apply()
        {
            if (!this.IsValidState() || Source == null || Target == null)
            {
                return;
            }

            Target.velocity = Source.GetVelocity();
            Target.angularVelocity = Source.GetAngularVelocity();
        }
    }
}