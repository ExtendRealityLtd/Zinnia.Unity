namespace Zinnia.Tracking.CameraRig.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
#if UNITY_2020_2_OR_NEWER
#else
    using UnityEngine.Experimental.XR;
#endif
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts play area dimensions as a <see cref="Vector3"/>.
    /// </summary>
    public class PlayAreaDimensionsExtractor : Vector3Extractor<GameObject, PlayAreaDimensionsExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines an event with a <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <inheritdoc />
        protected override Vector3? ExtractValue()
        {
#if UNITY_2020_2_OR_NEWER
            Debug.LogWarning("PlayAreaDimensionsExtractor is not supported on Unity 2020.2.");
            return null;
#else
            if (Boundary.TryGetDimensions(out Vector3 dimensions))
            {
                return dimensions;
            }
            return null;
#endif
        }
    }
}
