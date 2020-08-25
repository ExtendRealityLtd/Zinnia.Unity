namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine.Events;

    [Obsolete("Use `Zinnia.Data.Type.Observer.IntObservableProperty` instead.")]
    public class IntCache : ValueCache<int, IntCache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="int"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<int> { }
    }
}