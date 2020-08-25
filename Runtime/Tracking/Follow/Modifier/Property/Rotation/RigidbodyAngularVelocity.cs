namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the <see cref="Rigidbody"/> angular velocity by rotating towards a given source.
    /// </summary>
    public class RigidbodyAngularVelocity : DivergablePropertyModifier
    {
        #region Velocity Settings
        /// <summary>
        /// The maximum squared magnitude of angular velocity that can be applied to the source <see cref="Transform"/>.
        /// </summary>
        [Serialized]
        [field: Header("Velocity Settings"), DocumentedByXml]
        public float AngularVelocityLimit { get; set; } = float.PositiveInfinity;
        /// <summary>
        /// The maximum difference in distance to the tracked position.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float MaxDistanceDelta { get; set; } = 10f;
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
            angle = angle.GetSignedDegree();

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(cachedTargetRigidbody.angularVelocity, angularTarget, MaxDistanceDelta);
                if (float.IsPositiveInfinity(AngularVelocityLimit) || calculatedAngularVelocity.sqrMagnitude < AngularVelocityLimit)
                {
                    cachedTargetRigidbody.angularVelocity = calculatedAngularVelocity;
                }
            }

            base.DoModify(source, target, offset);
        }

        /// <summary>
        /// Gets the source and target Euler rotations to check divergence against.
        /// </summary>
        /// <param name="source">The source to check against.</param>
        /// <param name="target">The target to check with.</param>
        /// <param name="offset">Any offset applied to the target.</param>
        /// <param name="a">The source position.</param>
        /// <param name="b">The target position.</param>
        protected override void GetCheckPoints(GameObject source, GameObject target, GameObject offset, out Vector3 a, out Vector3 b)
        {
            a = source.transform.SignedEulerAngles();
            b = target.transform.SignedEulerAngles();
            if (offset != null)
            {
                b = (offset.transform.localRotation * target.transform.rotation).eulerAngles.UnsignedEulerToSignedEuler();
            }
        }
    }
}