namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// The BooleanAction emits a boolean value.
    /// </summary>
    public class BooleanAction : BaseAction<bool>
    {
        /// <summary>
        /// The BooleanActionUnityEvent emits an event with the specified type and the sender object.
        /// </summary>
        [Serializable]
        public class BooleanActionUnityEvent : UnityEvent<bool, object>
        {
        };

        /// <summary>
        /// The Activated event is emitted when the action value changes into it's active state.
        /// </summary>
        public BooleanActionUnityEvent Activated;
        /// <summary>
        /// The Changed event is emitted when the action value changes from it's previous state.
        /// </summary>
        public BooleanActionUnityEvent Changed;
        /// <summary>
        /// The Deactivated event is emitted when the action value changes into it's inactive state.
        /// </summary>
        public BooleanActionUnityEvent Deactivated;

        /// <summary>
        /// The Receive method allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(bool value, object sender = null)
        {
            previousValue = Value;
            Value = value;
            state = Value;
            if (state)
            {
                OnActivated(true);
            }
            else
            {
                OnDeactivated(false);
            }

            if (HasChanged())
            {
                OnChanged(value);
            }
        }

        /// <summary>
        /// The OnActivated Method is used to call the appropriate Activated event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected override void OnActivated(bool value)
        {
            state = value;
            if (CanEmit())
            {
                Activated?.Invoke(value, this);
            }
        }

        /// <summary>
        /// The OnChanged Method is used to call the appropriate Changed event.
        /// </summary>
        /// <param name="value">The value to pass to the event.</param>
        protected override void OnChanged(bool value)
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
        protected override void OnDeactivated(bool value)
        {
            state = value;
            if (CanEmit())
            {
                Deactivated?.Invoke(value, this);
            }
        }
    }
}