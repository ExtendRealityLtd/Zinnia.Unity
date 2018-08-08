namespace VRTK.Core.Mutation.TransformProperty
{
    using UnityEngine;

    /// <summary>
    /// Mutates the euler rotation of a transform.
    /// </summary>
    public class EulerRotationProperty : BaseProperty
    {
        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return target.transform.eulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return target.transform.localEulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            return target.transform.eulerAngles += input;
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return target.transform.localEulerAngles += input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            return target.transform.eulerAngles = input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return target.transform.localEulerAngles = input;
        }
    }
}