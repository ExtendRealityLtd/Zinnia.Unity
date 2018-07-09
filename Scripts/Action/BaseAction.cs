namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

    /// <summary>
    /// The basis for all action types.
    /// </summary>
    public abstract class BaseAction : MonoBehaviour
    {
        /// <summary>
        /// Determines whether the action is currently activated.
        /// </summary>
        public bool IsActivated { get; protected set; }

        /// <summary>
        /// Adds a given action to the sources collection.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public abstract void AddSource(BaseAction action);
        /// <summary>
        /// Removes the given action from the sources collection.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        public abstract void RemoveSource(BaseAction action);
        /// <summary>
        /// Clears all sources.
        /// </summary>
        public abstract void ClearSources();

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
    /// <typeparam name="TSelf">This type itself.</typeparam>
    /// <typeparam name="TValue">The variable type the action will be utilizing.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the action will be utilizing.</typeparam>
    public abstract class BaseAction<TSelf, TValue, TEvent> : BaseAction where TSelf : BaseAction<TSelf, TValue, TEvent> where TEvent : UnityEvent<TValue>, new()
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
        /// Actions to subscribe to when this action is <see cref="Behaviour.enabled"/>. Allows chaining the source actions to this action.
        /// </summary>
        public List<TSelf> Sources
        {
            get
            {
                return sources;
            }
        }

        /// <summary>
        /// Emitted when the action becomes active.
        /// </summary>
        public TEvent Activated = new TEvent();
        /// <summary>
        /// Emitted when the <see cref="Value"/> of the action changes.
        /// </summary>
        public TEvent ValueChanged = new TEvent();
        /// <summary>
        /// Emitted when the action becomes deactivated.
        /// </summary>
        public TEvent Deactivated = new TEvent();

        /// <summary>
        /// Actions to subscribe to when this action is <see cref="Behaviour.enabled"/>. Allows chaining the source actions to this action.
        /// </summary>
        [Tooltip("Actions to subscribe to when this action is enabled. Allows chaining the source actions to this action.")]
        [SerializeField]
        protected List<TSelf> sources = new List<TSelf>();

        /// <inheritdoc />
        public override void AddSource(BaseAction action)
        {
            sources.Add((TSelf)action);
            SubscribeToSource((TSelf)action);
        }

        /// <inheritdoc />
        public override void RemoveSource(BaseAction action)
        {
            UnsubscribeFromSource((TSelf)action);
            sources.Remove((TSelf)action);
        }

        /// <inheritdoc />
        public override void ClearSources()
        {
            UnsubscribeFromSources();
            sources.Clear();
        }

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

            ProcessValue(value);
        }

        protected virtual void OnEnable()
        {
            SubscribeToSources();
        }

        protected virtual void OnDisable()
        {
            ProcessValue(defaultValue);
            UnsubscribeFromSources();
        }

        /// <summary>
        /// Subscribes the current action as a listener to the given action.
        /// </summary>
        /// <param name="source">The source action to subscribe listeners on.</param>
        protected virtual void SubscribeToSource(TSelf source)
        {
            source.Activated.AddListener(Receive);
            source.ValueChanged.AddListener(Receive);
            source.Deactivated.AddListener(Receive);
        }

        /// <summary>
        /// Unsubscribes the current action from listening to the given action.
        /// </summary>
        /// <param name="source">The source action to unsubscribe listeners on.</param>
        protected virtual void UnsubscribeFromSource(TSelf source)
        {
            source.Activated.RemoveListener(Receive);
            source.ValueChanged.RemoveListener(Receive);
            source.Deactivated.RemoveListener(Receive);
        }

        /// <summary>
        /// Attempts to subscribe listeners to each of the source actions.
        /// </summary>
        protected virtual void SubscribeToSources()
        {
            Sources.ForEach(
                source =>
                {
                    SubscribeToSource(source);
                });
        }

        /// <summary>
        /// Attempts to unsubscribe existing listeners from each of the source actions.
        /// </summary>
        protected virtual void UnsubscribeFromSources()
        {
            Sources.ForEach(
                source =>
                {
                    UnsubscribeFromSource(source);
                });
        }

        /// <summary>
        /// Processes the given value and emits the appropriate events.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected virtual void ProcessValue(TValue value)
        {
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