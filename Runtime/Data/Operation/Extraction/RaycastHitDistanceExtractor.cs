namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the distance of the collision from <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitDistanceExtractor : FloatExtractor<RaycastHit, RaycastHitDistanceExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <inheritdoc />
        protected override float? ExtractValue()
        {
            if (Source.transform == null)
            {
                return null;
            }

            return Source.distance;
        }
    }
}