namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Process;

    /// <summary>
    /// Provides a basis for extracting properties from a <see cref="Transform"/>.
    /// </summary>
    /// <typeparam name="TElement">The element type to extract.</typeparam>
    /// <typeparam name="TEvent">The event to emit on extraction.</typeparam>
    public abstract class TransformPropertyExtractor<TElement, TEvent> : MonoBehaviour, IProcessable where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// The source of the <see cref="Transform"/> to extract from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// Determines whether to extract the local property or the world property.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool UseLocal { get; set; }

        /// <summary>
        /// The last extracted property value.
        /// </summary>
        public TElement LastExtractedValue { get; protected set; }

        /// <summary>
        /// Emitted when the property is extracted.
        /// </summary>
        [DocumentedByXml]
        public TEvent Extracted = new TEvent();

        /// <summary>
        /// Extracts each time the process is run in a moment.
        /// </summary>
        public virtual void Process()
        {
            DoExtract();
        }

        /// <summary>
        /// Extracts the property from the <see cref="Source"/> transform.
        /// </summary>
        /// <returns>The property value.</returns>
        [RequiresBehaviourState]
        public virtual TElement Extract()
        {
            DoExtract();
            return LastExtractedValue;
        }

        /// <summary>
        /// Extracts the property from the <see cref="Source"/> transform.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void DoExtract()
        {
            if (Source == null)
            {
                return;
            }

            LastExtractedValue = ExtractValue();
            Extracted?.Invoke(LastExtractedValue);
        }

        /// <summary>
        /// The mechanism for extracting the property value.
        /// </summary>
        /// <returns>The extracted value.</returns>
        protected abstract TElement ExtractValue();
    }

    /// <summary>
    /// Provides a basis for extracting <see cref="Vector3"/> properties from a <see cref="Transform"/>.
    /// </summary>
    public abstract class TransformVector3PropertyExtractor : TransformPropertyExtractor<Vector3, TransformVector3PropertyExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }
    }
}