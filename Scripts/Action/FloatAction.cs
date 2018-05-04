namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// The FloatAction emits a float value.
    /// </summary>
    public class FloatAction : BaseAction<float>
    {
        /// <summary>
        /// The FloatActionUnityEvent emits an event with the specified type and the sender object.
        /// </summary>
        [Serializable]
        public class FloatActionUnityEvent : UnityEvent<float, object>
        {
        };

        /// <summary>
        /// The Activated event is emitted when the action value changes into it's active state.
        /// </summary>
        public FloatActionUnityEvent Activated = new FloatActionUnityEvent();
        /// <summary>
        /// The Changed event is emitted when the action value changes from it's previous state.
        /// </summary>
        public FloatActionUnityEvent Changed = new FloatActionUnityEvent();
        /// <summary>
        /// The Deactivated event is emitted when the action value changes into it's inactive state.
        /// </summary>
        public FloatActionUnityEvent Deactivated = new FloatActionUnityEvent();

        /// <summary>
        /// The Receive method allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(float value, object sender = null)
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
        protected override void OnActivated(float value)
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
        protected override void OnChanged(float value)
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
        protected override void OnDeactivated(float value)
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
            return (Value == default(float));
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