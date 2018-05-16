namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="Vector2"/> value.
    /// </summary>
    public class Vector2Action : BaseAction<Vector2>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> value and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class Vector2ActionUnityEvent : UnityEvent<Vector2, object>
        {
        }

        /// <summary>
        /// Emitted when the action value changes into it's active state.
        /// </summary>
        public Vector2ActionUnityEvent Activated = new Vector2ActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes from it's previous state.
        /// </summary>
        public Vector2ActionUnityEvent Changed = new Vector2ActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes into it's inactive state.
        /// </summary>
        public Vector2ActionUnityEvent Deactivated = new Vector2ActionUnityEvent();

        /// <inheritdoc />
        public override void Receive(Vector2 value, object sender = null)
        {
            previousValue = Value;
            Value = value;
            EmitEvents();
            State = IsActive();
        }

        /// <inheritdoc />
        protected override void OnActivated(Vector2 value)
        {
            if (CanEmit())
            {
                Activated?.Invoke(value, this);
            }
        }

        /// <inheritdoc />
        protected override void OnChanged(Vector2 value)
        {
            if (CanEmit())
            {
                Changed?.Invoke(value, this);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivated(Vector2 value)
        {
            if (CanEmit())
            {
                Deactivated?.Invoke(value, this);
            }
        }

        /// <summary>
        /// Attempts to emit each of the events if they are should be emitted.
        /// </summary>
        protected virtual void EmitEvents()
        {
            if (Activate())
            {
                OnActivated(Value);
            }

            if (HasChanged())
            {
                OnChanged(Value);
            }

            if (Deactivate())
            {
                OnDeactivated(Value);
            }
        }

        /// <summary>
        /// Determines if the current <see cref="BaseAction{T}.Value"/> is the default <see cref="Vector2"/> value.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="BaseAction{T}.Value"/> is currently the default <see cref="Vector2"/> value.</returns>
        protected virtual bool DefaultValue()
        {
            return Value.Equals(default(Vector2));
        }

        /// <summary>
        /// Determines whether the action is transitioning it's value state.
        /// </summary>
        /// <returns><see langword="true"/> if the action value is changing.</returns>
        protected virtual bool IsActive()
        {
            return (!DefaultValue() && HasChanged());
        }

        /// <summary>
        /// Determines whether the action has just become active.
        /// </summary>
        /// <returns><see langword="true"/> if the action has just become active.</returns>
        protected virtual bool Activate()
        {
            return (!State && IsActive());
        }

        /// <summary>
        /// Determines whether the action has just become inactive.
        /// </summary>
        /// <returns><see langword="true"/> if the action has just become inactive.</returns>
        protected virtual bool Deactivate()
        {
            return (State && (DefaultValue() || !HasChanged()));
        }
    }

}