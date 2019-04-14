namespace Zinnia.Tracking.Modification.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extracts the <see cref="TransformData"/> for the Source and Target contained within the <see cref="TransformPropertyApplier.EventData"/>.
    /// </summary>
    public class TransformPropertyApplierEventDataExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="TransformData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TransformData>
        {
        }

        /// <summary>
        /// Emitted when the <see cref="TransformPropertyApplier.EventData.EventSource"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent SourceExtracted = new UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="TransformPropertyApplier.EventData.EventTarget"/> is extracted.
        /// </summary>
        [DocumentedByXml]
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
        [RequiresBehaviourState]
        public virtual void Extract(TransformPropertyApplier.EventData eventData)
        {
            if (eventData == null)
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