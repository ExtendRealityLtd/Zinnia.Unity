namespace VRTK.Core.Mutation.TransformProperty
{
    using UnityEngine;
    using VRTK.Core.Data.Type;

    /// <summary>
    /// Provides a basis for mutating transform Vector3 properties.
    /// </summary>
    public abstract class BaseProperty : MonoBehaviour
    {
        /// <summary>
        /// The target to mutate.
        /// </summary>
        [Tooltip("The target to mutate.")]
        public GameObject target;
        /// <summary>
        /// Determines whether to mutate the local or global values.
        /// </summary>
        [Tooltip("Determines whether to mutate the local or global values.")]
        public bool useLocalValues;
        /// <summary>
        /// Determines which axes to lock and not mutate.
        /// </summary>
        [Tooltip("Determines which axes to lock and not mutate.")]
        public Vector3State lockAxis = Vector3State.False;

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="target">The new target.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clears the existing target.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Sets the property to the new value.
        /// </summary>
        /// <param name="input">The value to set it to.</param>
        /// <returns>The mutated value if the current component is <see cref="Behaviour.isActiveAndEnabled"/> and the <see cref="target"/> is valid. Otherwise returns the default value for <see cref="Vector3"/>.</returns>
        public virtual Vector3 SetProperty(Vector3 input)
        {
            if (!IsValid())
            {
                return default(Vector3);
            }

            input = LockSetInput(input);
            if (useLocalValues)
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
        /// <returns>The mutated value if the current component is <see cref="Behaviour.isActiveAndEnabled"/> and the <see cref="target"/> is valid. Otherwise returns the default value for <see cref="Vector3"/>.</returns>
        public virtual Vector3 IncrementProperty(Vector3 input)
        {
            if (!IsValid())
            {
                return default(Vector3);
            }

            input = LockIncrementInput(input);
            if (useLocalValues)
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
        /// Locks the set input based on the locked axes.
        /// </summary>
        /// <param name="input">The input to lock.</param>
        /// <returns>The input locked on the required axes.</returns>
        protected virtual Vector3 LockSetInput(Vector3 input)
        {
            input.x = (lockAxis.xState ? GetAxisValue(0) : input.x);
            input.y = (lockAxis.yState ? GetAxisValue(1) : input.y);
            input.z = (lockAxis.zState ? GetAxisValue(2) : input.z);
            return input;
        }

        /// <summary>
        /// Locks the increment input based on the locked axes.
        /// </summary>
        /// <param name="input">The input to lock.</param>
        /// <returns>The input locked on the required axes.</returns>
        protected virtual Vector3 LockIncrementInput(Vector3 input)
        {
            input.x = (lockAxis.xState ? 0f : input.x);
            input.y = (lockAxis.yState ? 0f : input.y);
            input.z = (lockAxis.zState ? 0f : input.z);
            return input;
        }

        /// <summary>
        /// Gets the value for a given axis.
        /// </summary>
        /// <param name="axis">The axis to get the value from.</param>
        /// <returns>The axis value.</returns>
        protected virtual float GetAxisValue(int axis)
        {
            if (useLocalValues)
            {
                return GetLocalAxisValue(axis);
            }
            else
            {
                return GetGlobalAxisValue(axis);
            }
        }

        /// <summary>
        /// Determines if the process is valid.
        /// </summary>
        /// <returns><see langword="true"/> if it is valid.</returns>
        protected virtual bool IsValid()
        {
            return (isActiveAndEnabled && target != null);
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
    }
}