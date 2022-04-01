namespace Zinnia.Data.Operation.Cache
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    [Obsolete("Use `Zinnia.Data.Type.Observer.Vector3ObservableProperty` instead.")]
    public class Vector3Cache : ValueCache<Vector3, Vector3Cache.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="Vector3"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

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
        protected override bool AreEqual(Vector3 a, Vector3 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}