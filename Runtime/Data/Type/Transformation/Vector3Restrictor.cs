namespace Zinnia.Data.Type.Transformation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

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
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// The minimum and maximum values that the x coordinate can be.
        /// </summary>
        [Tooltip("The minimum and maximum values that the x coordinate can be.")]
        [SerializeField]
        private FloatRange _xBounds = FloatRange.MinMax;
        public FloatRange XBounds
        {
            get
            {
                return _xBounds;
            }
            set
            {
                _xBounds = value;
            }
        }
        /// <summary>
        /// The minimum and maximum values that the y coordinate can be.
        /// </summary>
        [Tooltip("The minimum and maximum values that the y coordinate can be.")]
        [SerializeField]
        private FloatRange _yBounds = FloatRange.MinMax;
        public FloatRange YBounds
        {
            get
            {
                return _yBounds;
            }
            set
            {
                _yBounds = value;
            }
        }
        /// <summary>
        /// The minimum and maximum values that the z coordinate can be.
        /// </summary>
        [Tooltip("The minimum and maximum values that the z coordinate can be.")]
        [SerializeField]
        private FloatRange _zBounds = FloatRange.MinMax;
        public FloatRange ZBounds
        {
            get
            {
                return _zBounds;
            }
            set
            {
                _zBounds = value;
            }
        }

        /// <summary>
        /// Transforms the given input by clamping it within the specified bounds.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override Vector3 Process(Vector3 input)
        {
            return new Vector3(
                Mathf.Clamp(input.x, XBounds.minimum, XBounds.maximum),
                Mathf.Clamp(input.y, YBounds.minimum, YBounds.maximum),
                Mathf.Clamp(input.z, ZBounds.minimum, ZBounds.maximum)
                );
        }
    }
}