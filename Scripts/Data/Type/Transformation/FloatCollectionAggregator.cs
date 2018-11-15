namespace VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Processes a given float and adds it to all other values in the collection and outputs the sum of all values.
    /// </summary>
    /// <example>
    /// 1f + 2f + 3f = 6f
    /// </example>
    public class FloatCollectionAggregator : Transformer<float, float, FloatCollectionAggregator.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="float"/> values.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The collection of floats to aggregate.
        /// </summary>
        [Tooltip("The collection of floats to aggregate.")]
        public List<float> collection = new List<float>();

        /// <summary>
        /// The current collection index to add the given value to.
        /// </summary>
        protected int currentIndex;

        /// <summary>
        /// Sets the collection index that will store a given input value. The given index will be clamped to the range of valid indexes.
        /// </summary>
        /// <param name="index">The index to set the collection value at.</param>
        public virtual void SetIndex(int index)
        {
            currentIndex = Mathf.Clamp(index, 0, collection.Count - 1);
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <param name="index">The index of the collection to set the value at.</param>
        /// <returns>The summed value of all values in the collection.</returns>
        public virtual float Transform(float input, int index)
        {
            SetIndex(index);
            return Transform(input);
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <param name="index">The index of the collection to set the value at.</param>
        public virtual void DoTransform(float input, int index)
        {
            Transform(input, index);
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the saved current index.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <returns>The summed value of all values in the collection.</returns>
        protected override float Process(float input)
        {
            if (currentIndex < 0 || currentIndex >= collection.Count)
            {
                return 0f;
            }

            collection[currentIndex] = input;
            return collection.Sum();
        }
    }
}