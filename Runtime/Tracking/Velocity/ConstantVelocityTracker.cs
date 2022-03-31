namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;

    /// <summary>
    /// Provides the velocity as set on its properties.
    /// </summary>
    public class ConstantVelocityTracker : VelocityTracker
    {
        /// <summary>
        /// The velocity to return.
        /// </summary>
        [Tooltip("The velocity to return.")]
        [SerializeField]
        private Vector3 _velocity;
        public Vector3 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        /// <summary>
        /// The angular velocity to return.
        /// </summary>
        [Tooltip("The angular velocity to return.")]
        [SerializeField]
        private Vector3 _angularVelocity;
        public Vector3 AngularVelocity
        {
            get
            {
                return _angularVelocity;
            }
            set
            {
                _angularVelocity = value;
            }
        }

        /// <summary>
        /// Determines whether to extract the local property or the world property.
        /// </summary>
        [Tooltip("Determines whether to extract the local property or the world property.")]
        [SerializeField]
        private bool _useLocal;
        public bool UseLocal
        {
            get
            {
                return _useLocal;
            }
            set
            {
                _useLocal = value;
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