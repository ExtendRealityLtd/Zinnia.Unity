namespace Zinnia.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

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
        public Vector3UnityEvent DifferenceExtracted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the <see cref="Vector3"/> is extracted.
        /// </summary>
        public FloatUnityEvent DistanceExtracted = new FloatUnityEvent();

        /// <summary>
        /// Extracts the parts of the event data.
        /// </summary>
        /// <param name="eventData">The event data to extract from.</param>
        public virtual void Extract(ObjectDistanceComparator.EventData eventData)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            DifferenceExtracted?.Invoke(eventData.difference);
            DistanceExtracted?.Invoke(eventData.distance);
        }
    }
}