namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Sums up a <see cref="Vector2"/> collection.
    /// </summary>
    /// <example>
    /// Vector2.one + Vector2.one = Vector2(2f, 2f)
    /// </example>
    public class Vector2Adder : CollectionAggregator<Vector2, Vector2, Vector2Adder.UnityEvent, Vector2ObservableList, Vector2ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="Vector2"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <inheritdoc />
        protected override Vector2 ProcessCollection()
        {
            if (Collection.NonSubscribableElements.Count == 0)
            {
                return Vector2.zero;
            }

            Vector2 sum = Vector2.zero;
            for (int index = 0; index < Collection.NonSubscribableElements.Count; index++)
            {
                sum += Collection.NonSubscribableElements[index];
            }

            return sum;
        }
    }
}