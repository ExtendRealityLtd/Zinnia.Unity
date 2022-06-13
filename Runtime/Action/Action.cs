namespace Zinnia.Action
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// The basis for all action types.
    /// </summary>
    public abstract class Action : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/>.
        /// </summary>
        [Serializable]
        public class BooleanUnityEvent : UnityEvent<bool> { }

        /// <summary>
        /// Emitted when <see cref="IsActivated"/> changes.
        /// </summary>
        public BooleanUnityEvent ActivationStateChanged = new BooleanUnityEvent();

        /// <summary>
        /// The backing field for holding the value of <see cref="IsActivated"/>.
        /// </summary>
        private bool isActivated;
        /// <summary>
        /// Whether the action is currently activated.
        /// </summary>
        public virtual bool IsActivated
        {
            get => isActivated;
            protected set
            {
                if (isActivated == value)
                {
                    return;
                }

                isActivated = value;
                ActivationStateChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Adds a given action to the sources collection.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public abstract void AddSource(Action action);
        /// <summary>
        /// Removes the given action from the sources collection.
        /// </summary>
        /// <param name="action">The action to remove.</param>
        /// <returns>Whether the remove was successful.</returns>
        public abstract bool RemoveSource(Action action);
        /// <summary>
        /// Clears all sources.
        /// </summary>
        public abstract void ClearSources();
        /// <summary>
        /// Determines whether the sources collection contains an action.
        /// </summary>
        /// <param name="action">The action to check.</param>
        /// <returns>Whether it contains the given action.</returns>
        public abstract bool SourcesContains(Action action);
        /// <summary>
        /// Emits the appropriate event for when the activation state changes from Activated or Deactivated.
        /// </summary>
        public abstract void EmitActivationState();
        /// <summary>
        /// Makes the action receive its own initial value to reset it back to when it was first created.
        /// </summary>
        public abstract void ReceiveInitialValue();
        /// <summary>
        /// Makes the action receive its own default value to set it back to inactive.
        /// </summary>
        public abstract void ReceiveDefaultValue();

        /// <summary>
        /// Whether the event should be emitted.
        /// </summary>
        /// <returns><see langword="true"/> if the event should be emitted.</returns>
        protected virtual bool CanEmit()
        {
            return isActiveAndEnabled;
        }
    }

    /// <summary>
    /// A generic type that forms as the basis for all action types.
    /// </summary>
    /// <typeparam name="TSelf">This type itself.</typeparam>
    /// <typeparam name="TValue">The variable type the action will be utilizing.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the action will be utilizing.</typeparam>
    public abstract class Action<TSelf, TValue, TEvent> : Action where TSelf : Action<TSelf, TValue, TEvent> where TEvent : UnityEvent<TValue>, new()
    {
        [Tooltip("The initial value upon creation of the component.")]
        [SerializeField]
        private TValue initialValue;
        /// <summary>
        /// The initial value upon creation of the component.
        /// </summary>
        public TValue InitialValue
        {
            get
            {
                return initialValue;
            }
            set
            {
                initialValue = value;
            }
        }
        [Tooltip("The value that is considered the inactive value.")]
        [SerializeField]
        private TValue defaultValue;
        /// <summary>
        /// The value that is considered the inactive value.
        /// </summary>
        public TValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterDefaultValueChange();
                }
            }
        }
        [Tooltip("Actions to subscribe to when this action is Behaviour.enabled. Allows chaining the source actions to this action.")]
        [SerializeField]
        private List<TSelf> sources = new List<TSelf>();
        /// <summary>
        /// Actions to subscribe to when this action is <see cref="Behaviour.enabled"/>. Allows chaining the source actions to this action.
        /// </summary>
        protected List<TSelf> Sources
        {
            get
            {
                return sources;
            }
            set
            {
                if (this.IsMemberChangeAllowed())
                {
                    OnBeforeSourcesChange();
                }
                sources = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterSourcesChange();
                }
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
        /// Emitted when the <see cref="Value"/> of the action remains unchanged.
        /// </summary>
        public TEvent ValueUnchanged = new TEvent();
        /// <summary>
        /// Emitted when the action becomes deactivated.
        /// </summary>
        public TEvent Deactivated = new TEvent();

        [Tooltip("Actions to subscribe to when this action is Behaviour.enabled. Allows chaining the source actions to this action.")]
        [SerializeField]
        private TValue value;
        /// <summary>
        /// The value of the action.
        /// </summary>
        public TValue Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        /// <summary>
        /// Actions subscribed to when this action is <see cref="Behaviour.enabled"/>. Allows chaining the source actions to this action.
        /// </summary>
        public virtual HeapAllocationFreeReadOnlyList<TSelf> ReadOnlySources => sources;

        /// <inheritdoc />
        public override void AddSource(Action action)
        {
            if (!this.IsValidState() || action == null)
            {
                return;
            }

            Sources.Add((TSelf)action);
            SubscribeToSource((TSelf)action);
        }

        /// <inheritdoc />
        public override bool RemoveSource(Action action)
        {
            if (!this.IsValidState() || action == null)
            {
                return false;
            }

            UnsubscribeFromSource((TSelf)action);
            return Sources.Remove((TSelf)action);
        }

        /// <inheritdoc />
        public override void ClearSources()
        {
            if (!this.IsValidState())
            {
                return;
            }

            UnsubscribeFromSources();
            Sources.Clear();
        }

        /// <inheritdoc />
        public override bool SourcesContains(Action action)
        {
            if (!this.IsValidState() || action == null)
            {
                return false;
            }

            return Sources.Contains((TSelf)action);
        }

        /// <inheritdoc />
        public override void EmitActivationState()
        {
            if (!this.IsValidState())
            {
                return;
            }

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

        /// <inheritdoc />
        public override void ReceiveInitialValue()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Receive(InitialValue);
        }

        /// <inheritdoc />
        public override void ReceiveDefaultValue()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Receive(DefaultValue);
        }

        /// <summary>
        /// Acts on the value.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        public virtual void Receive(TValue value)
        {
            if (!this.IsValidState())
            {
                return;
            }

            if (IsValueEqual(value))
            {
                ValueUnchanged?.Invoke(Value);
                return;
            }

            ProcessValue(value);
        }

        protected virtual void Awake()
        {
            Value = DefaultValue;
        }

        protected virtual void OnEnable()
        {
            SubscribeToSources();
        }

        protected virtual void Start()
        {
            if (!IsValueEqual(InitialValue))
            {
                ProcessValue(InitialValue);
            }
        }

        protected virtual void OnDisable()
        {
            ProcessValue(DefaultValue);
            UnsubscribeFromSources();
        }

        /// <summary>
        /// Subscribes the current action as a listener to the given action.
        /// </summary>
        /// <param name="source">The source action to subscribe listeners on.</param>
        protected virtual void SubscribeToSource(TSelf source)
        {
            if (source == null)
            {
                return;
            }

            source.ValueChanged.AddListener(Receive);
        }

        /// <summary>
        /// Unsubscribes the current action from listening to the given action.
        /// </summary>
        /// <param name="source">The source action to unsubscribe listeners on.</param>
        protected virtual void UnsubscribeFromSource(TSelf source)
        {
            if (source == null)
            {
                return;
            }

            source.ValueChanged.RemoveListener(Receive);
        }

        /// <summary>
        /// Attempts to subscribe listeners to each of the source actions.
        /// </summary>
        protected virtual void SubscribeToSources()
        {
            if (Sources == null)
            {
                return;
            }

            foreach (TSelf source in Sources)
            {
                SubscribeToSource(source);
            }
        }

        /// <summary>
        /// Attempts to unsubscribe existing listeners from each of the source actions.
        /// </summary>
        protected virtual void UnsubscribeFromSources()
        {
            if (Sources == null)
            {
                return;
            }

            foreach (TSelf source in Sources)
            {
                UnsubscribeFromSource(source);
            }
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
        /// Whether the given <see cref="TValue"/> is equal to the action's cached <see cref="Value"/>.
        /// </summary>
        /// <param name="value">The value to check equality for.</param>
        /// <returns><see langword="true"/> if the given <see cref="TValue"/> is equal to the action's cached <see cref="Value"/>.</returns>
        protected virtual bool IsValueEqual(TValue value)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, value);
        }

        /// <summary>
        /// Whether the action should become active.
        /// </summary>
        /// <param name="value">The current value to check activation state on.</param>
        /// <returns><see langword="true"/> if the action should activate.</returns>
        protected virtual bool ShouldActivate(TValue value)
        {
            return !EqualityComparer<TValue>.Default.Equals(DefaultValue, value);
        }

        /// <summary>
        /// Called after <see cref="DefaultValue"/> has been changed.
        /// </summary>
        protected virtual void OnAfterDefaultValueChange()
        {
            bool shouldActivate = ShouldActivate(Value);
            if (IsActivated == shouldActivate)
            {
                return;
            }

            IsActivated = shouldActivate;
            EmitActivationState();
        }

        /// <summary>
        /// Called before <see cref="Sources"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeSourcesChange()
        {
            UnsubscribeFromSources();
        }

        /// <summary>
        /// Called after <see cref="Sources"/> has been changed.
        /// </summary>
        protected virtual void OnAfterSourcesChange()
        {
            SubscribeToSources();
        }
    }
}