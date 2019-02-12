namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Transforms a <see cref="Vector3"/> by clamping the coordinates to be within given boundaries.
    /// </summary>
    /// <example>
    /// input(5f,-5f,5f) -> bounds([-2f,2f], [-3f,3f], [-2f,2f]) = (2f,-3f,2f)
    /// input(5f,-5f,5f) -> bounds([-2f,2f], [1f,3f], [-2f,2f]) = (2f,1f,2f)
    /// </example>
    public class Vector3Restrictor : Transformer<Vector3, Vector3, Vector3Restrictor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The minimum and maximum values that the x coordinate can be.
        /// </summary>
        [DocumentedByXml]
        public FloatRange xBounds = new FloatRange();
        /// <summary>
        /// The minimum and maximum values that the y coordinate can be.
        /// </summary>
        [DocumentedByXml]
        public FloatRange yBounds = new FloatRange();
        /// <summary>
        /// The minimum and maximum values that the z coordinate can be.
        /// </summary>
        [DocumentedByXml]
        public FloatRange zBounds = new FloatRange();

        /// <summary>
        /// Transforms the given input by clamping it within the specified bounds.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector3 Process(Vector3 input)
        {
            return new Vector3(
                Mathf.Clamp(input.x, xBounds.minimum, xBounds.maximum),
                Mathf.Clamp(input.y, yBounds.minimum, yBounds.maximum),
                Mathf.Clamp(input.z, zBounds.minimum, zBounds.maximum)
                );
        }
    }
}