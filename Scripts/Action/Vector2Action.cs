namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// The Vector2Action emits a Vector2 value.
    /// </summary>
    public class Vector2Action : BaseAction<Vector2>
    {
        /// <summary>
        /// The Vector2ActionUnityEvent emits an event with the specified type and the sender object.
        /// </summary>
        [Serializable]
        public class Vector2ActionUnityEvent : UnityEvent<Vector2, object>
        {
        };

        /// <summary>
        /// The Activated event is emitted when the action value changes into it's active state.
        /// </summary>
        public Vector2ActionUnityEvent Activated = new Vector2ActionUnityEvent();
        /// <summary>
        /// The Changed event is emitted when the action value changes from it's previous state.
        /// </summary>
        public Vector2ActionUnityEvent Changed = new Vector2ActionUnityEvent();
        /// <summary>
        /// The Deactivated event is emitted when the action value changes into it's inactive state.
        /// </summary>
        public Vector2ActionUnityEvent Deactivated = new Vector2ActionUnityEvent();

        /// <summary>
        /// The Receive method allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(Vector2 value, object sender = null)
        {
            previousValue = Value;
            Value = value;
            EmitEvents();
            State = IsActive();
        }

        /// <summary>
        /// The OnActivated Method is used to call the appropriate Activated event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected override void OnActivated(Vector2 value)
        {
            if (CanEmit())
            {
                Activated?.Invoke(value, this);
            }
        }

        /// <summary>
        /// The OnChanged Method is used to call the appropriate Changed event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected override void OnChanged(Vector2 value)
        {
            if (CanEmit())
            {
                Changed?.Invoke(value, this);
            }
        }

        /// <summary>
        /// The OnDeactivated Method is used to call the appropriate Deactivated event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected override void OnDeactivated(Vector2 value)
        {
            if (CanEmit())
            {
                Deactivated?.Invoke(value, this);
            }
        }

        /// <summary>
        /// The EmitEvents method attempts to emit each of the events if they are should be emitted.
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
        /// The DefaultValue method returns true if the current value is at the default value for the type.
        /// </summary>
        /// <returns></returns>
        protected virtual bool DefaultValue()
        {
            return Value.Equals(default(Vector2));
        }

        /// <summary>
        /// The IsActive method returns whether the action is transitioning it's value state.
        /// </summary>
        /// <returns>Returns `true` if the action value is changing.</returns>
        protected virtual bool IsActive()
        {
            return (!DefaultValue() && HasChanged());
        }

        /// <summary>
        /// The Activate method returns whether the action has just become active.
        /// </summary>
        /// <returns>Returns `true` if the action has just become active.</returns>
        protected virtual bool Activate()
        {
            return (!State && IsActive());
        }

        /// <summary>
        /// The Deactivate method returns whether the action has just become inactive.
        /// </summary>
        /// <returns>Returns `true` if the action has just become inactive.</returns>
        protected virtual bool Deactivate()
        {
            return (State && (DefaultValue() || !HasChanged()));
        }
    }

}