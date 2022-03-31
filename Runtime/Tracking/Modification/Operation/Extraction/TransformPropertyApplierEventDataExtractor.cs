namespace Zinnia.Tracking.Modification.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts the <see cref="TransformData"/> for the Source and Target contained within the <see cref="TransformPropertyApplier.EventData"/>.
    /// </summary>
    public class TransformPropertyApplierEventDataExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="TransformData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TransformData> { }

        /// <summary>
        /// Emitted when the <see cref="TransformPropertyApplier.EventData.EventSource"/> is extracted.
        /// </summary>
        public UnityEvent SourceExtracted = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="TransformPropertyApplier.EventData.EventTarget"/> is extracted.
        /// </summary>
        public UnityEvent TargetExtracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="TransformPropertyApplier.EventData.EventSource"/>.
        /// </summary>
        public TransformData SourceResult { get; protected set; }
        /// <summary>
        /// The extracted <see cref="TransformPropertyApplier.EventData.EventTarget"/>.
        /// </summary>
        public TransformData TargetResult { get; protected set; }

        /// <summary>
        /// Extracts the source and target from the event data.
        /// </summary>
        /// <param name="eventData">The event data to extract from.</param>
        public virtual void Extract(TransformPropertyApplier.EventData eventData)
        {
            if (!this.IsValidState() || eventData == null)
            {
                return;
            }

            SourceResult = eventData.EventSource;
            SourceExtracted?.Invoke(SourceResult);
            TargetResult = eventData.EventTarget;
            TargetExtracted?.Invoke(TargetResult);
        }
    }
}