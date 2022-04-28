namespace Zinnia.Data.Type.Transformation.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Transforms a integer value to the equivalent float value.
    /// </summary>
    /// <example>
    /// 1 = 1f
    /// 2 = 2f
    /// </example>
    public class FloatToInteger : Transformer<float, int, FloatToInteger.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the transformed <see cref="int"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<int> { }

        /// <summary>
        /// The ways of rounding the <see cref="float"/>.
        /// </summary>
        public enum RoundingType
        {
            /// <summary>
            /// Uses <see cref="Mathf.Round"/> to return the nearest integer.
            /// </summary>
            Round,
            /// <summary>
            /// Uses <see cref="Mathf.Floor"/> to return the lowest integer.
            /// </summary>
            Floor,
            /// <summary>
            /// Uses <see cref="Mathf.Ceil"/> to return the highest integer.
            /// </summary>
            Ceil
        }

        [Tooltip("Determines how to round the float to a whole int.")]
        [SerializeField]
        private RoundingType roundBy;
        /// <summary>
        /// Determines how to round the <see cref="float"/> to a whole <see cref="int"/>.
        /// </summary>
        public RoundingType RoundBy
        {
            get
            {
                return roundBy;
            }
            set
            {
                roundBy = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="RoundBy"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="RoundingType"/>.</param>
        public virtual void SetRoundBy(int index)
        {
            RoundBy = EnumExtensions.GetByIndex<RoundingType>(index);
        }

        /// <summary>
        /// Transforms the given input <see cref="float"/> to the <see cref="int"/> equivalent value.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected override int Process(float input)
        {
            switch (RoundBy)
            {
                case RoundingType.Round:
                    return Mathf.RoundToInt(input);
                case RoundingType.Floor:
                    return Mathf.FloorToInt(input);
                case RoundingType.Ceil:
                    return Mathf.CeilToInt(input);
            }

            return (int)input;
        }
    }
}