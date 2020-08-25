namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;

    /// <summary>
    /// Extracts and emits the <see cref="SurfaceData.CollisionData"/>.
    /// </summary>
    public class SurfaceDataCollisionDataExtractor : ValueExtractor<RaycastHit?, SurfaceData, SurfaceDataCollisionDataExtractor.UnityEvent, RaycastHit>
    {

        /// <summary>
        /// Defines an event with a <see cref="RaycastHit"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<RaycastHit> { }

        /// <inheritdoc />
        protected override RaycastHit? ExtractValue()
        {
            if (Source == null || Source.CollisionData.transform == null)
            {
                return null;
            }

            return Source.CollisionData;
        }

        /// <inheritdoc/>
        protected override bool InvokeResult(RaycastHit? data)
        {
            return InvokeEvent(data.GetValueOrDefault());
        }
    }
}