namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Subtracts a collection of <see cref="float"/>s by subtracting each one from the first entry in the collection.
    /// </summary>
    /// <example>
    /// 1f - 1f = 0f
    /// </example>
    public class FloatSubtractor : CollectionAggregator<float, float, FloatSubtractor.UnityEvent, FloatObservableList, FloatObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="float"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <inheritdoc />
        protected override float ProcessCollection()
        {
            if (Collection.NonSubscribableElements.Count == 0)
            {
                return 0f;
            }

            float difference = Collection.NonSubscribableElements[0];
            for (int index = 1; index < Collection.NonSubscribableElements.Count; index++)
            {
                difference -= Collection.NonSubscribableElements[index];
            }

            return difference;
        }
    }
}