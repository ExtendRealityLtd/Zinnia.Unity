namespace Zinnia.Tracking.Collision.Active
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Rule;

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

            /// <inheritdoc />
            public override string ToString()
            {
                string[] titles = new string[]
                {
                "Publisher",
                "CurrentCollision"
                };

                object[] values = new object[]
                {
                Publisher,
                CurrentCollision
                };

                return StringExtensions.FormatForToString(titles, values);
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
        public class UnityEvent : UnityEvent<EventData> { }

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
        /// The current highest level container of the consumer to allow for nested consumers.
        /// </summary>
        public GameObject ConsumerContainer { get; protected set; }

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
        /// <param name="publisherPayload">The publisher payload data.</param>
        /// <param name="currentCollision">The current collision within published data.</param>
        /// <returns>Whether the consumption was allowed and successful.</returns>
        [RequiresBehaviourState]
        public virtual bool Consume(ActiveCollisionPublisher.PayloadData publisherPayload, CollisionNotifier.EventData currentCollision)
        {
            if (!PublisherValidity.Accepts(publisherPayload.PublisherContainer))
            {
                return false;
            }

            PublisherSource = publisherPayload;
            ActiveCollision = currentCollision;
            ConsumerContainer = currentCollision != null ? currentCollision.ColliderData.GetContainingTransform().TryGetGameObject() : null;
            Consumed?.Invoke(eventData.Set(PublisherSource, currentCollision));
            return true;
        }

        /// <summary>
        /// Clears the previously consumed data.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Clear()
        {
            if (PublisherSource != null && PublisherSource.Publisher != null)
            {
                PublisherSource.Publisher.UnregisterRegisteredConsumer(this);
            }
            Cleared?.Invoke(eventData.Set(PublisherSource, ActiveCollision));
            PublisherSource = null;
            ActiveCollision = null;
        }
    }
}