namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// The basis for all action types.
    /// </summary>
    public abstract class BaseAction : MonoBehaviour
    {
        /// <summary>
        /// Determines whether the action is currently activated.
        /// </summary>
        public bool IsActivated
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines whether the event should be emitted.
        /// </summary>
        /// <returns><see langword="true"/> if the event should be emitted.</returns>
        protected virtual bool CanEmit()
        {
            return (isActiveAndEnabled);
        }
    }

    /// <summary>
    /// A generic type that forms as the basis for all action types.
    /// </summary>
    /// <typeparam name="TValue">The variable type the action will be utilizing.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the action will be utilizing.</typeparam>
    public abstract class BaseAction<TValue, TEvent> : BaseAction where TEvent : UnityEvent<TValue>, new()
    {
        /// <summary>
        /// The value of the action.
        /// </summary>
        public TValue Value
        {
            get;
            protected set;
        }

        /// <summary>
        /// The initial value of the action.
        /// </summary>
        [Tooltip("The initial value of the action.")]
        public TValue defaultValue;

        /// <summary>
        /// Emitted when the <see cref="BaseAction{TValue,TEvent}"/> becomes active.
        /// </summary>
        public TEvent Activated = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="Value"/> of the <see cref="BaseAction{TValue,TEvent}"/> changes.
        /// </summary>
        public TEvent ValueChanged = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="BaseAction{TValue,TEvent}"/> becomes deactivated.
        /// </summary>
        public TEvent Deactivated = new TEvent();

        /// <summary>
        /// Acts on the value.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        public virtual void Receive(TValue value)
        {
            if (!isActiveAndEnabled || IsValueEqual(value))
            {
                return;
            }

            Value = value;

            bool shouldActivate = ShouldActivate(value);
            if (IsActivated != shouldActivate)
            {
                IsActivated = shouldActivate;
                EmitActivationState();
            }
            else
            {
                ValueChanged?.Invoke(Value);
            }
        }

        /// <summary>
        /// Determines if the given <see cref="TValue"/> is equal to the action's cached <see cref="Value"/>.
        /// </summary>
        /// <param name="value">The value to check equality for.</param>
        /// <returns><see langword="true"/> if the given <see cref="TValue"/> is equal to the action's cached <see cref="Value"/>.</returns>
        protected virtual bool IsValueEqual(TValue value)
        {
            return Value.Equals(value);
        }

        /// <summary>
        /// Determines if the action should become active.
        /// </summary>
        /// <param name="value">The current value to check activation state on.</param>
        /// <returns><see langword="true"/> if the action should activate.</returns>
        protected virtual bool ShouldActivate(TValue value)
        {
            return !defaultValue.Equals(value);
        }

        /// <summary>
        /// Emits the appropriate event for when the activation state changes from Activated or Deactivated.
        /// </summary>
        protected virtual void EmitActivationState()
        {
            if (IsActivated)
            {
                Activated?.Invoke(Value);
                ValueChanged?.Invoke(Value);
            }
            else
            {
                ValueChanged?.Invoke(Value);
                Deactivated?.Invoke(Value);
            }
        }
    }
}