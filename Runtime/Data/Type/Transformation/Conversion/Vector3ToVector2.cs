namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Transforms a <see cref="Vector3"/> to a <see cref="Vector2"/> and allows mapping of the relevant coordinates.
    /// </summary>
    /// <example>
    /// Vector3(1f, 2f, 3f) -> XToXAndYToYExcludeZ -> Vector2(1f, 2f)
    /// Vector3(1f, 2f, 3f) -> XToYAndYToXExcludeZ -> Vector2(2f, 1f)
    /// Vector3(1f, 2f, 3f) -> XToYAndZToXExcludeY -> Vector2(3f, 1f)
    /// </example>
    public class Vector3ToVector2 : Transformer<Vector3, Vector2, Vector3ToVector2.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// The mapping of <see cref="Vector3"/> coordinates to the <see cref="Vector2"/> coordinates.
        /// </summary>
        public enum CoordinateMapType
        {
            /// <summary>
            /// Maps (X,Y,Z) to (X,Y)
            /// </summary>
            XToXAndYToYExcludeZ,
            /// <summary>
            /// Maps (X,Y,Z) to (Y,X)
            /// </summary>
            XToYAndYToXExcludeZ,
            /// <summary>
            /// Maps (X,Y,Z) to (Z,Y)
            /// </summary>
            YToYAndZToXExcludeX,
            /// <summary>
            /// Maps (X,Y,Z) to (Y,Z)
            /// </summary>
            YToXAndZToYExcludeX,
            /// <summary>
            /// Maps (X,Y,Z) to (X,Z)
            /// </summary>
            XToXAndZToYExcludeY,
            /// <summary>
            /// Maps (X,Y,Z) to (Z,X)
            /// </summary>
            XToYAndZToXExcludeY
        }

        /// <summary>
        /// The mechanism for mapping the <see cref="Vector3"/> coordinates to the <see cref="Vector2"/> coordinates.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public CoordinateMapType CoordinateMap { get; set; } = CoordinateMapType.XToXAndYToYExcludeZ;

        /// <summary>
        /// Transforms the given <see cref="Vector3"/> into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector2 Process(Vector3 input)
        {
            switch (CoordinateMap)
            {
                case CoordinateMapType.XToXAndYToYExcludeZ:
                    return new Vector3(input.x, input.y);
                case CoordinateMapType.XToYAndYToXExcludeZ:
                    return new Vector3(input.y, input.x);
                case CoordinateMapType.YToYAndZToXExcludeX:
                    return new Vector3(input.z, input.y);
                case CoordinateMapType.YToXAndZToYExcludeX:
                    return new Vector3(input.y, input.z);
                case CoordinateMapType.XToXAndZToYExcludeY:
                    return new Vector3(input.x, input.z);
                case CoordinateMapType.XToYAndZToXExcludeY:
                    return new Vector3(input.z, input.x);
            }
            return Vector2.zero;
        }
    }
}