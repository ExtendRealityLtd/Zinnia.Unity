namespace Zinnia.Cast.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Extracts and emits the <see cref="RaycastHit"/> of the collision from <see cref="PointsCast.EventData"/>.
    /// </summary>
    public class PointsCastEventDataRaycastHitExtractor : PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="RaycastHit"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<RaycastHit> { }

        /// <inheritdoc />
        protected override RaycastHit ExtractValue()
        {
            return CanExtract() ? (RaycastHit)Source.HitData : new RaycastHit();
        }

        /// <inheritdoc />
        protected override bool InvokeResult(RaycastHit data)
        {
            return CanExtract() ? InvokeEvent(data) : false;
        }
    }
}