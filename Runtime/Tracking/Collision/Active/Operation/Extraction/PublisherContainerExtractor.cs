namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using UnityEngine;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the source container from a given <see cref="ActiveCollisionPublisher.PayloadData"/>.
    /// </summary>
    public class PublisherContainerExtractor : GameObjectExtractor
    {
        /// <summary>
        /// Extracts the source container from the given publisher payload data.
        /// </summary>
        /// <param name="publisher">The publisher payload data to extract from.</param>
        /// <returns>The source container within the publisher.</returns>
        public virtual GameObject Extract(ActiveCollisionPublisher.PayloadData publisher)
        {
            if (publisher == null || publisher.SourceContainer == null)
            {
                Result = null;
                return null;
            }

            Result = publisher.SourceContainer;
            return base.Extract();
        }

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
        /// Extracts the source container from the given publisher payload data.
        /// </summary>
        /// <param name="publisher">The publisher payload data to extract from.</param>
        public virtual void DoExtract(ActiveCollisionPublisher.PayloadData publisher)
        {
            Extract(publisher);
        }

        /// <summary>
        /// Extracts the source container from the given publisher contained within the <see cref="ActiveCollisionConsumer.EventData"/>.
        /// </summary>
        /// <param name="eventData">The event data to extract the source container from.</param>
        public virtual void DoExtract(ActiveCollisionConsumer.EventData eventData)
        {
            Extract(eventData?.Publisher);
        }
    }
}