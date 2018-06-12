namespace VRTK.Core.Action
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="bool"/> value.
    /// </summary>
    public class BooleanAction : BaseAction<bool>
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/> state and initiator <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class BooleanActionUnityEvent : UnityEvent<bool, object>
        {
        }

        /// <summary>
        /// Emitted when the action value changes into it's active state.
        /// </summary>
        public BooleanActionUnityEvent Activated = new BooleanActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes from it's previous state.
        /// </summary>
        public BooleanActionUnityEvent Changed = new BooleanActionUnityEvent();
        /// <summary>
        /// Emitted when the action value changes into it's inactive state.
        /// </summary>
        public BooleanActionUnityEvent Deactivated = new BooleanActionUnityEvent();

        /// <inheritdoc />
        public override void Receive(bool value, object initiator = null)
        {
            previousValue = Value;
            Value = value;
            State = Value;
            if (State)
            {
                OnActivated(true, initiator);
            }
            else
            {
                OnDeactivated(false, initiator);
            }

            if (HasChanged())
            {
                OnChanged(value, initiator);
            }
        }

        /// <inheritdoc />
        protected override void OnActivated(bool value, object sender)
        {
            State = value;
            if (CanEmit())
            {
                Activated?.Invoke(value, sender);
            }
        }

        /// <inheritdoc />
        protected override void OnChanged(bool value, object sender)
        {
            if (CanEmit())
            {
                Changed?.Invoke(value, sender);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivated(bool value, object sender)
        {
            State = value;
            if (CanEmit())
            {
                Deactivated?.Invoke(value, sender);
            }
        }
    }
}