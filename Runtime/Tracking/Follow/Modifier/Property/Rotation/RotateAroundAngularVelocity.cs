namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Tracking.Velocity;

    /// <summary>
    /// Rotates the target around the applied axes based on the angular velocity provided by a source <see cref="VelocityTracker"/>.
    /// </summary>
    public class RotateAroundAngularVelocity : PropertyModifier
    {
        [Tooltip("The VelocityTracker that is the source of the angular velocity.")]
        [SerializeField]
        private VelocityTracker angularVelocitySource;
        /// <summary>
        /// The <see cref="VelocityTracker"/> that is the source of the angular velocity.
        /// </summary>
        public VelocityTracker AngularVelocitySource
        {
            get
            {
                return angularVelocitySource;
            }
            set
            {
                angularVelocitySource = value;
            }
        }
        [Tooltip("Multiplies the AngularVelocitySource by this value.")]
        [SerializeField]
        private Vector3 sourceMultiplier = Vector3.one;
        /// <summary>
        /// Multiplies the <see cref="AngularVelocitySource"/> by this value.
        /// </summary>
        public Vector3 SourceMultiplier
        {
            get
            {
                return sourceMultiplier;
            }
            set
            {
                sourceMultiplier = value;
            }
        }
        [Tooltip("The axes to apply the angular velocity to.")]
        [SerializeField]
        private Vector3State applyToAxis;
        /// <summary>
        /// The axes to apply the angular velocity to.
        /// </summary>
        public Vector3State ApplyToAxis
        {
            get
            {
                return applyToAxis;
            }
            set
            {
                applyToAxis = value;
            }
        }
        [Tooltip("When true, transforms the angular velocity to be in target's space instead of world space.")]
        [SerializeField]
        private bool inTargetSpace;
        /// <summary>
        /// When true, transforms the angular velocity to be in target's space instead of world space.
        /// </summary>
        public bool InTargetSpace
        {
            get
            {
                return inTargetSpace;
            }
            set
            {
                inTargetSpace = value;
            }
        }

        /// <summary>
        /// Clears <see cref="AngularVelocitySource"/>.
        /// </summary>
        public virtual void ClearAngularVelocitySource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            AngularVelocitySource = default;
        }

        /// <summary>
        /// Sets the <see cref="SourceMultiplier"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSourceMultiplierX(float value)
        {
            SourceMultiplier = new Vector3(value, SourceMultiplier.y, SourceMultiplier.z);
        }

        /// <summary>
        /// Sets the <see cref="SourceMultiplier"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSourceMultiplierY(float value)
        {
            SourceMultiplier = new Vector3(SourceMultiplier.x, value, SourceMultiplier.z);
        }

        /// <summary>
        /// Sets the <see cref="SourceMultiplier"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSourceMultiplierZ(float value)
        {
            SourceMultiplier = new Vector3(SourceMultiplier.x, SourceMultiplier.y, value);
        }

        /// <summary>
        /// Sets the <see cref="ApplyToAxis"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyToAxisX(bool value)
        {
            ApplyToAxis = new Vector3State(value, ApplyToAxis.yState, ApplyToAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyToAxis"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyToAxisY(bool value)
        {
            ApplyToAxis = new Vector3State(ApplyToAxis.xState, value, ApplyToAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyToAxis"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyToAxisZ(bool value)
        {
            ApplyToAxis = new Vector3State(ApplyToAxis.xState, ApplyToAxis.yState, value);
        }

        /// <summary>
        /// Modifies the target rotation to match the given source rotation.
        /// </summary>
        /// <param name="source">The source is unused in this implementation.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (AngularVelocitySource == null)
            {
                return;
            }

            Vector3 input = InTargetSpace
                ? target.transform.parent.InverseTransformVector(AngularVelocitySource.GetAngularVelocity())
                : AngularVelocitySource.GetAngularVelocity();

            input.Scale(SourceMultiplier);
            input.Scale(ApplyToAxis.ToVector3());

            Vector3 point = offset != null ? offset.transform.position : target.transform.position;

            target.transform.RotateAround(point, target.transform.right, input.x);
            target.transform.RotateAround(point, target.transform.up, input.y);
            target.transform.RotateAround(point, target.transform.forward, input.z);
        }
    }
}
