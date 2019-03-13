namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the rigidbody angular velocity by rotating towards a given source.
    /// </summary>
    public class RigidbodyAngularVelocity : PropertyModifier
    {
        /// <summary>
        /// The maximum squared magnitude of angular velocity that can be applied to the source <see cref="Transform"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AngularVelocityLimit { get; set; } = float.PositiveInfinity;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaxDistanceDelta { get; set; } = 10f;

        /// <summary>
        /// A cached version of the target rigidbody.
        /// </summary>
        protected Rigidbody cachedTargetRigidbody;
        /// <summary>
        /// A cached version of the target.
        /// </summary>
        protected GameObject cachedTarget;

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> angular velocity to rotate towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = cachedTargetRigidbody == null || target != cachedTarget ? target.TryGetComponent<Rigidbody>(true) : cachedTargetRigidbody;
            cachedTarget = target;

            Quaternion rotationDelta = source.transform.rotation * Quaternion.Inverse(offset != null ? offset.transform.rotation : target.transform.rotation);

            rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
            angle = (angle > 180f) ? angle - 360f : angle;

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(cachedTargetRigidbody.angularVelocity, angularTarget, MaxDistanceDelta);
                if (float.IsPositiveInfinity(AngularVelocityLimit) || calculatedAngularVelocity.sqrMagnitude < AngularVelocityLimit)
                {
                    cachedTargetRigidbody.angularVelocity = calculatedAngularVelocity;
                }
            }
        }
    }
}