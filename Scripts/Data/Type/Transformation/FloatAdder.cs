namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine.Events;
    using System;
    using System.Linq;

    /// <summary>
    /// Sums up a collection of <see cref="float"/>s.
    /// </summary>
    /// <example>
    /// 1f + 2f + 3f = 6f
    /// </example>
    public class FloatAdder : CollectionAggregator<float, float, FloatAdder.UnityEvent>
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
            return collection.Sum();
        }
    }
}