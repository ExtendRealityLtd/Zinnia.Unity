namespace Zinnia.Data.Type.Transformation
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a <see cref="Vector2"/> by changing its magnitude.
    /// </summary>
    public class Vector2MagnitudeSetter : Transformer<Vector2, Vector2, Vector2MagnitudeSetter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> with the changed magnitude.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <summary>
        /// The magnitude to use when transforming values.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float Magnitude { get; set; } = 1f;

        /// <summary>
        /// Sets the magnitude to use.
        /// </summary>
        /// <param name="magnitudeSource">The source of the magnitude to use when transforming values.</param>
        public virtual void SetMagnitude(Vector2 magnitudeSource)
        {
            Magnitude = magnitudeSource.magnitude;
        }

        /// <summary>
        /// Transforms the given <see cref="Vector2"/> by changing its magnitude to <see cref="Magnitude"/>.
        /// </summary>
        /// <param name="input">The value to change the magnitude of.</param>
        /// <returns>A new <see cref="Vector2"/> with the same direction as <paramref name="input"/> and a magnitude of <see cref="Magnitude"/>.</returns>
        protected override Vector2 Process(Vector2 input)
        {
            return input.normalized * Magnitude;
        }
    }
}