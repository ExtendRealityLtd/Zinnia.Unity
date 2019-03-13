namespace Zinnia.Data.Operation
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Mutates the position of a transform with an optional rotation offset.
    /// </summary>
    public class TransformPositionMutator : TransformPropertyMutator
    {
        /// <summary>
        /// An optional rotation offset.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject RotationOffset { get; set; }

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
            return Target.transform.position += LockIncrementInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return Target.transform.localPosition += LockIncrementInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            return Target.transform.position = LockSetInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return Target.transform.localPosition = LockSetInput(GetRotationOffset() * input);
        }

        /// <summary>
        /// Determines the value to use for the rotation offset.
        /// </summary>
        /// <returns>The rotation offset.</returns>
        protected virtual Quaternion GetRotationOffset()
        {
            return RotationOffset == null ? Quaternion.identity : (UseLocalValues ? RotationOffset.transform.localRotation : RotationOffset.transform.rotation);
        }
    }
}