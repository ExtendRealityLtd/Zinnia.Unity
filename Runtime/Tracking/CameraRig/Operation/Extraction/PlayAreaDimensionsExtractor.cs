namespace Zinnia.Tracking.CameraRig.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Experimental.XR;
    using System;
    using Malimbe.XmlDocumentationAttribute;

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
        [DocumentedByXml]
        public Vector3UnityEvent Extracted = new Vector3UnityEvent();
        /// <summary>
        /// Emitted when the play area dimensions can't be extracted.
        /// </summary>
        [DocumentedByXml]
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

            if (Boundary.TryGetDimensions(out Vector3 dimensions))
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
