namespace VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;

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
            /// The publisher that is pushing to the consumer.
            /// </summary>
            public ActiveCollisionPublisher publisher;
            /// <summary>
            /// The current collision data.
            /// </summary>
            public CollisionNotifier.EventData currentCollision;

            public EventData Set(EventData source)
            {
                return Set(source.publisher, source.currentCollision);
            }

            public EventData Set(ActiveCollisionPublisher publisher, CollisionNotifier.EventData currentCollision)
            {
                this.publisher = publisher;
                this.currentCollision = currentCollision;
                return this;
            }

            public void Clear()
            {
                Set(default(ActiveCollisionPublisher), default(CollisionNotifier.EventData));
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
        /// Determines whether to consume the received call from specific publishers.
        /// </summary>
        [Tooltip("Determines whether to consume the received call from specific publishers.")]
        public ExclusionRule publisherValidity;

        /// <summary>
        /// The publisher that last initiated the receive.
        /// </summary>
        public ActiveCollisionPublisher PublisherSource
        {
            get;
            protected set;
        }

        /// <summary>
        /// The current collision data from the broadcaster.
        /// </summary>
        public CollisionNotifier.EventData CurrentCollision
        {
            get;
            protected set;
        }

        /// <summary>
        /// Emitted when the publisher call has been consumed.
        /// </summary>
        public UnityEvent Consumed = new UnityEvent();
        /// <summary>
        /// Emitted when the consumer is cleared.
        /// </summary>
        public UnityEvent Cleared = new UnityEvent();

        protected EventData eventData = new EventData();

        /// <summary>
        /// Consumes data from a from a <see cref="ActiveCollisionPublisher"/>.
        /// </summary>
        /// <param name="publisher">The publisher of the data.</param>
        /// <param name="currentCollision">The current collision within published data.</param>
        public virtual void Consume(ActiveCollisionPublisher publisher, CollisionNotifier.EventData currentCollision)
        {
            if (!isActiveAndEnabled || ExclusionRule.ShouldExclude(publisher.gameObject, publisherValidity))
            {
                return;
            }

            PublisherSource = publisher;
            CurrentCollision = currentCollision;
            Consumed?.Invoke(eventData.Set(PublisherSource, currentCollision));
        }

        /// <summary>
        /// Clears the previously consumed data.
        /// </summary>
        public virtual void Clear()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Cleared?.Invoke(eventData.Set(PublisherSource, CurrentCollision));
            PublisherSource = null;
            CurrentCollision = null;
        }
    }
}