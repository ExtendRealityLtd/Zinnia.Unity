namespace VRTK.Core.Data.Operation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Process;

    /// <summary>
    /// Provides a basis for extracting properties from a <see cref="Transform"/>.
    /// </summary>
    /// <typeparam name="TElement">The element type to extract.</typeparam>
    /// <typeparam name="TEvent">The event to emit on extraction.</typeparam>
    public abstract class TransformPropertyExtractor<TElement, TEvent> : MonoBehaviour, IProcessable
        where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// The source of the <see cref="Transform"/> to extract from.
        /// </summary>
        [Tooltip("The source of the Transform to extract from.")]
        public GameObject source;
        /// <summary>
        /// Determines whether to extract the local property or the world property.
        /// </summary>
        [Tooltip("Determines whether to extract the local property or the world property.")]
        public bool useLocal;

        /// <summary>
        /// The last extracted property value.
        /// </summary>
        public TElement LastExtractedValue
        {
            get;
            protected set;
        }

        /// <summary>
        /// Emitted when the property is extracted.
        /// </summary>
        public TEvent Extracted = new TEvent();

        /// <summary>
        /// Extracts each time the process is run in a moment.
        /// </summary>
        public virtual void Process()
        {
            DoExtract();
        }

        /// <summary>
        /// Extracts the property from the <see cref="source"/> transform.
        /// </summary>
        /// <returns>The property value.</returns>
        public virtual TElement Extract()
        {
            DoExtract();
            return LastExtractedValue;
        }

        /// <summary>
        /// Extracts the property from the <see cref="source"/> transform.
        /// </summary>
        public virtual void DoExtract()
        {
            if (source == null || !isActiveAndEnabled)
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