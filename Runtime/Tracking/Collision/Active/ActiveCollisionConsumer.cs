namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Rule;
    using Zinnia.Extension;

    /// <summary>
    /// Consumes a published <see cref="ActiveCollisionPublisher"/>.
    /// </summary>
    public class ActiveCollisionConsumer : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="ActiveCollisionConsumer"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The publisher payload data that is being pushed to the consumer.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public ActiveCollisionPublisher.PayloadData Publisher { get; set; }
            /// <summary>
            /// The current collision data.
            /// </summary>
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public CollisionNotifier.EventData CurrentCollision { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.Publisher, source.CurrentCollision);
            }

            public EventData Set(ActiveCollisionPublisher.PayloadData publisher, CollisionNotifier.EventData currentCollision)
            {
                Publisher = publisher;
                CurrentCollision = currentCollision;
                return this;
            }

            public void Clear()
            {
                Set(default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// The highest level container of the consumer to allow for nested consumers.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Container { get; set; }

        /// <summary>
        /// Determines whether to consume the received call from specific publishers.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer PublisherValidity { get; set; }

        /// <summary>
        /// The publisher payload that was last received from.
        /// </summary>
        public ActiveCollisionPublisher.PayloadData PublisherSource { get; protected set; }

        /// <summary>
        /// The current active collision data from the broadcaster.
        /// </summary>
        public CollisionNotifier.EventData ActiveCollision { get; protected set; }

        /// <summary>
        /// Emitted when the publisher call has been consumed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Consumed = new UnityEvent();
        /// <summary>
        /// Emitted when the consumer is cleared.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Cleared = new UnityEvent();

        /// <summary>
        /// The event data emitted when collisions are consumed.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Consumes data from a from a <see cref="ActiveCollisionPublisher"/>.
        /// </summary>
        /// <param name="publisher">The publisher payload data.</param>
        /// <param name="currentCollision">The current collision within published data.</param>
        [RequiresBehaviourState]
        public virtual void Consume(ActiveCollisionPublisher.PayloadData publisher, CollisionNotifier.EventData currentCollision)
        {
            if (!PublisherValidity.Accepts(publisher.PublisherContainer))
            {
                return;
            }

            PublisherSource = publisher;
            ActiveCollision = currentCollision;
            Consumed?.Invoke(eventData.Set(PublisherSource, currentCollision));
        }

        /// <summary>
        /// Clears the previously consumed data.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Clear()
        {
            Cleared?.Invoke(eventData.Set(PublisherSource, ActiveCollision));
            PublisherSource = null;
            ActiveCollision = null;
        }
    }
}