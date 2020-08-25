namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the point of collision from <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitPointExtractor : Vector3Extractor<RaycastHit, RaycastHitPointExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <inheritdoc />
        protected override Vector3? ExtractValue()
        {
            if (Source.transform == null)
            {
                return null;
            }

            return Source.point;
        }
    }
}