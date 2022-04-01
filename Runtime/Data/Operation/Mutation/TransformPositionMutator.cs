namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the position of a transform with an optional facing direction.
    /// </summary>
    public class TransformPositionMutator : TransformPropertyMutator
    {
        #region Position Settings
        [Header("Position Settings")]
        [Tooltip("Determines the facing direction when mutating the position.")]
        [SerializeField]
        private GameObject facingDirection;
        /// <summary>
        /// Determines the facing direction when mutating the position.
        /// </summary>
        public GameObject FacingDirection
        {
            get
            {
                return facingDirection;
            }
            set
            {
                facingDirection = value;
            }
        }
        [Tooltip("Determines which axes to take from the FacingDirection.")]
        [SerializeField]
        private Vector3State applyFacingDirectionOnAxis = Vector3State.True;
        /// <summary>
        /// Determines which axes to take from the <see cref="FacingDirection"/>.
        /// </summary>
        public Vector3State ApplyFacingDirectionOnAxis
        {
            get
            {
                return applyFacingDirectionOnAxis;
            }
            set
            {
                applyFacingDirectionOnAxis = value;
            }
        }
        #endregion

        /// <summary>
        /// Clears <see cref="FacingDirection"/>.
        /// </summary>
        public virtual void ClearFacingDirection()
        {
            if (!this.IsValidState())
            {
                return;
            }

            FacingDirection = default;
        }

        /// <summary>
        /// Clears <see cref="ApplyFacingDirectionOnAxis"/>.
        /// </summary>
        public virtual void ClearApplyFacingDirectionOnAxis()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ApplyFacingDirectionOnAxis = default;
        }

        /// <summary>
        /// Sets the <see cref="ApplyFacingDirectionOnAxis"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyFacingDirectionOnAxisX(bool value)
        {
            ApplyFacingDirectionOnAxis = new Vector3State(value, ApplyFacingDirectionOnAxis.yState, ApplyFacingDirectionOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyFacingDirectionOnAxis"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyFacingDirectionOnAxisY(bool value)
        {
            ApplyFacingDirectionOnAxis = new Vector3State(ApplyFacingDirectionOnAxis.xState, value, ApplyFacingDirectionOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyFacingDirectionOnAxis"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyFacingDirectionOnAxisZ(bool value)
        {
            ApplyFacingDirectionOnAxis = new Vector3State(ApplyFacingDirectionOnAxis.xState, ApplyFacingDirectionOnAxis.yState, value);
        }

        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return Target.transform.position[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return Target.transform.localPosition[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            return Target.transform.position += LockIncrementInput(GetFacingDirection() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return Target.transform.localPosition += LockIncrementInput(GetFacingDirection() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            return Target.transform.position = LockSetInput(GetFacingDirection() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return Target.transform.localPosition = LockSetInput(GetFacingDirection() * input);
        }

        /// <summary>
        /// Determines the value to use for the facing direction.
        /// </summary>
        /// <returns>The facing direction.</returns>
        protected virtual Quaternion GetFacingDirection()
        {
            if (FacingDirection == null)
            {
                return Quaternion.identity;
            }

            Quaternion returnValue = UseLocalValues ? FacingDirection.transform.localRotation : FacingDirection.transform.rotation;
            Vector3 facingAxesToApply = ApplyFacingDirectionOnAxis.ToVector3();

            if (facingAxesToApply.ApproxEquals(Vector3.one))
            {
                return returnValue;
            }

            facingAxesToApply.Scale(returnValue.eulerAngles);
            return Quaternion.Euler(facingAxesToApply);
        }
    }
}