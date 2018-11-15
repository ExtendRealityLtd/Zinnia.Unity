namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// Transforms a float value to a boolean based on a defined threshold.
    /// </summary>
    /// <example>
    /// threshold[0.3, 0.8] -> 0f = false
    /// threshold[0.3, 0.8] -> 1f = false
    /// threshold[0.3, 0.8] -> 0.5f = true
    /// </example>
    public class FloatToBoolean : Transformer<float, bool, FloatToBoolean.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="bool"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<bool>
        {
        }

        /// <summary>
        /// The bounds in which the float must be to be considered a positive boolean.
        /// </summary>
        [Tooltip("The bounds in which the float must be to be considered a positive boolean."), MinMaxRange(0f, 1f), SerializeField]
        protected FloatRange positiveBounds = new FloatRange(0f, 1f);

        /// <summary>
        /// Sets the positive bounds.
        /// </summary>
        /// <param name="positiveBounds">The new positive bounds.</param>
        public virtual void SetPositiveBounds(FloatRange positiveBounds)
        {
            this.positiveBounds = new FloatRange(positiveBounds.minimum, positiveBounds.maximum);
        }

        /// <summary>
        /// Sets the positive bounds from a given Vector2.
        /// </summary>
        /// <param name="positiveBounds">The new positive bounds.</param>
        public virtual void SetPositiveBounds(Vector2 positiveBounds)
        {
            this.positiveBounds = new FloatRange(positiveBounds);
        }

        /// <summary>
        /// Transforms the given input bool to the float equavalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override bool Process(float input)
        {
            return positiveBounds.Contains(input);
        }
    }
}