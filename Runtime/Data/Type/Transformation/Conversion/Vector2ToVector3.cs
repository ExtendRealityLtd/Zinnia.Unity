namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Transforms a <see cref="Vector2"/> to a <see cref="Vector3"/> and allows mapping of the relevant coordinates.
    /// </summary>
    /// <example>
    /// Vector2(1f, 2f) -> XToXAndYToY -> Vector3(1f, 2f, 0f)
    /// Vector2(1f, 2f) -> XToXAndYToZ -> Vector3(1f, 0f, 2f)
    /// Vector2(1f, 2f) -> XToYAndYToX -> Vector3(2f, 1f, 0f)
    /// </example>
    public class Vector2ToVector3 : Transformer<Vector2, Vector3, Vector2ToVector3.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The mapping of <see cref="Vector2"/> coordinates to the <see cref="Vector3"/> coordinates.
        /// </summary>
        public enum CoordinateMapType
        {
            /// <summary>
            /// Maps (X,Y) to (X,Y,-)
            /// </summary>
            XToXAndYToY,
            /// <summary>
            /// Maps (X,Y) to (X,-,Y)
            /// </summary>
            XToXAndYToZ,
            /// <summary>
            /// Maps (X,Y) to (Y,X,-)
            /// </summary>
            XToYAndYToX,
            /// <summary>
            /// Maps (X,Y) to (-,X,Y)
            /// </summary>
            XToYAndYToZ,
            /// <summary>
            /// Maps (X,Y) to (Y,-,X)
            /// </summary>
            XToZAndYToX,
            /// <summary>
            /// Maps (X,Y) to (-,Y,X)
            /// </summary>
            XToZAndYToY
        }

        /// <summary>
        /// The mechanism for mapping the <see cref="Vector2"/> coordinates to the <see cref="Vector3"/> coordinates.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public CoordinateMapType CoordinateMap { get; set; } = CoordinateMapType.XToXAndYToY;
        /// <summary>
        /// The value to set the unused coordinate to during the conversion.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float UnusedCoordinateValue { get; set; }

        /// <summary>
        /// Transforms the given <see cref="Vector2"/> into a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector3 Process(Vector2 input)
        {
            switch (CoordinateMap)
            {
                case CoordinateMapType.XToXAndYToY:
                    return new Vector3(input.x, input.y, UnusedCoordinateValue);
                case CoordinateMapType.XToXAndYToZ:
                    return new Vector3(input.x, UnusedCoordinateValue, input.y);
                case CoordinateMapType.XToYAndYToX:
                    return new Vector3(input.y, input.x, UnusedCoordinateValue);
                case CoordinateMapType.XToYAndYToZ:
                    return new Vector3(UnusedCoordinateValue, input.x, input.y);
                case CoordinateMapType.XToZAndYToX:
                    return new Vector3(input.y, UnusedCoordinateValue, input.x);
                case CoordinateMapType.XToZAndYToY:
                    return new Vector3(UnusedCoordinateValue, input.y, input.x);
            }
            return Vector3.zero;
        }
    }
}