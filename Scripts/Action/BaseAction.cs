namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

    /// <summary>
    /// A generic type that forms as the basis for all action types.
    /// </summary>
    /// <typeparam name="TValue">The variable type the action will be utilizing.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the action will be utilizing.</typeparam>
    public abstract class BaseAction<TValue, TEvent> : BaseAction where TEvent : UnityEvent<TValue, object>, new()
    {
        /// <summary>
        /// Emitted when the <see cref="BaseAction{TValue,TEvent}"/> becomes active.
        /// </summary>
        public TEvent Activated = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="Value"/> of the <see cref="BaseAction{TValue,TEvent}"/> changes.
        /// </summary>
        public TEvent Changed = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="BaseAction{TValue,TEvent}"/> becomes inactive.
        /// </summary>
        public TEvent Deactivated = new TEvent();

        /// <summary>
        /// The initial value of the action.
        /// </summary>
        public TValue DefaultValue;

        /// <summary>
        /// The value of the action.
        /// </summary>
        public TValue Value
        {
            get;
            protected set;
        }

        /// <summary>
        /// The comparer to use for equality comparisons of <see cref="Value"/>.
        /// </summary>
        protected IEqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;

        /// <summary>
        /// The previous value of the action.
        /// </summary>
        protected TValue previousValue;

        /// <summary>
        /// Allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public virtual void Receive(TValue value, object sender = null)
        {
            if (equalityComparer.Equals(Value, value))
            {
                return;
            }

            previousValue = Value;
            Value = value;

            sender = sender ?? this;

            bool isActivated = !equalityComparer.Equals(Value, DefaultValue);
            if (IsActivated != isActivated)
            {
                IsActivated = isActivated;

                if (IsActivated)
                {
                    OnActivated(value, sender);
                }
                else
                {
                    OnDeactivated(value, sender);
                }
            }

            OnChanged(value, sender);
        }

        protected virtual void Awake()
        {
            previousValue = DefaultValue;
            Value = DefaultValue;
        }

        protected virtual void OnActivated(TValue value, object sender)
        {
            if (CanEmit())
            {
                Activated?.Invoke(value, sender);
            }
        }

        protected virtual void OnChanged(TValue value, object sender)
        {
            if (CanEmit())
            {
                Changed?.Invoke(value, sender);
            }
        }

        protected virtual void OnDeactivated(TValue value, object sender)
        {
            if (CanEmit())
            {
                Deactivated?.Invoke(value, sender);
            }
        }
    }

    /// <summary>
    /// The basis for all action types. Don't subclass this, inherit from <see cref="BaseAction{TValue,TEvent}"/> instead.
    /// </summary>
    public abstract class BaseAction : MonoBehaviour
    {
        /// <summary>
        /// Whether the action is activated.
        /// </summary>
        public bool IsActivated
        {
            get;
            protected set;
        }

        /// <summary>
        /// Determines whether any event should be emitted.
        /// </summary>
        /// <returns><see langword="true"/> if the event should be emitted.</returns>
        protected virtual bool CanEmit()
        {
            return isActiveAndEnabled;
        }
    }
}