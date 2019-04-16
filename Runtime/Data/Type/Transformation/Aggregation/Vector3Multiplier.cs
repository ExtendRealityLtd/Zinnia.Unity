namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Multiplies a collection of <see cref="Vector3"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// (2f,3f,4f) * [3f,4f,5f] = (6f,12f,20f)
    /// </example>
    public class Vector3Multiplier : CollectionAggregator<Vector3, Vector3, Vector3Multiplier.UnityEvent, Vector3ObservableList, Vector3ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the multiplied <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// Sets the x value of the <see cref="ObservableList{TElement,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new x value.</param>
        public virtual void SetComponentX(float value)
        {
            Vector3 currentValue = Collection.NonSubscribableElements[Collection.CurrentIndex];
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
            Vector3 currentValue = Collection.NonSubscribableElements[Collection.CurrentIndex];
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

        /// <summary>
        /// Sets the z value of the <see cref="ObservableList{TElement,TEvent}.CurrentIndex"/> element.
        /// </summary>
        /// <param name="value">The new z value.</param>
        public virtual void SetComponentZ(float value)
        {
            Vector3 currentValue = Collection.NonSubscribableElements[Collection.CurrentIndex];
            currentValue.z = value;
            Collection.SetAtCurrentIndex(currentValue);
        }

        /// <summary>
        /// Sets the z value of the given index element.
        /// </summary>
        /// <param name="value">The new z value.</param>
        /// <param name="index">The index in the collection to update at.</param>
        public virtual void SetComponentZ(float value, int index)
        {
            Collection.CurrentIndex = index;
            SetComponentZ(value);
        }

        /// <inheritdoc />
        protected override Vector3 ProcessCollection()
        {
            Vector3 product = Vector3.one;
            foreach (Vector3 element in Collection.NonSubscribableElements)
            {
                product = Vector3.Scale(product, element);
            }

            return product;
        }
    }
}