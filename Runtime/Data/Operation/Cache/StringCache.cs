namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine.Events;

    [Obsolete("Use `Zinnia.Data.Type.Observer.StringObservableProperty` instead.")]
    public class StringCache : ValueCache<string, StringCache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="string"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string> { }
    }
}