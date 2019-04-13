namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

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
        [Serialized]
        [field: DocumentedByXml]
        public float CurrentX { get; set; }
        /// <summary>
        /// A float to use as the current y value of the Vector2.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float CurrentY { get; set; }

        /// <summary>
        /// A reusable array of two <see cref="float"/>s.
        /// </summary>
        protected readonly float[] input = new float[2];

        /// <summary>
        /// Builds a float array from the current set x and y values and transforms it into a Vector2.
        /// </summary>
        public virtual Vector2 Transform()
        {
            input[0] = CurrentX;
            input[1] = CurrentY;
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
            return input.Length >= 2 ? new Vector2(input[0], input[1]) : Vector2.zero;
        }
    }
}