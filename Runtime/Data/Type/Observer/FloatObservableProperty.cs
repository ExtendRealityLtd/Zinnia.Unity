namespace Zinnia.Data.Type.Observer
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes of a <see cref="float"/>.
    /// </summary>
    public class FloatObservableProperty : ObservableProperty<float, FloatObservableProperty.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        /// <summary>
        /// The tolerance to consider the current value and the cached value equal.
        /// </summary>
        [Tooltip("The tolerance to consider the current value and the cached value equal.")]
        [SerializeField]
        private float _equalityTolerance = float.Epsilon;
        public float EqualityTolerance
        {
            get
            {
                return _equalityTolerance;
            }
            set
            {
                _equalityTolerance = value;
            }
        }

        /// <inheritdoc/>
        protected override bool Equals(float a, float b)
        {
            return a.ApproxEquals(b, EqualityTolerance);
        }
    }
}