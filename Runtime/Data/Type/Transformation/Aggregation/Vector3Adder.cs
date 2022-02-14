namespace Zinnia.Data.Type.Transformation.Aggregation
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Sums up a <see cref="Vector3"/> collection.
    /// </summary>
    /// <example>
    /// Vector3.one + Vector3.one = Vector3(2f, 2f, 2f)
    /// </example>
    public class Vector3Adder : CollectionAggregator<Vector3, Vector3, Vector3Adder.UnityEvent, Vector3ObservableList, Vector3ObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the aggregated <see cref="Vector3"/> value.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <inheritdoc />
        protected override Vector3 ProcessCollection()
        {
            if (Collection.NonSubscribableElements.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 sum = Vector3.zero;
            for (int index = 0; index < Collection.NonSubscribableElements.Count; index++)
            {
                sum += Collection.NonSubscribableElements[index];
            }

            return sum;
        }
    }
}