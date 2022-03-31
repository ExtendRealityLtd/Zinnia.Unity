namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using System;
    using System.Linq;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Finds the median (middle) value in a <see cref="float"/> collection.
    /// </summary>
    /// <example>
    /// [2f, 2f, 3f, 5f, 5f, 7f, 8f] = 5f
    /// [2f, 2f, 3f, 4f, 5f, 7f, 8f, 9f] = 4.5f
    /// [3f, 2f, 5f, 2f, 8f, 7f, 5f] = 5f
    /// </example>
    public class FloatMedianFinder : CollectionAggregator<float, float, FloatAdder.UnityEvent, FloatObservableList, FloatObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <inheritdoc />
        protected override float ProcessCollection()
        {
            int numberCount = Collection.NonSubscribableElements.Count;
            int halfIndex = numberCount / 2;
            int halfIndexLess1 = halfIndex - 1;
            IOrderedEnumerable<float> sortedNumbers = Collection.NonSubscribableElements.OrderBy(n => n);

            return (numberCount % 2) == 0
                ? (sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndexLess1)) * 0.5f
                : sortedNumbers.ElementAt(halfIndex);
        }
    }
}