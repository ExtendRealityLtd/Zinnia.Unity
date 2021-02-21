namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a string value to the equivalent TimeSpan value.
    /// </summary>
    public class StringToTimeSpan : Transformer<string, TimeSpan, StringToTimeSpan.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="TimeSpan"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TimeSpan> { }

        /// <summary>
        /// Transforms the given input <see cref="string"/> to the <see cref="TimeSpan"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override TimeSpan Process(string input)
        {
            return TimeSpan.Parse(input);
        }
    }
}