namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Type;

    /// <summary>
    /// Determines whether a given <see cref="int"/> is within the range of the specified <see cref="Range"/>.
    /// </summary>
    public class IntInRangeRule : IntRule
    {
        /// <summary>
        /// The range in which the given <see cref="int"/> must be equal to the bounds or within the range.
        /// </summary>
        [Tooltip("The range in which the given int must be equal to the bounds or within the range.")]
        [SerializeField]
        private IntRange _range;
        public IntRange Range
        {
            get
            {
                return _range;
            }
            set
            {
                _range = value;
            }
        }

        /// <inheritdoc />
        protected override bool Accepts(int targetInt)
        {
            return Range.Contains(targetInt);
        }
    }
}