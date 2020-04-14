namespace Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Provides the basis for emitting the <see cref="GameObject"/> that any concrete implementation is residing on.
    /// </summary>
    /// <typeparam name="TSource">The source to retrieve the <see cref="GameObject"/> from.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    public abstract class GameObjectExtractor<TSource, TEvent> : ValueExtractor<GameObject, TSource, TEvent, GameObject> where TEvent : UnityEvent<GameObject>, new()
    {
        /// <inheritdoc/>
        protected override bool InvokeResult(GameObject data)
        {
            return InvokeEvent(data);
        }
    }
}