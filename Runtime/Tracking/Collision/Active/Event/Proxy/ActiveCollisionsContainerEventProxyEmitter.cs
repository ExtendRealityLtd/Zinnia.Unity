namespace Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Event.Proxy;
    using Zinnia.Tracking.Collision.Active;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionsContainer.EventData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class ActiveCollisionsContainerEventProxyEmitter : SingleEventProxyEmitter<ActiveCollisionsContainer.EventData, ActiveCollisionsContainerEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionsContainer.EventData>
        {
        }
    }
}