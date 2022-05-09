namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Transforms a boolean value to the equivalent float value.
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
        public class UnityEvent : UnityEvent<float> { }

        [Tooltip("The value to use if the boolean is false.")]
        [SerializeField]
        private float falseValue = 0f;
        /// <summary>
        /// The value to use if the boolean is false.
        /// </summary>
        public float FalseValue
        {
            get
            {
                return falseValue;
            }
            set
            {
                falseValue = value;
            }
        }
        [Tooltip("The value to use if the boolean is true.")]
        [SerializeField]
        private float trueValue = 1f;
        /// <summary>
        /// The value to use if the boolean is true.
        /// </summary>
        public float TrueValue
        {
            get
            {
                return trueValue;
            }
            set
            {
                trueValue = value;
            }
        }

        /// <summary>
        /// Transforms the given input <see cref="bool"/> to the <see cref="float"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override float Process(bool input)
        {
            return input ? TrueValue : FalseValue;
        }
    }
}