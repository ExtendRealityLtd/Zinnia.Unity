namespace Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Transforms a <see cref="Vector2"/> value to a <see cref="float"/> angle.
    /// </summary>
    /// <remarks>
    /// The origin angle (i.e. zero) is set to the top by default to mimic a clock with 12 o'clock being the 0 angle.
    /// </remarks>
    /// <example>
    /// Vector2[0f, 0f] -> unit.degrees -> origin.Vector2(0f, 1f) = 0f
    /// Vector2[1f, 0f] -> unit.degrees -> origin.Vector2(0f, 1f) = 90f
    /// Vector2[0f, -1f] -> unit.degrees -> origin.Vector2(0f, 1f) = 180f
    /// </example>
    public class Vector2ToAngle : Transformer<Vector2, float, Vector2ToAngle.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The unit to calculate the angle in.
        /// </summary>
        public enum AngleUnit
        {
            /// <summary>
            /// Measurement in degrees. A full rotation is 360 degrees.
            /// </summary>
            Degrees,
            /// <summary>
            /// Measurement in radians. A full rotation is 2 * PI radians.
            /// </summary>
            Radians
        }

        /// <summary>
        /// The unit to return the converted angle in.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public AngleUnit Unit { get; set; } = AngleUnit.Degrees;

        /// <summary>
        /// The direction that defines the origin (i.e. zero) angle.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector2 Origin { get; set; } = new Vector2(0f, 1f);

        /// <summary>
        /// The full circle in radians.
        /// </summary>
        protected const double fullCircle = 2f * Math.PI;

        /// <summary>
        /// Transforms the given input <see cref="Vector2"/> to the angle.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(Vector2 input)
        {
            if (input.sqrMagnitude.ApproxEquals(0f))
            {
                return 0f;
            }

            return (float)
                ((Math.Atan2(Origin.y, Origin.x) - Math.Atan2(input.y, input.x) + fullCircle) % fullCircle
                    * (Unit == AngleUnit.Degrees ? 180f / Math.PI : 1f));
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Origin))]
        protected virtual void OnAfterOriginChange()
        {
            Origin = Origin.normalized;
        }
    }
}