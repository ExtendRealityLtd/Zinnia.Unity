namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Subtracts a collection of <see cref="Vector2"/>s by subtracting each one from the first entry in the collection.
    /// </summary>
    /// <example>
    /// Vector2.one - Vector2.one = Vector2(0f, 0f)
    /// </example>
    public class Vector2Subtractor : CollectionAggregator<Vector2, Vector2, Vector2Subtractor.UnityEvent, Vector2ObservableList, Vector2ObservableList.UnityEvent>
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

            Vector2 difference = Collection.NonSubscribableElements[0];
            for (int index = 1; index < Collection.NonSubscribableElements.Count; index++)
            {
                difference -= Collection.NonSubscribableElements[index];
            }

            return difference;
        }
    }
}