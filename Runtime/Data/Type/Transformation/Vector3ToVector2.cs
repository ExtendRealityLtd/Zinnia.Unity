namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;

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
        public enum CoordinateMap
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
        [SerializeField, DocumentedByXml]
        protected CoordinateMap coordinateMap = CoordinateMap.XToXAndYToYExcludeZ;

        /// <summary>
        /// Sets the coordinate mapping used during the conversion.
        /// </summary>
        /// <param name="coordinateMap">The new coordinate mapping.</param>
        public virtual void SetCoordinateMap(CoordinateMap coordinateMap)
        {
            this.coordinateMap = coordinateMap;
        }

        /// <summary>
        /// Transforms the given <see cref="Vector3"/> into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector2 Process(Vector3 input)
        {
            switch (coordinateMap)
            {
                case CoordinateMap.XToXAndYToYExcludeZ:
                    return new Vector3(input.x, input.y);
                case CoordinateMap.XToYAndYToXExcludeZ:
                    return new Vector3(input.y, input.x);
                case CoordinateMap.YToYAndZToXExcludeX:
                    return new Vector3(input.z, input.y);
                case CoordinateMap.YToXAndZToYExcludeX:
                    return new Vector3(input.y, input.z);
                case CoordinateMap.XToXAndZToYExcludeY:
                    return new Vector3(input.x, input.z);
                case CoordinateMap.XToYAndZToXExcludeY:
                    return new Vector3(input.z, input.x);
            }
            return Vector2.zero;
        }
    }
}