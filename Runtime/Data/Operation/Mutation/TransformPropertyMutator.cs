namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Type;

    /// <summary>
    /// Provides a basis for mutating transform Vector3 properties.
    /// </summary>
    public abstract class TransformPropertyMutator : MonoBehaviour
    {
        /// <summary>
        /// The target to mutate.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// Determines whether to mutate the local or global values.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool UseLocalValues { get; set; }
        /// <summary>
        /// Determines which axes to mutate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3State MutateOnAxis { get; set; } = Vector3State.True;

        /// <summary>
        /// Sets the property to the new value.
        /// </summary>
        /// <param name="input">The value to set it to.</param>
        /// <returns>The mutated value if the current component is <see cref="Behaviour.isActiveAndEnabled"/> and the <see cref="Target"/> is valid. Otherwise returns the default value for <see cref="Vector3"/>.</returns>
        public virtual Vector3 SetProperty(Vector3 input)
        {
            if (!IsValid())
            {
                return default;
            }

            input = LockSetInput(input);
            if (UseLocalValues)
            {
                return SetLocal(input);
            }
            else
            {
                return SetGlobal(input);
            }
        }

        /// <summary>
        /// Sets the property to the new value.
        /// </summary>
        /// <param name="input">The value to set it to.</param>
        public virtual void DoSetProperty(Vector3 input)
        {
            SetProperty(input);
        }

        /// <summary>
        /// Increments the property by the given value.
        /// </summary>
        /// <param name="input">The value to increment by.</param>
        /// <returns>The mutated value if the current component is <see cref="Behaviour.isActiveAndEnabled"/> and the <see cref="Target"/> is valid. Otherwise returns the default value for <see cref="Vector3"/>.</returns>
        public virtual Vector3 IncrementProperty(Vector3 input)
        {
            if (!IsValid())
            {
                return default;
            }

            input = LockIncrementInput(input);
            if (UseLocalValues)
            {
                return IncrementLocal(input);
            }
            else
            {
                return IncrementGlobal(input);
            }
        }

        /// <summary>
        /// Increments the property by the given value.
        /// </summary>
        /// <param name="input">The value to increment by.</param>
        public virtual void DoIncrementProperty(Vector3 input)
        {
            IncrementProperty(input);
        }

        /// <summary>
        /// Sets the local property to the new value.
        /// </summary>
        /// <param name="input">The value to set it to.</param>
        /// <returns>The new value.</returns>
        protected abstract Vector3 SetLocal(Vector3 input);
        /// <summary>
        /// Sets the global property to the new value.
        /// </summary>
        /// <param name="input">The value to set it to.</param>
        /// <returns>The new value.</returns>
        protected abstract Vector3 SetGlobal(Vector3 input);
        /// <summary>
        /// Increments the local property by the given value.
        /// </summary>
        /// <param name="input">The value to increment by.</param>
        /// <returns>The new value.</returns>
        protected abstract Vector3 IncrementLocal(Vector3 input);
        /// <summary>
        /// Increments the global property by the given value.
        /// </summary>
        /// <param name="input">The value to increment by.</param>
        /// <returns>The new value.</returns>
        protected abstract Vector3 IncrementGlobal(Vector3 input);
        /// <summary>
        /// Gets the value for a given axis on the local property.
        /// </summary>
        /// <param name="axis">The axis to get the value from.</param>
        /// <returns>The axis value.</returns>
        protected abstract float GetLocalAxisValue(int axis);
        /// <summary>
        /// Gets the value for a given axis on the global property.
        /// </summary>
        /// <param name="axis">The axis to get the value from.</param>
        /// <returns>The axis value.</returns>
        protected abstract float GetGlobalAxisValue(int axis);

        /// <summary>
        /// Locks the set input based on the locked axes.
        /// </summary>
        /// <param name="input">The input to lock.</param>
        /// <returns>The input locked on the required axes.</returns>
        protected virtual Vector3 LockSetInput(Vector3 input)
        {
            input.x = MutateOnAxis.xState ? input.x : GetAxisValue(0);
            input.y = MutateOnAxis.yState ? input.y : GetAxisValue(1);
            input.z = MutateOnAxis.zState ? input.z : GetAxisValue(2);
            return input;
        }

        /// <summary>
        /// Locks the increment input based on the locked axes.
        /// </summary>
        /// <param name="input">The input to lock.</param>
        /// <returns>The input locked on the required axes.</returns>
        protected virtual Vector3 LockIncrementInput(Vector3 input)
        {
            input.x = MutateOnAxis.xState ? input.x : 0f;
            input.y = MutateOnAxis.yState ? input.y : 0f;
            input.z = MutateOnAxis.zState ? input.z : 0f;
            return input;
        }

        /// <summary>
        /// Gets the value for a given axis.
        /// </summary>
        /// <param name="axis">The axis to get the value from.</param>
        /// <returns>The axis value.</returns>
        protected virtual float GetAxisValue(int axis)
        {
            return UseLocalValues ? GetLocalAxisValue(axis) : GetGlobalAxisValue(axis);
        }

        /// <summary>
        /// Determines if the process is valid.
        /// </summary>
        /// <returns><see langword="true"/> if it is valid.</returns>
        [RequiresBehaviourState]
        protected virtual bool IsValid()
        {
            return Target != null;
        }
    }
}