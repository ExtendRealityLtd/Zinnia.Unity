namespace Zinnia.Data.Operation
{
    using UnityEngine;

    /// <summary>
    /// Mutates the euler rotation of a transform.
    /// </summary>
    public class TransformEulerRotationMutator : TransformPropertyMutator
    {
        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return Target.transform.eulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return Target.transform.localEulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            return Target.transform.eulerAngles += input;
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return Target.transform.localEulerAngles += input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            return Target.transform.eulerAngles = input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return Target.transform.localEulerAngles = input;
        }
    }
}