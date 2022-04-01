namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes of a <see cref="Vector2"/>.
    /// </summary>
    public class Vector2ObservableProperty : ObservableProperty<Vector2, Vector2ObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> state.
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
        protected override bool Equals(Vector2 a, Vector2 b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}