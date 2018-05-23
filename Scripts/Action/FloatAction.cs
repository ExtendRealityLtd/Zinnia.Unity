namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="float"/> value.
    /// </summary>
    public class FloatAction : BaseAction<float>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/> value and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class FloatActionUnityEvent : UnityEvent<float, object>
        {
        }

        /// <summary>
        /// Emitted when the action value changes into it's active state.
        /// </summary>
        public FloatActionUnityEvent Activated = new FloatActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes from it's previous state.
        /// </summary>
        public FloatActionUnityEvent Changed = new FloatActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes into it's inactive state.
        /// </summary>
        public FloatActionUnityEvent Deactivated = new FloatActionUnityEvent();

        /// <inheritdoc />
        public override void Receive(float value, object sender = null)
        {
            previousValue = Value;
            Value = value;
            EmitEvents();
            State = IsActive();
        }

        /// <inheritdoc />
        protected override void OnActivated(float value)
        {
            if (CanEmit())
            {
                Activated?.Invoke(value, this);
            }
        }

        /// <inheritdoc />
        protected override void OnChanged(float value)
        {
            if (CanEmit())
            {
                Changed?.Invoke(value, this);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivated(float value)
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
        /// Determines if the current <see cref="BaseAction{T}.Value"/> is the default float value.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="BaseAction{T}.Value"/> is currently the default float value.</returns>
        protected virtual bool DefaultValue()
        {
            return (Value.ApproxEquals(default(float)));
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