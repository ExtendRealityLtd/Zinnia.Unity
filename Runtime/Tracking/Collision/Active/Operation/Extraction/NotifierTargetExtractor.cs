namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> of the <see cref="Collider"/> from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierTargetExtractor : GameObjectExtractor<CollisionNotifier.EventData, NotifierTargetExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            Transform containingTransform = Source != null ? Source.ColliderData.GetContainingTransform() : null;
            return containingTransform != null ? containingTransform.gameObject : null;
        }
    }
}