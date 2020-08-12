namespace Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Event.Proxy;
    using Zinnia.Tracking.Collision.Active;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionPublisher.PayloadData"/> payload whenever the Receive method is called.
    /// </summary>
    public class ActiveCollisionPublisherEventProxyEmitter : SingleEventProxyEmitter<ActiveCollisionPublisher.PayloadData, ActiveCollisionPublisherEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionPublisher.PayloadData> { }
    }
}