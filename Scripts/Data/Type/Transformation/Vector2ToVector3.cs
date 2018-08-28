namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a Vector2 to a Vector3 and allows mapping of the relevant coordinates.
    /// </summary>
    /// <example>
    /// Vector2(1f, 2f) -> XToXAndYToY -> Vector3(1f, 2f, 0f)
    /// Vector2(1f, 2f) -> XToXAndYToZ -> Vector3(1f, 0f, 2f)
    /// Vector2(1f, 2f) -> XToYAndYToX -> Vector3(2f, 1f, 0f)
    /// </example>
    public class Vector2ToVector3 : BaseTransformer<Vector2, Vector3, Vector2ToVector3.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The mapping of Vector2 coordinates to the Vector3 coordinates.
        /// </summary>
        public enum CoordinateMap
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
        /// The mechanism for mapping the Vector2 coordinates to the Vector3 coordinates.
        /// </summary>
        [Tooltip("The mechanism for mapping the Vector2 coordinates to the Vector3 coordinates."), SerializeField]
        protected CoordinateMap coordinateMap = CoordinateMap.XToXAndYToY;
        /// <summary>
        /// The value to set the unused coordinate to during the conversion.
        /// </summary>
        [Tooltip("The value to set the unused coordinate to during the conversion."), SerializeField]
        protected float unusedCoordinateValue = 0f;

        /// <summary>
        /// Sets the coordinate mapping used during the conversion.
        /// </summary>
        /// <param name="coordinateMap">The new coordinate mapping.</param>
        public virtual void SetCoordinateMap(CoordinateMap coordinateMap)
        {
            this.coordinateMap = coordinateMap;
        }

        /// <summary>
        /// Sets the value that set the unused coordinate during the conversion.
        /// </summary>
        /// <param name="unusedCoordinateValue">The new unused coordinate value.</param>
        public virtual void SetUnusedCoordinateValue(float unusedCoordinateValue)
        {
            this.unusedCoordinateValue = unusedCoordinateValue;
        }

        /// <summary>
        /// Transforms the given Vector2 into a Vector3.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector3 Process(Vector2 input)
        {
            switch (coordinateMap)
            {
                case CoordinateMap.XToXAndYToY:
                    return new Vector3(input.x, input.y, unusedCoordinateValue);
                case CoordinateMap.XToXAndYToZ:
                    return new Vector3(input.x, unusedCoordinateValue, input.y);
                case CoordinateMap.XToYAndYToX:
                    return new Vector3(input.y, input.x, unusedCoordinateValue);
                case CoordinateMap.XToYAndYToZ:
                    return new Vector3(unusedCoordinateValue, input.x, input.y);
                case CoordinateMap.XToZAndYToX:
                    return new Vector3(input.y, unusedCoordinateValue, input.x);
                case CoordinateMap.XToZAndYToY:
                    return new Vector3(unusedCoordinateValue, input.y, input.x);
            }
            return Vector3.zero;
        }
    }
}