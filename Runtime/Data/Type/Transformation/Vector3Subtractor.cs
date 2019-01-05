namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;

    /// <summary>
    /// Subtracts a collection of <see cref="Vector3"/>s by subtracting each one from the first entry in the collection.
    /// </summary>
    /// <example>
    /// Vector3.one - Vector3.one = Vector3(0f, 0f, 0f)
    /// </example>
    public class Vector3Subtractor : CollectionAggregator<Vector3, Vector3, Vector3Subtractor.UnityEvent>
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
            return collection.Aggregate(Subtract);
        }

        /// <summary>
        /// Subtracts two <see cref="Vector3"/> values.
        /// </summary>
        /// <param name="subtractFrom">The value to subtract from.</param>
        /// <param name="subtractWith">The value to subtract with</param>
        /// <returns>The calculated value.</returns>
        protected virtual Vector3 Subtract(Vector3 subtractFrom, Vector3 subtractWith)
        {
            return subtractFrom - subtractWith;
        }
    }
}