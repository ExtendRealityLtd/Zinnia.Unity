namespace Zinnia.Data.Type.Transformation
{
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a <see cref="Vector3"/> by changing its magnitude.
    /// </summary>
    public class Vector3MagnitudeSetter : Transformer<Vector3, Vector3, Vector3MagnitudeSetter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/> with the changed magnitude.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The magnitude to use when transforming values.
        /// </summary>
        [DocumentedByXml]
        public float magnitude = 1f;

        /// <summary>
        /// Sets the magnitude to use.
        /// </summary>
        /// <param name="magnitude">The magnitude to use when transforming values.</param>
        public virtual void SetMagnitude(float magnitude)
        {
            this.magnitude = magnitude;
        }

        /// <summary>
        /// Sets the magnitude to use.
        /// </summary>
        /// <param name="magnitudeSource">The source of the magnitude to use when transforming values.</param>
        public virtual void SetMagnitude(Vector3 magnitudeSource)
        {
            magnitude = magnitudeSource.magnitude;
        }

        /// <summary>
        /// Transforms the given <see cref="Vector3"/> by changing its magnitude to <see cref="magnitude"/>.
        /// </summary>
        /// <param name="input">The value to change the magnitude of.</param>
        /// <returns>A new <see cref="Vector3"/> with the same direction as <paramref name="input"/> and a magnitude of <see cref="magnitude"/>.</returns>
        protected override Vector3 Process(Vector3 input)
        {
            return input.normalized * magnitude;
        }
    }
}