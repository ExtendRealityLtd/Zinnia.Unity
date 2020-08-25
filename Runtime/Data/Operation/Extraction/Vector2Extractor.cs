namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Provides the basis for emitting a <see cref="Vector2"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to retrieve the <see cref="Vector2"/> from.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    public abstract class Vector2Extractor<TSource, TEvent> : ValueExtractor<Vector2?, TSource, TEvent, Vector2> where TEvent : UnityEvent<Vector2>, new()
    {
        /// <inheritdoc/>
        protected override bool InvokeResult(Vector2? data)
        {
            return InvokeEvent(data.GetValueOrDefault());
        }
    }
}