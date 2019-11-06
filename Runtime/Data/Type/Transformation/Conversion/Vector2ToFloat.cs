namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Transforms a <see cref="Vector2"/> to a <see cref="float"/> and allows mapping of the relevant coordinates.
    /// </summary>
    /// <example>
    /// Vector2(3f, 4f) -> ExtractX -> 3f
    /// Vector2(3f, 4f) -> ExtractY -> 4f
    /// Vector2(3f, 4f) -> ExtractMagnitude -> 5f
    /// Vector2(3f, 4f) -> ExtractSqrMagnitude -> 25f
    /// </example>
    public class Vector2ToFloat : Transformer<Vector2, float, Vector2ToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The <see cref="Vector2"/> coordinate to extract.
        /// </summary>
        public enum ExtractionCoordinate
        {
            /// <summary>
            /// Extracts the X value.
            /// </summary>
            ExtractX,
            /// <summary>
            /// Extracts the Y value.
            /// </summary>
            ExtractY,
            /// <summary>
            /// Extracts the magnitude.
            /// </summary>
            ExtractMagnitude,
            /// <summary>
            /// Extracts the squared magnitude.
            /// </summary>
            ExtractSqrMagnitude
        }

        /// <summary>
        /// Which <see cref="Vector2"/> coordinate to extract.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public ExtractionCoordinate CoordinateToExtract { get; set; } = ExtractionCoordinate.ExtractX;

        /// <summary>
        /// Transforms the given <see cref="Vector2"/> into a <see cref="float"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(Vector2 input)
        {
            switch (CoordinateToExtract)
            {
                case ExtractionCoordinate.ExtractX:
                    return input.x;
                case ExtractionCoordinate.ExtractY:
                    return input.y;
                case ExtractionCoordinate.ExtractMagnitude:
                    return input.magnitude;
                case ExtractionCoordinate.ExtractSqrMagnitude:
                    return input.sqrMagnitude;
            }
            return 0f;
        }
    }
}