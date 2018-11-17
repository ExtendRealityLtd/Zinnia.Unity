namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;

    /// <summary>
    /// Multiplies a collection of <see cref="Vector2"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// (2f,3f) * [3f,4f] = (6f,12f)
    /// </example>
    public class Vector2Multiplier : CollectionAggregator<Vector2, Vector2, Vector2Multiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// Sets the x value of the <see cref="CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new x value.</param>
        public virtual void SetElementX(float value)
        {
            Vector2 currentValue = collection[CurrentIndex];
            currentValue.x = value;
            collection[CurrentIndex] = currentValue;
        }

        /// <summary>
        /// Sets the x value of the given index element.
        /// </summary>
        /// <param name="index">The index in the collection to update at.</param>
        /// <param name="value">The new x value.</param>
        public virtual void SetElementX(int index, float value)
        {
            index = WrapAroundAndClamp(index);
            Vector2 currentValue = collection[index];
            currentValue.x = value;
            collection[index] = currentValue;
        }

        /// <summary>
        /// Sets the y value of the <see cref="CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new y value.</param>
        public virtual void SetElementY(float value)
        {
            Vector2 currentValue = collection[CurrentIndex];
            currentValue.y = value;
            collection[CurrentIndex] = currentValue;
        }

        /// <summary>
        /// Sets the y value of the given index element.
        /// </summary>
        /// <param name="index">The index in the collection to update at.</param>
        /// <param name="value">The new y value.</param>
        public virtual void SetElementY(int index, float value)
        {
            index = WrapAroundAndClamp(index);
            Vector2 currentValue = collection[index];
            currentValue.y = value;
            collection[index] = currentValue;
        }

        /// <inheritdoc />
        protected override Vector2 ProcessCollection()
        {
            return collection.Aggregate(Multiply);
        }

        /// <summary>
        /// Multiplies two <see cref="Vector2"/> values.
        /// </summary>
        /// <param name="multiplicand">The value to be multiplied.</param>
        /// <param name="multiplier">The value to multiply with.</param>
        /// <returns>The calculated value.</returns>
        protected virtual Vector2 Multiply(Vector2 multiplicand, Vector2 multiplier)
        {
            return Vector2.Scale(multiplicand, multiplier);
        }
    }
}