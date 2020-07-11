namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the texture coordinate of collision from <see cref="RaycastHit"/>.
    /// </summary>
    public class RaycastHitTextureCoordExtractor : Vector2Extractor<RaycastHit, RaycastHitTextureCoordExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector2"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <inheritdoc />
        protected override Vector2? ExtractValue()
        {
            if (Source.transform == null)
            {
                return null;
            }

            return Source.textureCoord;
        }
    }
}