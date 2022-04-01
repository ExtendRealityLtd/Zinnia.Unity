namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a TimeSpan value to the equivalent string value.
    /// </summary>
    public class TimeSpanToString : Transformer<TimeSpan, string, TimeSpanToString.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="string"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string> { }

        [Tooltip("A standard or custom TimeSpan format string.")]
        [SerializeField]
        private string format;
        /// <summary>
        /// A standard or custom <see cref="TimeSpan"/> format string.
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// Transforms the given input <see cref="TimeSpan"/> to the <see cref="string"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override string Process(TimeSpan input)
        {
            return input.ToString(Format);
        }
    }
}