namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Provides a basis for mutating transform Vector3 properties.
    /// </summary>
    public abstract class TransformPropertyMutator : MonoBehaviour
    {
        #region Target Settings
        [Header("Target Settings")]
        [Tooltip("The target to mutate.")]
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// The target to mutate.
        /// </summary>
        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        [Tooltip("Determines whether to mutate the local or global values.")]
        [SerializeField]
        private bool useLocalValues;
        /// <summary>
        /// Determines whether to mutate the local or global values.
        /// </summary>
        public bool UseLocalValues
        {
            get
            {
                return useLocalValues;
            }
            set
            {
                useLocalValues = value;
            }
        }
        [Tooltip("Determines which axes to mutate.")]
        [SerializeField]
        private Vector3State mutateOnAxis = Vector3State.True;
        /// <summary>
        /// Determines which axes to mutate.
        /// </summary>
        public Vector3State MutateOnAxis
        {
            get
            {
                return mutateOnAxis;
            }
            set
            {
                mutateOnAxis = value;
            }
        }
        #endregion

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Clears <see cref="MutateOnAxis"/>.
        /// </summary>
        public virtual void ClearMutateOnAxis()
        {
            if (!this.IsValidState())
            {
                return;
            }

            MutateOnAxis = default;
        }

        /// <summary>
        /// Sets the <see cref="MutateOnAxis"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetMutateOnAxisX(bool value)
        {
            MutateOnAxis = new Vector3State(value, MutateOnAxis.yState, MutateOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="MutateOnAxis"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetMutateOnAxisY(bool value)
        {
            MutateOnAxis = new Vector3State(MutateOnAxis.xState, value, MutateOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="MutateOnAxis"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetMutateOnAxisZ(bool value)
        {
            MutateOnAxis = new Vector3State(MutateOnAxis.xState, MutateOnAxis.yState, value);
        }

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
        protected virtual bool IsValid()
        {
            if (!this.IsValidState())
            {
                return default;
            }

            return Target != null;
        }
    }
}