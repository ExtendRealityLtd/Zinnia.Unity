namespace Zinnia.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using Zinnia.Event;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> of the <see cref="Collider"/> from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierTargetExtractor : GameObjectEmitter
    {
        /// <summary>
        /// Extracts the <see cref="GameObject"/> of the <see cref="Collider"/> from the given <see cref="CollisionNotifier.EventData"/>.
        /// </summary>
        /// <param name="eventData">The notifier event data to extract from.</param>
        /// <returns>The <see cref="GameObject"/> of the <see cref="Collider"/> within the event data.</returns>
        public virtual GameObject Extract(CollisionNotifier.EventData eventData)
        {
            if (eventData == null || eventData.ColliderData == null)
            {
                Result = null;
                return null;
            }

            Result = eventData.ColliderData.gameObject;
            return base.Extract();
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/> of the <see cref="Collider"/> from the given <see cref="CollisionNotifier.EventData"/>.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        public virtual void DoExtract(CollisionNotifier.EventData notifier)
        {
            Extract(notifier);
        }
    }
}