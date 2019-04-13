namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using UnityEngine;
    using Zinnia.Event;

    /// <summary>
    /// Extracts the forward source container from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierContainerExtractor : GameObjectEmitter
    {
        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        /// <returns>The forward source container within the notifier.</returns>
        public virtual GameObject Extract(CollisionNotifier.EventData notifier)
        {
            if (notifier == null || notifier.ForwardSource == null)
            {
                Result = null;
                return null;
            }

            Result = notifier.ForwardSource.gameObject;
            return base.Extract();
        }

        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        public virtual void DoExtract(CollisionNotifier.EventData notifier)
        {
            Extract(notifier);
        }
    }
}