namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a float value to a boolean based on a defined threshold.
    /// </summary>
    /// <example>
    /// PositiveBounds[0.3, 0.8] -> 0f = false
    /// PositiveBounds[0.3, 0.8] -> 1f = false
    /// PositiveBounds[0.3, 0.8] -> 0.5f = true
    /// </example>
    public class FloatToBoolean : Transformer<float, bool, FloatToBoolean.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="bool"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<bool> { }

        [Tooltip("The bounds in which the float must be to be considered a positive boolean.")]
        [SerializeField]
        private FloatRange positiveBounds = new FloatRange(0f, 1f);
        /// <summary>
        /// The bounds in which the <see cref="float"/> must be to be considered a positive boolean.
        /// </summary>
        public FloatRange PositiveBounds
        {
            get
            {
                return positiveBounds;
            }
            set
            {
                positiveBounds = value;
            }
        }

        /// <summary>
        /// Sets the positive bounds from a given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="positiveBounds">The new positive bounds.</param>
        public virtual void SetPositiveBounds(Vector2 positiveBounds)
        {
            PositiveBounds = new FloatRange(positiveBounds);
        }

        /// <summary>
        /// Transforms the given input <see cref="flaot"/> to the equivalent <see cref="bool"/> value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override bool Process(float input)
        {
            return PositiveBounds.Contains(input);
        }
    }
}