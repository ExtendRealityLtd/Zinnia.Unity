namespace VRTK.Core.Event
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Tracking.Collision.Active;
    using VRTK.Core.Rule;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a UnityEvent with a <see cref="ActiveCollisionConsumer.EventData"/> payload whenever the Receive method is called.
    /// </summary>
    public class ActiveCollisionConsumerEventProxyEmitter : SingleEventProxyEmitter<ActiveCollisionConsumer.EventData, ActiveCollisionConsumerEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActiveCollisionConsumer.EventData>
        {
        }

        /// <summary>
        /// Determines whether to consume the received call from specific publishers.
        /// </summary>
        [Tooltip("Determines whether to consume the received call from specific publishers.")]
        public RuleContainer publisherValidity;

        /// <inheritdoc />
        protected override bool IsValid()
        {
            return (base.IsValid() && publisherValidity.Accepts(Payload?.publisher?.PublisherContainer));
        }
    }
}