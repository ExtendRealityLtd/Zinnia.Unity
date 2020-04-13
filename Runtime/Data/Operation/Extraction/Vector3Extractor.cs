namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Provides the basis for emitting a <see cref="Vector3?"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to retrieve the <see cref="Vector3?"/> from.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    public abstract class Vector3Extractor<TSource, TEvent> : ValueExtractor<Vector3?, TSource, TEvent, Vector3> where TEvent : UnityEvent<Vector3>, new()
    {
        /// <inheritdoc/>
        protected override bool InvokeResult(Vector3? data)
        {
            return InvokeEvent(data.GetValueOrDefault());
        }
    }
}