namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts and emits the <see cref="Rigidbody"/> found in relation to the <see cref="Source"/>.
    /// </summary>
    public class ComponentRigidbodyExtractor : ComponentExtractor<Rigidbody, ComponentRigidbodyExtractor.UnityEvent, Rigidbody>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Rigidbody"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Rigidbody> { }

        /// <inheritdoc />
        protected override Rigidbody ExtractValue()
        {
            return Source != null
                ? Source.gameObject.TryGetComponent<Rigidbody>(
                    (SearchAlsoOn & SearchCriteria.IncludeDescendants) != 0,
                    (SearchAlsoOn & SearchCriteria.IncludeAncestors) != 0
                    )
                : null;
        }

        /// <inheritdoc/>
        protected override bool InvokeResult(Rigidbody data)
        {
            return InvokeEvent(data);
        }
    }
}
