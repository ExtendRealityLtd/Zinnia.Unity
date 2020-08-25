namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the source container from a given <see cref="ActiveCollisionPublisher.PayloadData"/>.
    /// </summary>
    public class PublisherContainerExtractor : GameObjectExtractor<ActiveCollisionPublisher.PayloadData, PublisherContainerExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <summary>
        /// Extracts the source container from the given publisher contained within the <see cref="ActiveCollisionConsumer.EventData"/>.
        /// </summary>
        /// <param name="eventData">The event data to extract the source container from.</param>
        /// <returns>The source container within the publisher.</returns>
        public virtual GameObject Extract(ActiveCollisionConsumer.EventData eventData)
        {
            return Extract(eventData?.Publisher);
        }

        /// <summary>
        /// Extracts the source container from the given publisher contained within the <see cref="ActiveCollisionConsumer.EventData"/>.
        /// </summary>
        /// <param name="eventData">The event data to extract the source container from.</param>
        public virtual void DoExtract(ActiveCollisionConsumer.EventData eventData)
        {
            Extract(eventData?.Publisher);
        }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            return Source != null ? Source.SourceContainer : null;
        }

    }
}