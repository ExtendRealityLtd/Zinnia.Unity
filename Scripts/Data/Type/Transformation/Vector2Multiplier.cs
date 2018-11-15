namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a Vector2 by multiplying each coordinate by a given multiplier.
    /// </summary>
    /// <example>
    /// (2f,3f) * [3f,4f] = (6f,12f)
    /// </example>
    public class Vector2Multiplier : Transformer<Vector2, Vector2, Vector2Multiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// The value to multiply the input by.
        /// </summary>
        [Tooltip("The value to multiply the input by."), SerializeField]
        protected Vector2 multiplier = Vector2.one;

        /// <summary>
        /// Sets the value to multiply the input by.
        /// </summary>
        /// <param name="multiplier">The new multiplier value.</param>
        public virtual void SetMultiplier(Vector2 multiplier)
        {
            this.multiplier = multiplier;
        }

        /// <summary>
        /// Sets the x value to multiply the input x by.
        /// </summary>
        /// <param name="xMultiplier">The new x multiplier value.</param>
        public virtual void SetXMultiplier(float xMultiplier)
        {
            multiplier.x = xMultiplier;
        }

        /// <summary>
        /// Sets the y value to multiply the input y by.
        /// </summary>
        /// <param name="yMultiplier">The new y multiplier value.</param>
        public virtual void SetYMultiplier(float yMultiplier)
        {
            multiplier.y = yMultiplier;
        }

        /// <summary>
        /// Multiplies the input by the multipliers.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector2 Process(Vector2 input)
        {
            return Vector2.Scale(input, multiplier);
        }
    }
}