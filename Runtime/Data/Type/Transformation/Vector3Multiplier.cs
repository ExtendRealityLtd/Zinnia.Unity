namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Zinnia.Extension;

    /// <summary>
    /// Multiplies a collection of <see cref="Vector3"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// (2f,3f,4f) * [3f,4f,5f] = (6f,12f,20f)
    /// </example>
    public class Vector3Multiplier : CollectionAggregator<Vector3, Vector3, Vector3Multiplier.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// Sets the x value of the <see cref="CollectionAggregator{TInput,TOutput,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new x value.</param>
        public virtual void SetElementX(float value)
        {
            Vector3 currentValue = collection[CurrentIndex];
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
            index = collection.GetWrappedAndClampedIndex(index);
            Vector3 currentValue = collection[index];
            currentValue.x = value;
            collection[index] = currentValue;
        }

        /// <summary>
        /// Sets the y value of the <see cref="CollectionAggregator{TInput,TOutput,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new y value.</param>
        public virtual void SetElementY(float value)
        {
            Vector3 currentValue = collection[CurrentIndex];
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
            index = collection.GetWrappedAndClampedIndex(index);
            Vector3 currentValue = collection[index];
            currentValue.y = value;
            collection[index] = currentValue;
        }

        /// <summary>
        /// Sets the z value of the <see cref="CollectionAggregator{TInput,TOutput,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new z value.</param>
        public virtual void SetElementZ(float value)
        {
            Vector3 currentValue = collection[CurrentIndex];
            currentValue.z = value;
            collection[CurrentIndex] = currentValue;
        }

        /// <summary>
        /// Sets the z value of the given index element.
        /// </summary>
        /// <param name="index">The index in the collection to update at.</param>
        /// <param name="value">The new z value.</param>
        public virtual void SetElementZ(int index, float value)
        {
            index = collection.GetWrappedAndClampedIndex(index);
            Vector3 currentValue = collection[index];
            currentValue.z = value;
            collection[index] = currentValue;
        }

        /// <inheritdoc />
        protected override Vector3 ProcessCollection()
        {
            Vector3 product = Vector3.one;
            foreach (Vector3 element in collection)
            {
                product = Vector3.Scale(product, element);
            }

            return product;
        }
    }
}