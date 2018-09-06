namespace VRTK.Core.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using VRTK.Core.Event;

    /// <summary>
    /// Extracts the forward source container from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierContainerExtractor : BaseGameObjectEmitter
    {
        /// <summary>
        /// Extracts the forward source container from the given notifier event data.
        /// </summary>
        /// <param name="notifier">The notifier event data to extract from.</param>
        /// <returns>The forward source container within the notifier.</returns>
        public virtual GameObject Extract(CollisionNotifier.EventData notifier)
        {
            if (notifier == null)
            {
                Result = null;
                return null;
            }

            Result = notifier.forwardSource?.gameObject;
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