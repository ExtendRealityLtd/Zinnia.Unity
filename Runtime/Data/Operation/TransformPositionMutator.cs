namespace Zinnia.Data.Operation
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Mutates the position of a transform with an optional rotation offset.
    /// </summary>
    public class TransformPositionMutator : TransformPropertyMutator
    {
        /// <summary>
        /// An optional rotation offset.
        /// </summary>
        [DocumentedByXml]
        public GameObject rotationOffset;

        /// <summary>
        /// Sets the rotation offset.
        /// </summary>
        /// <param name="rotationOffset">The new rotation offset.</param>
        public virtual void SetRotationOffset(GameObject rotationOffset)
        {
            this.rotationOffset = rotationOffset;
        }

        /// <summary>
        /// Clears the existing rotation offset.
        /// </summary>
        public virtual void ClearRotationOffset()
        {
            rotationOffset = null;
        }

        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return target.transform.position[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return target.transform.localPosition[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            return target.transform.position += LockIncrementInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return target.transform.localPosition += LockIncrementInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            return target.transform.position = LockSetInput(GetRotationOffset() * input);
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return target.transform.localPosition = LockSetInput(GetRotationOffset() * input);
        }

        /// <summary>
        /// Determines the value to use for the rotation offset.
        /// </summary>
        /// <returns>The rotation offset.</returns>
        protected virtual Quaternion GetRotationOffset()
        {
            return (rotationOffset == null ? Quaternion.identity : (useLocalValues ? rotationOffset.transform.localRotation : rotationOffset.transform.rotation));
        }
    }
}