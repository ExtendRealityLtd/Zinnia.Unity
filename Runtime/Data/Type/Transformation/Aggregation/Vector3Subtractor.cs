namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Subtracts a collection of <see cref="Vector3"/>s by subtracting each one from the first entry in the collection.
    /// </summary>
    /// <example>
    /// Vector3.one - Vector3.one = Vector3(0f, 0f, 0f)
    /// </example>
    public class Vector3Subtractor : CollectionAggregator<Vector3, Vector3, Vector3Subtractor.UnityEvent, Vector3ObservableList, Vector3ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <inheritdoc />
        protected override Vector3 ProcessCollection()
        {
            if (Collection.NonSubscribableElements.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 difference = Collection.NonSubscribableElements[0];
            for (int index = 1; index < Collection.NonSubscribableElements.Count; index++)
            {
                difference -= Collection.NonSubscribableElements[index];
            }

            return difference;
        }
    }
}