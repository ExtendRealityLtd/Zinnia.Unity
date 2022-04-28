namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Retrieves the velocity for a <see cref="Rigidbody"/>.
    /// </summary>
    public class RigidbodyVelocityTracker : VelocityTracker
    {
        [Tooltip("The source to track and estimate velocities for.")]
        [SerializeField]
        private Rigidbody source;
        /// <summary>
        /// The source to track and estimate velocities for.
        /// </summary>
        public Rigidbody Source
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

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Source != null && Source.gameObject.activeInHierarchy;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return Source != null ? Source.angularVelocity : Vector3.zero;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return Source != null ? Source.velocity : Vector3.zero;
        }
    }
}