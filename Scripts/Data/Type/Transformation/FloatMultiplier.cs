namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a float by multiplying it by a given multiplier.
    /// </summary>
    /// <example>
    /// 2f * 2f = 4f
    /// </example>
    public class FloatMultiplier : Transformer<float, float, FloatMultiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The value to multiply the input by.
        /// </summary>
        [Tooltip("The value to multiply the input by."), SerializeField]
        protected float multiplier;

        /// <summary>
        /// Sets the value to multiply the input by.
        /// </summary>
        /// <param name="multiplier">The new multiplier value.</param>
        public virtual void SetMultiplier(float multiplier)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// Multiplies the input by the multiplier.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(float input)
        {
            return (input * multiplier);
        }
    }
}