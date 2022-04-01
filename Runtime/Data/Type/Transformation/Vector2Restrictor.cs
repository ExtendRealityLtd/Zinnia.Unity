namespace Zinnia.Data.Type.Transformation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a <see cref="Vector2"/> by clamping the coordinates to be within given boundaries.
    /// </summary>
    /// <example>
    /// input(5f,-5f) -> bounds([-2f,2f], [-3f,3f]) = (2f,-3f)
    /// input(5f,-5f) -> bounds([-2f,2f], [1f,3f]) = (2f,1f)
    /// </example>
    public class Vector2Restrictor : Transformer<Vector2, Vector2, Vector2Restrictor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        [Tooltip("The minimum and maximum values that the x coordinate can be.")]
        [SerializeField]
        private FloatRange xBounds = FloatRange.MinMax;
        /// <summary>
        /// The minimum and maximum values that the x coordinate can be.
        /// </summary>
        public FloatRange XBounds
        {
            get
            {
                return xBounds;
            }
            set
            {
                xBounds = value;
            }
        }
        [Tooltip("The minimum and maximum values that the y coordinate can be.")]
        [SerializeField]
        private FloatRange yBounds = FloatRange.MinMax;
        /// <summary>
        /// The minimum and maximum values that the y coordinate can be.
        /// </summary>
        public FloatRange YBounds
        {
            get
            {
                return yBounds;
            }
            set
            {
                yBounds = value;
            }
        }

        /// <summary>
        /// Transforms the given input by clamping it within the specified bounds.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector2 Process(Vector2 input)
        {
            return new Vector2(
                Mathf.Clamp(input.x, XBounds.minimum, XBounds.maximum),
                Mathf.Clamp(input.y, YBounds.minimum, YBounds.maximum)
                );
        }
    }
}