namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Multiplies a collection of <see cref="float"/>s by multiplying each one to the next entry in the collection.
    /// </summary>
    /// <example>
    /// 2f * 2f * 2f = 8f
    /// </example>
    public class FloatMultiplier : CollectionAggregator<float, float, FloatMultiplier.UnityEvent, FloatObservableList, FloatObservableList.UnityEvent>
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
            float product = 1f;
            foreach (float element in Collection.NonSubscribableElements)
            {
                product *= element;
            }

            return product;
        }
    }
}