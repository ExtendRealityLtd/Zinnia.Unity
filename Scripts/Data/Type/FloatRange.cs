namespace Zinnia.Data.Type
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Specifies a range of float values.
    /// </summary>
    [Serializable]
    public class FloatRange
    {
        /// <summary>
        /// The inclusive minimum value of the range.
        /// </summary>
        public float minimum;
        /// <summary>
        /// The inclusive maximum value of the range.
        /// </summary>
        public float maximum;

        /// <summary>
        /// Constructs a new range with the minimum value being <see cref="float.MinValue"/> and the maximum value being <see cref="float.MaxValue"/>.
        /// </summary>
        public FloatRange()
        {
            minimum = float.MinValue;
            maximum = float.MaxValue;
        }

        /// <summary>
        /// Constructs a new range with the given minimum and maximum values.
        /// </summary>
        /// <param name="minimum">The minimum value for the range.</param>
        /// <param name="maximum">The maximum value for the range.</param>
        public FloatRange(float minimum, float maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        /// <summary>
        /// Constructs a new range from a given <see cref="Vector2"/> using the <see cref="Vector2.x"/> value as the minumum value and the <see cref="Vector2.y"/> value as the maximum value.
        /// </summary>
        /// <param name="range">The range data.</param>
        public FloatRange(Vector2 range)
        {
            minimum = range.x;
            maximum = range.y;
        }

        /// <summary>
        /// Determines if the given value is contained within the set range.
        /// </summary>
        /// <param name="value">The value to check for.</param>
        /// <returns><see langword="true"/> if the value is found within the range.</returns>
        public bool Contains(float value)
        {
            return (value >= minimum && value <= maximum);
        }

        /// <summary>
        /// Converts to a <see cref="Vector2"/>
        /// </summary>
        /// <returns>The converted value.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(minimum, maximum);
        }
    }
}