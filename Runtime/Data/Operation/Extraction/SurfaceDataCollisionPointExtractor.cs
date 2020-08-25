namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extracts and emits the point of collision from <see cref="SurfaceData"/>.
    /// </summary>
    [Obsolete("Use `SurfaceDataCollisionDataExtractor -> RaycastHitPointExtractor` combination instead.")]
    public class SurfaceDataCollisionPointExtractor : Vector3Extractor<SurfaceData, SurfaceDataCollisionPointExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <inheritdoc />
        protected override Vector3? ExtractValue()
        {
            if (Source == null || Source.CollisionData.transform == null)
            {
                return null;
            }

            return Source.CollisionData.point;
        }
    }
}