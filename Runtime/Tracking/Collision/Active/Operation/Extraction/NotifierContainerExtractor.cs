namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts the forward source container from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierContainerExtractor : GameObjectExtractor<CollisionNotifier.EventData, NotifierContainerExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            return Source != null && Source.ForwardSource != null ? Source.ForwardSource.gameObject : null;
        }
    }
}