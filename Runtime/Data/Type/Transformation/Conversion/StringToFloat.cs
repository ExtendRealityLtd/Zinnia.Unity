namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a string value to the equivalent float value.
    /// </summary>
    /// <example>
    /// "1" = 1f
    /// "2.2" = 2.2f
    /// "x" = throw <see cref="FormatException"/>
    /// </example>
    public class StringToFloat : Transformer<string, float, StringToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// Transforms the given input <see cref="string"/> to the <see cref="float"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(string input)
        {
            return float.Parse(input);
        }
    }
}