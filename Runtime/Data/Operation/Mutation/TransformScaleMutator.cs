namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the scale of a transform.
    /// </summary>
    public class TransformScaleMutator : TransformPropertyMutator
    {
        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return Target.transform.lossyScale[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return Target.transform.localScale[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            Vector3 globalScale = Target.transform.lossyScale + input;
            Target.transform.SetGlobalScale(globalScale);
            return globalScale;
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            return Target.transform.localScale += input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            Target.transform.SetGlobalScale(input);
            return input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            return Target.transform.localScale = input;
        }
    }
}