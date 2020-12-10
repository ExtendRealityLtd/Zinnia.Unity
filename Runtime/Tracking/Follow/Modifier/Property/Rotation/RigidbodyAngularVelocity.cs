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

        #region Velocity Settings
        /// <summary>
        /// Whether to use the optional offset to set the target <see cref="Rigidbody.centerOfMass"/>;
        /// </summary>
        [Serialized]
        [field: Header("Calculation Settings"), DocumentedByXml]
        public bool UseOffsetAsCentreOfMass { get; set; }
        /// <summary>
        /// Whether calculate the rotational angle in degrees;
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool CalculateAngleInDegrees { get; set; } = true;
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
        /// A cached version of the offset.
        /// </summary>
        protected GameObject cachedOffset;

        /// <summary>
        /// Sets the <see cref="Rigidbody.centerOfMass"/> on the <see cref="cachedTargetRigidbody"/> based on the <see cref="cachedOffset"/> position.
        /// If <see cref="cachedOffset"/> is not set then the <see cref="Rigidbody.centerOfMass"/> will be reset.
        /// </summary>
        public virtual void SetCentreOfMass()
        {
            ResetCentreOfMass();
            if (!UseOffsetAsCentreOfMass || cachedTargetRigidbody == null || cachedOffset == null)
            {
                return;
            }

            cachedTargetRigidbody.centerOfMass = cachedOffset.transform.position;
        }

        /// <summary>
        /// Resets the <see cref="Rigidbody.centerOfMass"/> on the <see cref="cachedTargetRigidbody"/>.
        /// </summary>
        public virtual void ResetCentreOfMass()
        {
            if (cachedTargetRigidbody == null)
            {
                return;
            }

            cachedTargetRigidbody.ResetCenterOfMass();
        }

        /// <summary>
        /// Modifies the target <see cref="Rigidbody"/> angular velocity to rotate towards the given source.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = cachedTargetRigidbody == null || target != cachedTarget ? target.TryGetComponent<Rigidbody>(true) : cachedTargetRigidbody;
            cachedOffset = offset;

            if (cachedTarget == null || cachedTarget != target)
            {
                SetCentreOfMass();
            }

            cachedTarget = target;

            Quaternion rotationDelta = source.transform.rotation * Quaternion.Inverse(offset != null ? offset.transform.rotation : target.transform.rotation);
            rotationDelta.ToAngleAxis(out float angle, out Vector3 axis);
            float deltaTime = CalculateAngleInDegrees ? 1f : Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
            angle = angle.GetSignedDegree() * (CalculateAngleInDegrees ? 1f : Mathf.Deg2Rad) / deltaTime;

            if (!angle.ApproxEquals(0))
            {
                Vector3 angularTarget = angle * axis;
                Vector3 calculatedAngularVelocity = Vector3.MoveTowards(cachedTargetRigidbody.angularVelocity, angularTarget, MaxDistanceDelta / deltaTime);
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
                a = (source.transform.rotation * Quaternion.Inverse(offset.transform.localRotation)).eulerAngles.UnsignedEulerToSignedEuler();
            }
        }
    }
}