namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Provides the basis for emitting a <see cref="Vector3"/>.
    /// </summary>
    public abstract class Vector3Extractor : MonoBehaviour
    {
        /// <summary>
        /// Defines an event with a <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// Emitted when the <see cref="Vector3"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="Vector3"/>.
        /// </summary>
        public Vector3? Result { get; protected set; }

        /// <summary>
        /// Extracts the <see cref="Vector3"/>.
        /// </summary>
        /// <returns>The extracted <see cref="Vector3"/>.</returns>
        public virtual Vector3? Extract()
        {
            if (!isActiveAndEnabled || Result == null)
            {
                Result = null;
                return null;
            }

            Extracted?.Invoke((Vector3)Result);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="Vector3"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }
    }
}