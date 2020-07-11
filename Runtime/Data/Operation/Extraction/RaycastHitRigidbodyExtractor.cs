namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the <see cref="Rigidbody"/> of the collision from <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitRigidbodyExtractor : ValueExtractor<Rigidbody, RaycastHit, RaycastHitRigidbodyExtractor.UnityEvent, Rigidbody>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Rigidbody"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Rigidbody> { }

        /// <inheritdoc />
        protected override Rigidbody ExtractValue()
        {
            return Source.rigidbody;
        }

        /// <inheritdoc/>
        protected override bool InvokeResult(Rigidbody data)
        {
            return InvokeEvent(data);
        }
    }
}