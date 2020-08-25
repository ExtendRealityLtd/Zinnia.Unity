namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the <see cref="Collider"/> of the collision from <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitColliderExtractor : ValueExtractor<Collider, RaycastHit, RaycastHitColliderExtractor.UnityEvent, Collider>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Collider"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Collider> { }

        /// <inheritdoc />
        protected override Collider ExtractValue()
        {
            return Source.collider;
        }

        /// <inheritdoc/>
        protected override bool InvokeResult(Collider data)
        {
            return InvokeEvent(data);
        }
    }
}