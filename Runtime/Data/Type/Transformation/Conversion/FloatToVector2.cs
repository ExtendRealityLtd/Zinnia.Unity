namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

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
        public class UnityEvent : UnityEvent<Vector2> { }

        [Tooltip("A float to use as the current x value of the Vector2.")]
        [SerializeField]
        private float currentX;
        /// <summary>
        /// A float to use as the current x value of the <see cref="Vector2"/>.
        /// </summary>
        public float CurrentX
        {
            get
            {
                return currentX;
            }
            set
            {
                currentX = value;
            }
        }
        [Tooltip("A float to use as the current y value of the Vector2.")]
        [SerializeField]
        private float currentY;
        /// <summary>
        /// A float to use as the current y value of the <see cref="Vector2"/>.
        /// </summary>
        public float CurrentY
        {
            get
            {
                return currentY;
            }
            set
            {
                currentY = value;
            }
        }

        /// <summary>
        /// A reusable array of two <see cref="float"/>s.
        /// </summary>
        protected readonly float[] input = new float[2];

        /// <summary>
        /// Builds a <see cref="float"/> array from the current set x and y values and transforms it into a <see cref="Vector2"/>.
        /// </summary>
        public virtual Vector2 Transform()
        {
            input[0] = CurrentX;
            input[1] = CurrentY;
            return Transform(input);
        }

        /// <summary>
        /// Builds a <see cref="float"/> array from the current set x and y values and transforms it into a <see cref="Vector2"/>.
        /// </summary>
        public virtual void DoTransform()
        {
            Transform();
        }

        /// <summary>
        /// Transforms the given <see cref="float"/> array into a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value or <see cref="Vector2.zero"/> if the input isn't two-dimensional.</returns>
        protected override Vector2 Process(float[] input)
        {
            return input.Length >= 2 ? new Vector2(input[0], input[1]) : Vector2.zero;
        }
    }
}