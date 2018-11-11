namespace VRTK.Core.Tracking.CameraRig
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Experimental.XR;

    /// <summary>
    /// Extracts play area dimensions as a <see cref="Vector3"/>.
    /// </summary>
    public class PlayAreaDimensionsExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines an event with a <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class Vector3UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// Emitted when the play area dimensions are extracted.
        /// </summary>
        public Vector3UnityEvent Extracted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the play area dimensions can't be extracted.
        /// </summary>
        public UnityEvent Failed = new UnityEvent();

        /// <summary>
        /// The extracted play area dimensions.
        /// </summary>
        public Vector3 Result { get; protected set; }

        /// <summary>
        /// Extracts the play area dimensions.
        /// </summary>
        /// <returns>The extracted play area dimensions.</returns>
        public virtual Vector3 Extract()
        {
            if (!isActiveAndEnabled)
            {
                return Result;
            }

            Vector3 dimensions;
            if (Boundary.TryGetDimensions(out dimensions))
            {
                Result = dimensions;
                Extracted?.Invoke(Result);
            }
            else
            {
                Failed?.Invoke();
            }

            return Result;
        }

        /// <summary>
        /// Extracts the play area dimensions.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }
    }
}
