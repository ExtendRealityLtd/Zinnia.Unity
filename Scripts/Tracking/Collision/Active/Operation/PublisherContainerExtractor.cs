namespace VRTK.Core.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Extracts the source container from a given <see cref="ActiveCollisionPublisher"/>.
    /// </summary>
    public class PublisherContainerExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the source container <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the source container extracted from the publisher.
        /// </summary>
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The currently extracted source container.
        /// </summary>
        public GameObject SourceContainer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Extracts the source container from the given publisher.
        /// </summary>
        /// <param name="publisher">The publisher to extract from.</param>
        /// <returns>The source container within the publisher.</returns>
        public virtual GameObject Extract(ActiveCollisionPublisher publisher)
        {
            if (!isActiveAndEnabled || publisher == null)
            {
                SourceContainer = null;
                return null;
            }

            SourceContainer = publisher.sourceContainer;
            Extracted?.Invoke(SourceContainer);
            return SourceContainer;
        }

        /// <summary>
        /// Extracts the source container from the given publisher contained within the <see cref="ActiveCollisionConsumer.EventData"/>.
        /// </summary>
        /// <param name="eventData">The event data to extract the source container from.</param>
        /// <returns>The source container within the publisher.</returns>
        public virtual GameObject Extract(ActiveCollisionConsumer.EventData eventData)
        {
            return Extract(eventData?.publisher);
        }

        /// <summary>
        /// Extracts the source container from the given publisher.
        /// </summary>
        /// <param name="publisher">The publisher to extract from.</param>
        public virtual void DoExtract(ActiveCollisionPublisher publisher)
        {
            Extract(publisher);
        }

        /// <summary>
        /// Extracts the source container from the given publisher contained within the <see cref="ActiveCollisionConsumer.EventData"/>.
        /// </summary>
        /// <param name="eventData">The event data to extract the source container from.</param>
        public virtual void DoExtract(ActiveCollisionConsumer.EventData eventData)
        {
            Extract(eventData?.publisher);
        }
    }
}