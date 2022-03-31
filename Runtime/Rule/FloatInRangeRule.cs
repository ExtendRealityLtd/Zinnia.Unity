namespace Zinnia.Rule
{
    using UnityEngine;
    using Zinnia.Data.Type;

    /// <summary>
    /// Determines whether a given <see cref="float"/> is within the range of the specified <see cref="Range"/>.
    /// </summary>
    public class FloatInRangeRule : FloatRule
    {
        /// <summary>
        /// The range in which the given <see cref="float"/> must be equal to the bounds or within the range.
        /// </summary>
        [Tooltip("The range in which the given float must be equal to the bounds or within the range.")]
        [SerializeField]
        private FloatRange _range;
        public FloatRange Range
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
        protected override bool Accepts(float targetFloat)
        {
            return Range.Contains(targetFloat);
        }
    }
}