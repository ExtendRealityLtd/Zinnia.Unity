namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the <see cref="GameObject"/> associated with the colliding transform from the <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitTransformExtractor : GameObjectExtractor<RaycastHit, RaycastHitTransformExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            return Source.transform != null ? Source.transform.gameObject : null;
        }
    }
}