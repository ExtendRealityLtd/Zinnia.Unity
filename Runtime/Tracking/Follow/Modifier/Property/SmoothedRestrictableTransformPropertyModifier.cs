namespace Zinnia.Tracking.Follow.Modifier.Property
{
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    public abstract class SmoothedRestrictableTransformPropertyModifier : RestrictableTransformPropertyModifier
    {
        [Header("Smoothing Settings")]
        [Tooltip("The tolereance to consider the source and target property equal.")]
        [SerializeField]
        private float equalityTolerance = float.Epsilon;
        /// <summary>
        /// The tolereance to consider the source and target property equal.
        /// </summary>
        public float EqualityTolerance
        {
            get
            {
                return equalityTolerance;
            }
            set
            {
                equalityTolerance = value;
            }
        }

        [Tooltip("The approximate duration of transition for the smoothing operation.")]
        [SerializeField]
        private float transitionDuration = 0f;
        /// <summary>
        /// The approximate duration of transition for the smoothing operation.
        /// </summary>
        public float TransitionDuration
        {
            get
            {
                return transitionDuration;
            }
            set
            {
                transitionDuration = value;
            }
        }

        [Tooltip("The maximum speed the object can move between the smoothed distances.")]
        [SerializeField]
        private float maxSpeed = Mathf.Infinity;
        /// <summary>
        /// The maximum speed the object can move between the smoothed distances.
        /// </summary>
        public float MaxSpeed
        {
            get
            {
                return maxSpeed;
            }
            set
            {
                maxSpeed = value;
            }
        }

        [Header("Smoothing Events")]
        /// <summary>
        /// Emitted when the smoothing transition has completed.
        /// </summary>
        public UnityEvent Transitioned = new UnityEvent();

        /// <summary>
        /// The reference to the output velocity.
        /// </summary>
        public Vector3 Velocity => velocity;
        private Vector3 velocity;

        /// <summary>
        /// The reference to the output angulaer velocity.
        /// </summary>
        public Quaternion AngularVelocity => angularVelocity;
        private Quaternion angularVelocity;

        /// <summary>
        /// Whether the velocity <see cref="Vector3"/> is currently being smoothed.
        /// </summary>
        protected bool isVelocitySmoothing;
        /// <summary>
        /// Whether the angular velocity <see cref="Quaternion"/> is currently being smoothed.
        /// </summary>
        protected bool isAngularVelocitySmoothing;

        /// <summary>
        /// Gradually changes a <see cref="Vector3"/> towards a desired goal over time.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="final">The position we are trying to reach.</param>
        /// <returns>The smoothed value.</returns>
        protected virtual Vector3 Smooth(Vector3 current, Vector3 final)
        {
            if (TransitionDuration.ApproxEquals(0f) || current.ApproxEquals(final, EqualityTolerance))
            {
                if (isVelocitySmoothing)
                {
                    Transitioned?.Invoke();
                }

                isVelocitySmoothing = false;
                return final;
            }

            isVelocitySmoothing = true;
            return Vector3.SmoothDamp(current, final, ref velocity, TransitionDuration, MaxSpeed, Time.deltaTime);
        }

        /// <summary>
        /// Gradually changes a <see cref="Quaternion"/> towards a desired goal over time.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="final">The position we are trying to reach.</param>
        /// <returns>The smoothed value.</returns>
        protected virtual Quaternion Smooth(Quaternion current, Quaternion final)
        {
            if (TransitionDuration.ApproxEquals(0f) || current.ApproxEquals(final, EqualityTolerance))
            {
                if (isAngularVelocitySmoothing)
                {
                    Transitioned?.Invoke();
                }

                isAngularVelocitySmoothing = false;
                return final;
            }

            isAngularVelocitySmoothing = true;
            return QuaternionExtensions.SmoothDamp(current, final, ref angularVelocity, TransitionDuration);
        }
    }
}