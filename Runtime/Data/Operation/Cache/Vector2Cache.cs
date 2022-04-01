namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    [Obsolete("Use `Zinnia.Data.Type.Observer.Vector2ObservableProperty` instead.")]
    public class Vector2Cache : ValueCache<Vector2, Vector2Cache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector2"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

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
        protected override bool AreEqual(Vector2 a, Vector2 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}