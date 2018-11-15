namespace VRTK.Core.Mutation.TransformProperty
{
    using UnityEngine;
    using VRTK.Core.Extension;

    /// <summary>
    /// Mutates the scale of a transform.
    /// </summary>
    public class ScaleProperty : Property
    {
        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return target.transform.lossyScale[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return target.transform.localScale[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            Vector3 globalScale = target.transform.lossyScale + input;
            target.transform.SetGlobalScale(globalScale);
            return globalScale;
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return target.transform.localScale += input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            target.transform.SetGlobalScale(input);
            return input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return target.transform.localScale = input;
        }
    }
}