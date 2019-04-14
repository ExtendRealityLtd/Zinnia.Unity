namespace Zinnia.Tracking.Follow.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Extracts the parts of <see cref="ObjectDistanceComparator.EventData"/> and emits them in separate events.
    /// </summary>
    public class ObjectDistanceComparatorEventDataExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class Vector3UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class FloatUnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// Emitted when the <see cref="Vector3"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public Vector3UnityEvent DifferenceExtracted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Vector3"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public FloatUnityEvent DistanceExtracted = new FloatUnityEvent();

        /// <summary>
        /// The extracted <see cref="Vector3"/> difference.
        /// </summary>
        public Vector3 DifferenceResult { get; protected set; }
        /// <summary>
        /// The extracted <see cref="float"/> distance.
        /// </summary>
        public float DistanceResult { get; protected set; }

        /// <summary>
        /// Extracts the parts of the event data.
        /// </summary>
        /// <param name="eventData">The event data to extract from.</param>
        [RequiresBehaviourState]
        public virtual void Extract(ObjectDistanceComparator.EventData eventData)
        {
            if (eventData == null)
            {
                return;
            }

            DifferenceResult = eventData.CurrentDifference;
            DifferenceExtracted?.Invoke(DifferenceResult);
            DistanceResult = eventData.CurrentDistance;
            DistanceExtracted?.Invoke(DistanceResult);
        }
    }
}