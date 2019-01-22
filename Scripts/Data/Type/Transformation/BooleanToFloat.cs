namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Transforms a boolean value to the equvalent float value.
    /// </summary>
    /// <example>
    /// false = 0f
    /// true = 1f
    /// </example>
    public class BooleanToFloat : Transformer<bool, float, BooleanToFloat.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// Transforms the given input bool to the float equavalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(bool input)
        {
            return (input ? 1f : 0f);
        }
    }
}