namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a float array to a Vector2.
    /// </summary>
    /// <example>
    /// float[2f, 3f] = Vector2(2f, 3f)
    /// </example>
    public class FloatToVector2 : Transformer<float[], Vector2, FloatToVector2.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// A float to use as the current x value of the Vector2.
        /// </summary>
        protected float currentX;
        /// <summary>
        /// A float to use as the current y value of the Vector2.
        /// </summary>
        protected float currentY;

        /// <summary>
        /// Sets the x value of the Vector2.
        /// </summary>
        /// <param name="input">The float to use as the x value.</param>
        public virtual void SetX(float input)
        {
            currentX = input;
        }

        /// <summary>
        /// Sets the y value of the Vector2.
        /// </summary>
        /// <param name="input">The float to use as the y value.</param>
        public virtual void SetY(float input)
        {
            currentY = input;
        }

        /// <summary>
        /// Builds a float array from the current set x and y values and transforms it into a Vector2.
        /// </summary>
        public virtual Vector2 Transform()
        {
            float[] input = new float[] { currentX, currentY };
            return Transform(input);
        }

        /// <summary>
        /// Builds a float array from the current set x and y values and transforms it into a Vector2.
        /// </summary>
        public virtual void DoTransform()
        {
            Transform();
        }

        /// <summary>
        /// Transforms the given float array into a Vector2.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value or <see cref="Vector2.zero"/> if the input isn't two-dimensional.</returns>
        protected override Vector2 Process(float[] input)
        {
            return (input.Length >= 2 ? new Vector2(input[0], input[1]) : Vector2.zero);
        }
    }
}