namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Transforms a TimeSpan value to the equivalent float value.
    /// </summary>
    public class TimeSpanToFloat : Transformer<TimeSpan, float, TimeSpanToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

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
            Seconds,
            /// <summary>
            /// The ticks of the time interval.
            /// </summary>
            Ticks,
            /// <summary>
            /// The time interval expressed in whole and fractional days.
            /// </summary>
            TotalDays,
            /// <summary>
            /// The time interval expressed in whole and fractional hours.
            /// </summary>
            TotalHours,
            /// <summary>
            /// The time interval expressed in whole and fractional milliseconds.
            /// </summary>
            TotalMilliseconds,
            /// <summary>
            /// The time interval expressed in whole and fractional minutes.
            /// </summary>
            TotalMinutes,
            /// <summary>
            /// The time interval expressed in whole and fractional seconds.
            /// </summary>
            TotalSeconds
        }

        [Tooltip("Determines which value to use from the TimeSpan.")]
        [SerializeField]
        private TimeSpanProperty timeSpanValue;
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
        /// Transforms the given input <see cref="TimeSpan"/> to the <see cref="float"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(TimeSpan input)
        {
            switch (TimeSpanValue)
            {
                case TimeSpanProperty.Days:
                    return input.Days;
                case TimeSpanProperty.Hours:
                    return input.Hours;
                case TimeSpanProperty.Milliseconds:
                    return input.Milliseconds;
                case TimeSpanProperty.Minutes:
                    return input.Minutes;
                case TimeSpanProperty.Seconds:
                    return input.Seconds;
                case TimeSpanProperty.Ticks:
                    return input.Ticks;
                case TimeSpanProperty.TotalDays:
                    return (float)input.TotalDays;
                case TimeSpanProperty.TotalHours:
                    return (float)input.TotalHours;
                case TimeSpanProperty.TotalMilliseconds:
                    return (float)input.TotalMilliseconds;
                case TimeSpanProperty.TotalMinutes:
                    return (float)input.TotalMinutes;
                case TimeSpanProperty.TotalSeconds:
                    return (float)input.TotalSeconds;
            }

            return 0f;
        }
    }
}