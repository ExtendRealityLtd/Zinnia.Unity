namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine.Events;
    using System;
    using System.Linq;

    /// <summary>
    /// Multiplies a collection of <see cref="float"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// 2f * 2f * 2f = 8f
    /// </example>
    public class FloatMultiplier : CollectionAggregator<float, float, FloatMultiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <inheritdoc />
        protected override float ProcessCollection()
        {
            return collection.Aggregate(Multiply);
        }

        /// <summary>
        /// Multiplies two <see cref="float"/> values.
        /// </summary>
        /// <param name="multiplicand">The value to be multiplied.</param>
        /// <param name="multiplier">The value to multiply with.</param>
        /// <returns>The calculated value.</returns>
        protected virtual float Multiply(float multiplicand, float multiplier)
        {
            return multiplicand * multiplier;
        }
    }
}