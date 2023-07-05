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
            return Target != null ? Target.transform.lossyScale[axis] : default;
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return Target != null ? Target.transform.localScale[axis] : default;
        }

        /// <inheritdoc/>
        protected override Vector3 GetNewSetValue(Vector3 input)
        {
            return input;
        }

        /// <inheritdoc/>
        protected override Vector3 GetNewIncrementValue(Vector3 input)
        {
            if (Target == null)
            {
                return default;
            }

            return (UseLocalValues ? Target.transform.localScale : Target.transform.lossyScale) + input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobalTargetValue(Vector3 input)
        {
            if (Target == null)
            {
                return default;
            }

            Target.transform.SetGlobalScale(input);
            return input;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocalTargetValue(Vector3 input)
        {
            if (Target == null)
            {
                return default;
            }

            return Target.transform.localScale = input;
        }
    }
}