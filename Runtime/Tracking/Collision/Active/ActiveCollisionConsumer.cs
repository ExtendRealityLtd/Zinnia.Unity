namespace Zinnia.Tracking.Collision.Active
{
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
            [Tooltip("The publisher payload data that is being pushed to the consumer.")]
            [SerializeField]
            private ActiveCollisionPublisher.PayloadData publisher;
            /// <summary>
            /// The publisher payload data that is being pushed to the consumer.
            /// </summary>
            public ActiveCollisionPublisher.PayloadData Publisher
            {
                get
                {
                    return publisher;
                }
                set
                {
                    publisher = value;
                }
            }
            [Tooltip("The current collision data.")]
            [SerializeField]
            private CollisionNotifier.EventData currentCollision;
            /// <summary>
            /// The current collision data.
            /// </summary>
            public CollisionNotifier.EventData CurrentCollision
            {
                get
                {
                    return currentCollision;
                }
                set
                {
                    currentCollision = value;
                }
            }

            /// <summary>
            /// Clears <see cref="Publisher"/>.
            /// </summary>
            public virtual void ClearPublisher()
            {
                Publisher = default;
            }

            /// <summary>
            /// Clears <see cref="CurrentCollision"/>.
            /// </summary>
            public virtual void ClearCurrentCollision()
            {
                CurrentCollision = default;
            }

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

        [Tooltip("The highest level container of the consumer to allow for nested consumers.")]
        [SerializeField]
        private GameObject container;
        /// <summary>
        /// The highest level container of the consumer to allow for nested consumers.
        /// </summary>
        public GameObject Container
        {
            get
            {
                return container;
            }
            set
            {
                container = value;
            }
        }

        [Tooltip("Determines whether to consume the received call from specific publishers.")]
        [SerializeField]
        private RuleContainer publisherValidity;
        /// <summary>
        /// Determines whether to consume the received call from specific publishers.
        /// </summary>
        public RuleContainer PublisherValidity
        {
            get
            {
                return publisherValidity;
            }
            set
            {
                publisherValidity = value;
            }
        }

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
        public UnityEvent Consumed = new UnityEvent();
        /// <summary>
        /// Emitted when the consumer is cleared.
        /// </summary>
        public UnityEvent Cleared = new UnityEvent();

        /// <summary>
        /// The event data emitted when collisions are consumed.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Clears <see cref="Container"/>.
        /// </summary>
        public virtual void ClearContainer()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Container = default;
        }

        /// <summary>
        /// Clears <see cref="PublisherValidity"/>.
        /// </summary>
        public virtual void ClearPublisherValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PublisherValidity = default;
        }

        /// <summary>
        /// Consumes data from a from a <see cref="ActiveCollisionPublisher"/>.
        /// </summary>
        /// <param name="publisherPayload">The publisher payload data.</param>
        /// <param name="currentCollision">The current collision within published data.</param>
        /// <returns>Whether the consumption was allowed and successful.</returns>
        public virtual bool Consume(ActiveCollisionPublisher.PayloadData publisherPayload, CollisionNotifier.EventData currentCollision)
        {
            if (!this.IsValidState() || !PublisherValidity.Accepts(publisherPayload.PublisherContainer))
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
        public virtual void Clear()
        {
            if (!this.IsValidState())
            {
                return;
            }

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