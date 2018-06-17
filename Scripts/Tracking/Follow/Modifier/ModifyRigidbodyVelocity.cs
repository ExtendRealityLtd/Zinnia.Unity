namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Updates the position and rotation of the source <see cref="Transform"/> by applying velocity and angular velocity to its <see cref="Rigidbody"/> to track the target <see cref="Transform"/> position and rotation.
    /// </summary>
    public class ModifyRigidbodyVelocity : FollowModifier
    {
        /// <summary>
        /// The maximum squared magnitude of velocity that can be applied to the source <see cref="Transform"/>.
        /// </summary>
        [Tooltip("The maximum squared magnitude of velocity that can be applied to the source Transform.")]
        public float velocityLimit = float.PositiveInfinity;
        /// <summary>
        /// The maximum squared magnitude of angular velocity that can be applied to the source <see cref="Transform"/>.
        /// </summary>
        [Tooltip("The maximum squared magnitude of angular velocity that can be applied to the source Transform.")]
        public float angularVelocityLimit = float.PositiveInfinity;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        [Tooltip("The maximum difference in distance to the tracked position.")]
        public float maxDistanceDelta = 10f;

        /// <summary>
        /// The <see cref="Rigidbody"/> of the current source <see cref="Transform"/>.
        /// </summary>
        public Rigidbody SourceRigidbody
        {
            get
            {
                if (cachedSourceRigidbody == null)
                {
                    cachedSourceRigidbody = (CachedSource != null ? CachedSource.GetComponentInChildren<Rigidbody>() : null);
                }
                return cachedSourceRigidbody;
            }
        }

        protected Rigidbody cachedSourceRigidbody;

        /// <summary>
        /// Moves the source <see cref="Rigidbody"/> by applying velocity so it tracks the to the target <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        protected override void DoUpdatePosition(Transform source, Transform target)
        {
            if (SourceRigidbody == null)
            {
                return;
            }

            Vector3 positionDelta = target.position - source.position;
            Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
            Vector3 calculatedVelocity = Vector3.MoveTowards(SourceRigidbody.velocity, velocityTarget, maxDistanceDelta);

            if (calculatedVelocity.sqrMagnitude < velocityLimit)
            {
                SourceRigidbody.velocity = calculatedVelocity;
            }
        }

        /// <summary>
        /// Rotates the source <see cref="Rigidbody"/> by applying angular velocity so it tracks the to the target <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to modify.</param>
        /// <param name="target">The target <see cref="Transform"/> to utilize in the modification.</param>
        protected override void DoUpdateRotation(Transform source, Transform target)
        {
            if (SourceRigidbody == null)
            {
                return;
            }

            Quaternion rotationDelta = target.rotation * Quaternion.Inverse(source.rotation);
            float angle;
            Vector3 axis;

            rotationDelta.ToAngleAxis(out angle, out axis);
            angle = ((angle > 180f) ? angle - 360f : angle);

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(SourceRigidbody.angularVelocity, angularTarget, maxDistanceDelta);
                if (angularVelocityLimit == float.PositiveInfinity || calculatedAngularVelocity.sqrMagnitude < angularVelocityLimit)
                {
                    SourceRigidbody.angularVelocity = calculatedAngularVelocity;
                }
            }
        }

        /// <summary>
        /// Does not perform any modification.
        /// </summary>
        /// <param name="source">Unused.</param>
        /// <param name="target">Unused.</param>
        protected override void DoUpdateScale(Transform source, Transform target)
        {
        }
    }
}