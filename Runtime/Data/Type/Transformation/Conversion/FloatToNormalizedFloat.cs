namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a float value to a normalized float based on a defined range.
    /// </summary>
    /// <example>
    /// Range[0f, 10f] -> 5f = 0.5f
    /// Range[0f, 10f] -> 11f = 1f
    /// Range[0f, 10f] -> -5f = 0f
    /// </example>
    public class FloatToNormalizedFloat : Transformer<float, float, FloatToNormalizedFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        [Tooltip("The range in which to consider the minimum and maximum value for normalizing.")]
        [SerializeField]
        private FloatRange range = new FloatRange(0f, 1f);
        /// <summary>
        /// The range in which to consider the minimum and maximum value for normalizing.
        /// </summary>
        protected FloatRange Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="Range"/> from a given Vector2.
        /// </summary>
        /// <param name="range">The new range.</param>
        public virtual void SetRange(Vector2 range)
        {
            Range = new FloatRange(range);
        }

        /// <summary>
        /// Transforms the given input <see cref="float"/> to the equivalent normalized <see cref="float"/> value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(float input)
        {
            return Mathf.InverseLerp(Range.minimum, Range.maximum, input);
        }
    }
}