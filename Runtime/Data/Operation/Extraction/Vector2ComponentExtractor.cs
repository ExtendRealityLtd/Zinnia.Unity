namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Extracts and emits the <see cref="float"/> components of a <see cref="Vector2"/>.
    /// </summary>
    public class Vector2ComponentExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines an event with a <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The components of a <see cref="Vector2"/>
        /// </summary>
        public enum Vector2Component
        {
            /// <summary>
            /// The X component.
            /// </summary>
            X,
            /// <summary>
            /// The Y component.
            /// </summary>
            Y
        }

        /// <summary>
        /// The source to extract from.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2 Source { get; set; }
        /// <summary>
        /// The component to extract from the <see cref="Vector2"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2Component ComponentToExtract { get; set; } = Vector2Component.X;

        /// <summary>
        /// Emitted when the <see cref="float"/> component from <see cref="Source"/> is extracted.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Extracted = new UnityEvent();

        /// <summary>
        /// The extracted <see cref="float"/> component.
        /// </summary>
        public float? Result { get; protected set; }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The extracted <see cref="float"/>.</returns>
        public virtual float? Extract()
        {
            if (!isActiveAndEnabled)
            {
                Result = null;
                return null;
            }

            switch (ComponentToExtract)
            {
                case Vector2Component.X:
                    Result = Source.x;
                    break;
                case Vector2Component.Y:
                    Result = Source.y;
                    break;
            }

            Extracted?.Invoke(Result.Value);
            return Result;
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        public virtual void DoExtract()
        {
            Extract();
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source to extract from.</param>
        /// <returns>The extracted <see cref="float"/>.</returns>
        public virtual float? Extract(Vector2 source)
        {
            Source = source;
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="float"/> component from the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source to extract from.</param>
        public virtual void DoExtract(Vector2 source)
        {
            Extract(source);
        }
    }
}