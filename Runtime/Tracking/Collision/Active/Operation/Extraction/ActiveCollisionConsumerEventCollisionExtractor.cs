namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the <see cref="CollisionNotifier.EventData"/> data from a given <see cref="ActiveCollisionConsumer.UnityEvent"/>.
    /// </summary>
    public class ActiveCollisionConsumerEventCollisionExtractor : ValueExtractor<CollisionNotifier.EventData, ActiveCollisionConsumer.EventData, ActiveCollisionConsumerEventCollisionExtractor.UnityEvent, CollisionNotifier.EventData>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="CollisionNotifier.EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<CollisionNotifier.EventData> { }

        /// <inheritdoc />
        protected override CollisionNotifier.EventData ExtractValue()
        {
            return Source != null ? Source.CurrentCollision : null;
        }

        /// <inheritdoc />
        protected override bool InvokeResult(CollisionNotifier.EventData data)
        {
            return InvokeEvent(data);
        }
    }
}