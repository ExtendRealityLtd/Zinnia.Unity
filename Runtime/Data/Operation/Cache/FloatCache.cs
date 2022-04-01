namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    [Obsolete("Use `Zinnia.Data.Type.Observer.FloatObservableProperty` instead.")]
    public class FloatCache : ValueCache<float, FloatCache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="float"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        [Tooltip("The tolerance to consider the current value and the cached value equal.")]
        [SerializeField]
        private float equalityTolerance = float.Epsilon;
        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        public float EqualityTolerance
        {
            get
            {
                return equalityTolerance;
            }
            set
            {
                equalityTolerance = value;
            }
        }

        /// <inheritdoc/>
        protected override bool AreEqual(float a, float b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}