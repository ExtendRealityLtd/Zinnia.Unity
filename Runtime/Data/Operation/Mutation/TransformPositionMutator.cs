namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Mutates the position of a transform with an optional facing direction.
    /// </summary>
    public class TransformPositionMutator : TransformPropertyMutator
    {
        /// <summary>
        /// Determines the facing direction when mutating the position.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject FacingDirection { get; set; }

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
            return FacingDirection == null ? Quaternion.identity : (UseLocalValues ? FacingDirection.transform.localRotation : FacingDirection.transform.rotation);
        }
    }
}