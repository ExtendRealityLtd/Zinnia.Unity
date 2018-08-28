namespace VRTK.Core.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Updates the rigidbody angular velocity by rotating towards a given source.
    /// </summary>
    public class RigidbodyAngularVelocity : PropertyModifier
    {
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

        protected Rigidbody cachedTargetRigidbody;
        protected GameObject cachedTarget;

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> angular velocity to rotate towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = (cachedTargetRigidbody == null || target != cachedTarget ? target.FindRigidbody() : cachedTargetRigidbody);
            cachedTarget = target;

            Quaternion rotationDelta = source.transform.rotation * Quaternion.Inverse((offset != null ? offset.transform.rotation : target.transform.rotation));
            float angle;
            Vector3 axis;

            rotationDelta.ToAngleAxis(out angle, out axis);
            angle = ((angle > 180f) ? angle - 360f : angle);

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(cachedTargetRigidbody.angularVelocity, angularTarget, maxDistanceDelta);
                if (angularVelocityLimit == float.PositiveInfinity || calculatedAngularVelocity.sqrMagnitude < angularVelocityLimit)
                {
                    cachedTargetRigidbody.angularVelocity = calculatedAngularVelocity;
                }
            }
        }
    }
}