namespace Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Tracking.Collision.Active;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionRegisteredConsumerContainer.EventData"/> payload whenever the Receive method is called.
    /// </summary>
    public class ActiveCollisionRegisteredConsumerContainerEventProxyEmitter : RestrictableSingleEventProxyEmitter<ActiveCollisionRegisteredConsumerContainer.EventData, ActiveCollisionRegisteredConsumerContainerEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionRegisteredConsumerContainer.EventData> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload?.Consumer;
        }
    }
}