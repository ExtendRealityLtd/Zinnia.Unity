namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Updates the position and rotation of the target <see cref="Transform"/> by applying velocity and angular velocity to its <see cref="Rigidbody"/> to track the source <see cref="Transform"/> position and rotation.
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
        /// The <see cref="Rigidbody"/> of the current target <see cref="Transform"/>.
        /// </summary>
        public Rigidbody TargetRigidbody
        {
            get
            {
                if (cachedTargetRigidbody == null)
                {
                    cachedTargetRigidbody = (CachedTarget != null ? CachedTarget.GetComponentInChildren<Rigidbody>() : null);
                }
                return cachedTargetRigidbody;
            }
        }

        protected Rigidbody cachedTargetRigidbody;

        /// <summary>
        /// Moves the target <see cref="Rigidbody"/> by applying velocity so it tracks the to the source <see cref="Transform.position"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdatePosition(Transform source, Transform target, Transform offset = null)
        {
            if (TargetRigidbody == null)
            {
                return;
            }

            Vector3 positionDelta = source.position - (offset != null ? offset.position : target.position);
            Vector3 velocityTarget = positionDelta / Time.fixedDeltaTime;
            Vector3 calculatedVelocity = Vector3.MoveTowards(TargetRigidbody.velocity, velocityTarget, maxDistanceDelta);

            if (calculatedVelocity.sqrMagnitude < velocityLimit)
            {
                TargetRigidbody.velocity = calculatedVelocity;
            }
        }

        /// <summary>
        /// Rotates the target <see cref="Rigidbody"/> by applying angular velocity so it tracks the to the source <see cref="Transform.rotation"/>.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoUpdateRotation(Transform source, Transform target, Transform offset = null)
        {
            if (TargetRigidbody == null)
            {
                return;
            }

            Quaternion rotationDelta = source.rotation * Quaternion.Inverse((offset != null ? offset.rotation : target.rotation));
            float angle;
            Vector3 axis;

            rotationDelta.ToAngleAxis(out angle, out axis);
            angle = ((angle > 180f) ? angle - 360f : angle);

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(TargetRigidbody.angularVelocity, angularTarget, maxDistanceDelta);
                if (angularVelocityLimit == float.PositiveInfinity || calculatedAngularVelocity.sqrMagnitude < angularVelocityLimit)
                {
                    TargetRigidbody.angularVelocity = calculatedAngularVelocity;
                }
            }
        }

        /// <summary>
        /// Does not perform any modification.
        /// </summary>
        /// <param name="source">Unused.</param>
        /// <param name="target">Unused.</param>
        /// <param name="target">Unused.</param>
        protected override void DoUpdateScale(Transform source, Transform target, Transform offset = null)
        {
        }
    }
}