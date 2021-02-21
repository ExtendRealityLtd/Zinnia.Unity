namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a integer value to the equivalent float value.
    /// </summary>
    /// <example>
    /// 1 = 1f
    /// 2 = 2f
    /// </example>
    public class IntegerToFloat : Transformer<int, float, IntegerToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// Transforms the given input <see cref="int"/> to the <see cref="float"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(int input)
        {
            return input;
        }
    }
}