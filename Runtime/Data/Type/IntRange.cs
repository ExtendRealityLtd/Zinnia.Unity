namespace Zinnia.Data.Type
{
    using System;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Specifies a valid range between a lower and upper float value limit.
    /// </summary>
    [Serializable]
    public struct IntRange
    {
        /// <summary>
        /// The inclusive minimum value of the range.
        /// </summary>
        [Tooltip("The inclusive minimum value of the range.")]
        public int minimum;
        /// <summary>
        /// The inclusive maximum value of the range.
        /// </summary>
        [Tooltip("The inclusive maximum value of the range.")]
        public int maximum;

        /// <summary>
        /// Shorthand for writing <c>IntRange(int.MinValue, int.MaxValue)</c>.
        /// </summary>
        public static readonly IntRange MinMax = new IntRange(int.MinValue, int.MaxValue);

        /// <summary>
        /// Constructs a new range with the given minimum and maximum values.
        /// </summary>
        /// <param name="minimum">The minimum value for the range.</param>
        /// <param name="maximum">The maximum value for the range.</param>
        public IntRange(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        /// <summary>
        /// Constructs a new range from a given <see cref="FloatRange"/>.
        /// </summary>
        /// <param name="range">The range data.</param>
        public IntRange(FloatRange range)
        {
            minimum = (int)range.minimum;
            maximum = (int)range.maximum;
        }

        /// <summary>
        /// Constructs a new range from a given <see cref="Vector2"/> using the <see cref="Vector2.x"/> value as the minimum value and the <see cref="Vector2.y"/> value as the maximum value.
        /// </summary>
        /// <param name="range">The range data.</param>
        public IntRange(Vector2 range)
        {
            minimum = (int)range.x;
            maximum = (int)range.y;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string[] titles = new string[]
            {
                "minimum",
                "maximum"
            };

            object[] values = new object[]
            {
                minimum,
                maximum
            };

            return StringExtensions.FormatForToString(titles, values);
        }

        /// <summary>
        /// Determines if the given value is contained within the set range.
        /// </summary>
        /// <param name="value">The value to check for.</param>
        /// <returns><see langword="true"/> if the value is found within the range.</returns>
        public bool Contains(int value)
        {
            return value >= minimum && value <= maximum;
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