namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine.Events;

    /// <summary>
    /// Provides the basis for emitting a <see cref="float"/>.
    /// </summary>
    /// <typeparam name="TSource">The source to retrieve the <see cref="float"/> from.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    public abstract class FloatExtractor<TSource, TEvent> : ValueExtractor<float?, TSource, TEvent, float> where TEvent : UnityEvent<float>, new()
    {
        /// <inheritdoc/>
        protected override bool InvokeResult(float? data)
        {
            if (Result == null)
            {
                return false;
            }

            Extracted?.Invoke(data.GetValueOrDefault());
            return true;
        }
    }
}