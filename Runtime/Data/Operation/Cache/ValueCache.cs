namespace Zinnia.Data.Operation.Cache
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Caches <see cref="TValue"/> data and invokves an appropriate event when the cache is updated.
    /// </summary>
    /// <typeparam name="TValue">The type of data to cache.</typeparam>
    /// <typeparam name="TEvent">The event to invoke.</typeparam>
    [Obsolete("Use `Zinnia.Data.Type.Observer.ObservableProperty` instead.")]
    public abstract class ValueCache<TValue, TEvent> : MonoBehaviour where TEvent : UnityEvent<TValue>, new()
    {
        /// <summary>
        /// Emitted when the cached data is updated and has been modified to a new value.
        /// </summary>
        public TEvent Modified = new TEvent();
        /// <summary>
        /// Emitted when the cached data is updated but the value is unmodified.
        /// </summary>
        public TEvent Unmodified = new TEvent();

        /// <summary>
        /// The cache of data.
        /// </summary>
        public TValue Data
        {
            get
            {
                return data;
            }
            set
            {
                if (!this.IsValidState())
                {
                    return;
                }

                if (AreEqual(value, data))
                {
                    Unmodified?.Invoke(value);
                }
                else
                {
                    Modified?.Invoke(value);
                }
                data = value;
            }
        }
        /// <summary>
        /// The backing field for <see cref="Data"/>.
        /// </summary>
        private TValue data;

        /// <summary>
        /// Clears the cache by setting it to the default value.
        /// </summary>
        public virtual void ClearCache()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Data = default;
        }

        /// <summary>
        /// Determines if the two given values are considered equal.
        /// </summary>
        /// <param name="a">The first value to check.</param>
        /// <param name="b">The second value to check.</param>
        /// <returns>Whether the two given values are considered equal.</returns>
        protected virtual bool AreEqual(TValue a, TValue b)
        {
            return EqualityComparer<TValue>.Default.Equals(a, b);
        }
    }
}