namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using UnityEngine;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the forward source container from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierContainerExtractor : GameObjectExtractor
    {
        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="eventData">The notifier event data to extract from.</param>
        /// <returns>The forward source container within the notifier.</returns>
        public virtual GameObject Extract(CollisionNotifier.EventData eventData)
        {
            if (eventData == null || eventData.ForwardSource == null)
            {
                Result = null;
                return null;
            }

            Result = eventData.ForwardSource.gameObject;
            return base.Extract();
        }

        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="eventData">The notifier event data to extract from.</param>
        public virtual void DoExtract(CollisionNotifier.EventData eventData)
        {
            Extract(eventData);
        }
    }
}