namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the <see cref="Rigidbody"/> angular velocity by rotating towards a given source.
    /// </summary>
    public class RigidbodyAngularVelocity : DivergablePropertyModifier
    {
        #region Velocity Settings
        [Header("Velocity Settings")]
        [Tooltip("The maximum squared magnitude of angular velocity that can be applied to the source Transform.")]
        [SerializeField]
        private float angularVelocityLimit = float.PositiveInfinity;
        /// <summary>
        /// The maximum squared magnitude of angular velocity that can be applied to the source <see cref="Transform"/>.
        /// </summary>
        public float AngularVelocityLimit
        {
            get
            {
                return angularVelocityLimit;
            }
            set
            {
                angularVelocityLimit = value;
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

        #region Velocity Settings
        [Header("Calculation Settings")]
        [Tooltip("Whether to use the optional offset to set the target Rigidbody.centerOfMass;")]
        [SerializeField]
        private bool useOffsetAsCentreOfMass;
        /// <summary>
        /// Whether to use the optional offset to set the target <see cref="Rigidbody.centerOfMass"/>;
        /// </summary>
        public bool UseOffsetAsCentreOfMass
        {
            get
            {
                return useOffsetAsCentreOfMass;
            }
            set
            {
                useOffsetAsCentreOfMass = value;
            }
        }
        [Tooltip("Whether calculate the rotational angle in degrees;")]
        [SerializeField]
        private bool calculateAngleInDegrees = true;
        /// <summary>
        /// Whether calculate the rotational angle in degrees;
        /// </summary>
        public bool CalculateAngleInDegrees
        {
            get
            {
                return calculateAngleInDegrees;
            }
            set
            {
                calculateAngleInDegrees = value;
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