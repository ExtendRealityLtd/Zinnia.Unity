namespace Zinnia.Data.Type.Transformation
{
    using System;
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
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// The magnitude to use when transforming values.
        /// </summary>
        [Tooltip("The magnitude to use when transforming values.")]
        [SerializeField]
        private float _magnitude = 1f;
        public float Magnitude
        {
            get
            {
                return _magnitude;
            }
            set
            {
                _magnitude = value;
            }
        }

        /// <summary>
        /// Sets the magnitude to use.
        /// </summary>
        /// <param name="magnitudeSource">The source of the magnitude to use when transforming values.</param>
        public virtual void SetMagnitude(Vector3 magnitudeSource)
        {
            Magnitude = magnitudeSource.magnitude;
        }

        /// <summary>
        /// Transforms the given <see cref="Vector3"/> by changing its magnitude to <see cref="Magnitude"/>.
        /// </summary>
        /// <param name="input">The value to change the magnitude of.</param>
        /// <returns>A new <see cref="Vector3"/> with the same direction as <paramref name="input"/> and a magnitude of <see cref="Magnitude"/>.</returns>
        protected override Vector3 Process(Vector3 input)
        {
            return input.normalized * Magnitude;
        }
    }
}