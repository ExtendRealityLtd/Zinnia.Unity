namespace Zinnia.Data.Operation.Mutation
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the position of a transform with an optional facing direction.
    /// </summary>
    public class TransformPositionMutator : TransformPropertyMutator
    {
        #region Position Settings
        /// <summary>
        /// Determines the facing direction when mutating the position.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Position Settings"), DocumentedByXml]
        public GameObject FacingDirection { get; set; }
        /// <summary>
        /// Determines which axes to take from the <see cref="FacingDirection"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3State ApplyFacingDirectionOnAxis { get; set; } = Vector3State.True;
        #endregion

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