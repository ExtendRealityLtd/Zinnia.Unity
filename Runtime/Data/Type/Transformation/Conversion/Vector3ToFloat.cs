namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Transforms a <see cref="Vector3"/> to a <see cref="float"/> and allows mapping of the relevant coordinates.
    /// </summary>
    /// <example>
    /// Vector3(2f, 3f, 6f) -> ExtractX -> 2f
    /// Vector3(2f, 3f, 6f) -> ExtractY -> 3f
    /// Vector3(2f, 3f, 6f) -> ExtractZ -> 6f
    /// Vector3(2f, 3f, 6f) -> ExtractMagnitude -> 7f
    /// Vector3(2f, 3f, 6f) -> ExtractSqrMagnitude -> 49f
    /// </example>
    public class Vector3ToFloat : Transformer<Vector3, float, Vector3ToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The <see cref="Vector3"/> coordinate to extract.
        /// </summary>
        public enum ExtractionCoordinate
        {
            /// <summary>
            /// Extracts the X value.
            /// </summary>
            ExtractX,
            /// <summary>
            /// Extracts the Y value.
            /// </summary>
            ExtractY,
            /// <summary>
            /// Extracts the Z value.
            /// </summary>
            ExtractZ,
            /// <summary>
            /// Extracts the magnitude.
            /// </summary>
            ExtractMagnitude,
            /// <summary>
            /// Extracts the squared magnitude.
            /// </summary>
            ExtractSqrMagnitude
        }

        [Tooltip("Which Vector3 coordinate to extract.")]
        [SerializeField]
        private ExtractionCoordinate coordinateToExtract = ExtractionCoordinate.ExtractX;
        /// <summary>
        /// Which <see cref="Vector3"/> coordinate to extract.
        /// </summary>
        public ExtractionCoordinate CoordinateToExtract
        {
            get
            {
                return coordinateToExtract;
            }
            set
            {
                coordinateToExtract = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="CoordinateToExtract"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="ExtractionCoordinate"/>.</param>
        public virtual void SetCoordinateToExtract(int index)
        {
            CoordinateToExtract = EnumExtensions.GetByIndex<ExtractionCoordinate>(index);
        }

        /// <summary>
        /// Transforms the given <see cref="Vector3"/> into a <see cref="float"/>.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(Vector3 input)
        {
            switch (CoordinateToExtract)
            {
                case ExtractionCoordinate.ExtractX:
                    return input.x;
                case ExtractionCoordinate.ExtractY:
                    return input.y;
                case ExtractionCoordinate.ExtractZ:
                    return input.z;
                case ExtractionCoordinate.ExtractMagnitude:
                    return input.magnitude;
                case ExtractionCoordinate.ExtractSqrMagnitude:
                    return input.sqrMagnitude;
            }
            return 0f;
        }
    }
}