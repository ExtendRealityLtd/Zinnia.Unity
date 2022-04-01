namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Provides the velocity as set on its properties.
    /// </summary>
    public class ConstantVelocityTracker : VelocityTracker
    {
        [Tooltip("The velocity to return.")]
        [SerializeField]
        private Vector3 velocity;
        /// <summary>
        /// The velocity to return.
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        [Tooltip("The angular velocity to return.")]
        [SerializeField]
        private Vector3 angularVelocity;
        /// <summary>
        /// The angular velocity to return.
        /// </summary>
        public Vector3 AngularVelocity
        {
            get
            {
                return angularVelocity;
            }
            set
            {
                angularVelocity = value;
            }
        }

        [Tooltip("Determines whether to extract the local property or the world property.")]
        [SerializeField]
        private bool useLocal;
        /// <summary>
        /// Determines whether to extract the local property or the world property.
        /// </summary>
        public bool UseLocal
        {
            get
            {
                return useLocal;
            }
            set
            {
                useLocal = value;
            }
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return UseLocal ? transform.localRotation * AngularVelocity : AngularVelocity;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return UseLocal ? transform.localRotation * Velocity : Velocity;
        }
    }
}