namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Provides the basis for emitting the <see cref="GameObject"/> that any concrete implementation is residing on.
    /// </summary>
    public abstract class GameObjectExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the <see cref="GameObject"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="GameObject"/>.
        /// </summary>
        public GameObject Result { get; protected set; }

        /// <summary>
        /// Extracts the <see cref="GameObject"/>/
        /// </summary>
        /// <returns>The extracted <see cref="GameObject"/>.</returns>
        public virtual GameObject Extract()
        {
            if (!isActiveAndEnabled || Result == null)
            {
                Result = null;
                return null;
            }

            Extracted?.Invoke(Result);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="GameObject"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }
    }
}