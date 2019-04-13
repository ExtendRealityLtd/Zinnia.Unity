namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Sums up a collection of <see cref="float"/>s.
    /// </summary>
    /// <example>
    /// 1f + 2f + 3f = 6f
    /// </example>
    public class FloatAdder : CollectionAggregator<float, float, FloatAdder.UnityEvent, FloatObservableList, FloatObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <inheritdoc />
        protected override float ProcessCollection()
        {
            float sum = 0f;
            foreach (float element in Collection.NonSubscribableElements)
            {
                sum += element;
            }

            return sum;
        }
    }
}