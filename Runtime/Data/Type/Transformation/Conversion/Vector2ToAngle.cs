namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
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
        public class UnityEvent : UnityEvent<float> { }

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
            Radians,
            /// <summary>
            /// Measurement in degrees where anti-clockwise is considered negative (min: -180 degrees) and clockwise is considered positive (max: 180 degrees).
            /// </summary>
            SignedDegrees,
            /// <summary>
            /// Measurement in radians where anti-clockwise is considered negative (min: -PI radians) and clockwise is considered positive (max: PI radians).
            /// </summary>
            SignedRadians
        }

        [Tooltip("The unit to return the converted angle in.")]
        [SerializeField]
        private AngleUnit unit = AngleUnit.Degrees;
        /// <summary>
        /// The unit to return the converted angle in.
        /// </summary>
        public AngleUnit Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }

        [Tooltip("The direction that defines the origin (i.e. zero) angle.")]
        [SerializeField]
        private Vector2 origin = new Vector2(0f, 1f);
        /// <summary>
        /// The direction that defines the origin (i.e. zero) angle.
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterOriginChange();
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="Unit"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="AngleUnit"/>.</param>
        public virtual void SetUnit(int index)
        {
            Unit = EnumExtensions.GetByIndex<AngleUnit>(index);
        }

        /// <summary>
        /// Sets the <see cref="Origin"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetOriginX(float value)
        {
            Origin = new Vector2(value, Origin.y);
        }

        /// <summary>
        /// Sets the <see cref="Origin"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetOriginY(float value)
        {
            Origin = new Vector2(Origin.x, value);
        }

        /// <summary>
        /// The full circle in radians.
        /// </summary>
        protected const double fullCircleRadians = 2f * Math.PI;
        /// <summary>
        /// The full circle in degrees.
        /// </summary>
        protected const float fullCircleDegrees = 360f;

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

            float result = (float)((Math.Atan2(Origin.y, Origin.x) - Math.Atan2(input.y, input.x) + fullCircleRadians) % fullCircleRadians * CalculateMultiplier());
            return ProcessSigned(result);
        }

        /// <summary>
        /// Calculates the multiplier to used based on the <see cref="Unit"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateMultiplier()
        {
            return (float)(Unit == AngleUnit.Degrees || Unit == AngleUnit.SignedDegrees ? (fullCircleDegrees * 0.5f) / Math.PI : 1f);
        }

        /// <summary>
        /// Processes the given value if the <see cref="Unit"/> is a signed type.
        /// </summary>
        /// <param name="value">The value to process.</param>
        /// <returns>The processed value.</returns>
        protected virtual float ProcessSigned(float value)
        {
            float fullCircle;
            switch (Unit)
            {
                case AngleUnit.SignedDegrees:
                    fullCircle = fullCircleDegrees;
                    break;
                case AngleUnit.SignedRadians:
                    fullCircle = (float)fullCircleRadians;
                    break;
                default:
                    return value;
            }

            return value > (fullCircle * 0.5f) ? value - fullCircle : value;
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
        protected virtual void OnAfterOriginChange()
        {
            origin = Origin.normalized;
        }
    }
}