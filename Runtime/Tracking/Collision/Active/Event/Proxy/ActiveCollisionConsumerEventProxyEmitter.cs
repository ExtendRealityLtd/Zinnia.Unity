namespace Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Tracking.Collision.Active;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionConsumer.EventData"/> payload whenever the Receive method is called.
    /// </summary>
    public class ActiveCollisionConsumerEventProxyEmitter : RestrictableSingleEventProxyEmitter<ActiveCollisionConsumer.EventData, ActiveCollisionConsumerEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionConsumer.EventData>
        {
        }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload?.Publisher?.PublisherContainer;
        }
    }
}