namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Multiplies a collection of <see cref="Vector2"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// (2f,3f) * [3f,4f] = (6f,12f)
    /// </example>
    public class Vector2Multiplier : CollectionAggregator<Vector2, Vector2, Vector2Multiplier.UnityEvent, Vector2ObservableList, Vector2ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// Sets the x value of the <see cref="ObservableList{TElement,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new x value.</param>
        public virtual void SetComponentX(float value)
        {
            Vector2 currentValue = Collection.NonSubscribableElements[Collection.CurrentIndex];
            currentValue.x = value;
            Collection.SetAtCurrentIndex(currentValue);
        }

        /// <summary>
        /// Sets the x value of the given index element.
        /// </summary>
        /// <param name="value">The new x value.</param>
        /// <param name="index">The index in the collection to update at.</param>
        public virtual void SetComponentX(float value, int index)
        {
            Collection.CurrentIndex = index;
            SetComponentX(value);
        }

        /// <summary>
        /// Sets the y value of the <see cref="ObservableList{TElement,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new y value.</param>
        public virtual void SetComponentY(float value)
        {
            Vector2 currentValue = Collection.NonSubscribableElements[Collection.CurrentIndex];
            currentValue.y = value;
            Collection.SetAtCurrentIndex(currentValue);
        }

        /// <summary>
        /// Sets the y value of the given index element.
        /// </summary>
        /// <param name="value">The new y value.</param>
        /// <param name="index">The index in the collection to update at.</param>
        public virtual void SetComponentY(float value, int index)
        {
            Collection.CurrentIndex = index;
            SetComponentY(value);
        }

        /// <inheritdoc />
        protected override Vector2 ProcessCollection()
        {
            Vector2 product = Vector2.one;
            foreach (Vector2 element in Collection.NonSubscribableElements)
            {
                product = Vector2.Scale(product, element);
            }

            return product;
        }
    }
}