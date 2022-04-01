namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Transforms a float value to the equivalent TimeSpan value.
    /// </summary>
    /// <example>
    /// 60f = 00:01:00
    /// 150f = 00:02:30
    /// </example>
    public class FloatToTimeSpan : Transformer<float, TimeSpan, FloatToTimeSpan.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="TimeSpan"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TimeSpan> { }

        /// <summary>
        /// The properties for a <see cref="TimeSpan"/>.
        /// </summary>
        public enum TimeSpanProperty
        {
            /// <summary>
            /// The days of the time interval.
            /// </summary>
            Days,
            /// <summary>
            /// The hours of the time interval.
            /// </summary>
            Hours,
            /// <summary>
            /// The milliseconds of the time interval.
            /// </summary>
            Milliseconds,
            /// <summary>
            /// The minutes of the time interval.
            /// </summary>
            Minutes,
            /// <summary>
            /// The seconds of the time interval.
            /// </summary>
            Seconds
        }

        [Tooltip("Determines which value to use from the TimeSpan.")]
        [SerializeField]
        private TimeSpanProperty timeSpanValue = TimeSpanProperty.Seconds;
        /// <summary>
        /// Determines which value to use from the <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpanProperty TimeSpanValue
        {
            get
            {
                return timeSpanValue;
            }
            set
            {
                timeSpanValue = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="TimeSpanValue"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="TimeSpanProperty"/>.</param>
        public virtual void SetTimeSpanValue(int index)
        {
            TimeSpanValue = EnumExtensions.GetByIndex<TimeSpanProperty>(index);
        }

        /// <summary>
        /// Transforms the given input <see cref="float"/> to the <see cref="TimeSpan"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override TimeSpan Process(float input)
        {
            switch (TimeSpanValue)
            {
                case TimeSpanProperty.Days:
                    return TimeSpan.FromDays(input);
                case TimeSpanProperty.Hours:
                    return TimeSpan.FromHours(input);
                case TimeSpanProperty.Milliseconds:
                    return TimeSpan.FromMilliseconds(input);
                case TimeSpanProperty.Minutes:
                    return TimeSpan.FromMinutes(input);
                case TimeSpanProperty.Seconds:
                    return TimeSpan.FromSeconds(input);
            }

            return TimeSpan.Zero;
        }
    }
}