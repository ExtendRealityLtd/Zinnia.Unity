namespace Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the <see cref="Rigidbody"/> velocity by moving towards a given source.
    /// </summary>
    public class RigidbodyVelocity : DivergablePropertyModifier
    {
        #region Velocity Settings
        [Header("Velocity Settings")]
        [Tooltip("The maximum squared magnitude of velocity that can be applied to the source.")]
        [SerializeField]
        private float velocityLimit = float.PositiveInfinity;
        /// <summary>
        /// The maximum squared magnitude of velocity that can be applied to the source.
        /// </summary>
        public float VelocityLimit
        {
            get
            {
                return velocityLimit;
            }
            set
            {
                velocityLimit = value;
            }
        }
        [Tooltip("The maximum difference in distance to the tracked position.")]
        [SerializeField]
        private float maxDistanceDelta = 10f;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        public float MaxDistanceDelta
        {
            get
            {
                return maxDistanceDelta;
            }
            set
            {
                maxDistanceDelta = value;
            }
        }
        #endregion

        /// <summary>
        /// A cached version of the target <see cref="Rigidbody"/>.
        /// </summary>
        protected Rigidbody cachedTargetRigidbody;
        /// <summary>
        /// A cached version of the target.
        /// </summary>
        protected GameObject cachedTarget;

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> velocity to move towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = cachedTargetRigidbody == null || target != cachedTarget ? target.TryGetComponent<Rigidbody>(true) : cachedTargetRigidbody;
            cachedTarget = target;

            Vector3 positionDelta = source.transform.position - (offset != null ? offset.transform.position : target.transform.position);
            float deltaTime = Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
            Vector3 velocityTarget = positionDelta / deltaTime;
            Vector3 calculatedVelocity = Vector3.MoveTowards(cachedTargetRigidbody.velocity, velocityTarget, MaxDistanceDelta / deltaTime);

            if (calculatedVelocity.sqrMagnitude < VelocityLimit)
            {
                cachedTargetRigidbody.velocity = calculatedVelocity;
            }

            base.DoModify(source, target, offset);
        }

        /// <summary>
        /// Gets the source and target positions to check divergence against.
        /// </summary>
        /// <param name="source">The source to check against.</param>
        /// <param name="target">The target to check with.</param>
        /// <param name="offset">Any offset applied to the target.</param>
        /// <param name="a">The source position.</param>
        /// <param name="b">The target position.</param>

        protected override void GetCheckPoints(GameObject source, GameObject target, GameObject offset, out Vector3 a, out Vector3 b)
        {
            a = source.transform.position;
            b = target.transform.position;

            if (offset != null)
            {
                a = source.transform.position - (offset.transform.position - target.transform.position);
            }
        }
    }
}