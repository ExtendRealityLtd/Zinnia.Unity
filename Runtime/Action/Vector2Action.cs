namespace Zinnia.Action
{
    using Malimbe.MemberChangeMethod;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="Vector2"/> value.
    /// </summary>
    public class Vector2Action : Action<Vector2Action, Vector2, Vector2Action.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector2"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <summary>
        /// The tolerance of equality between two <see cref="Vector2"/> values.
        /// </summary>
        [Tooltip("The tolerance of equality between two Vector2 values.")]
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

        /// <inheritdoc />
        protected override bool IsValueEqual(Vector2 value)
        {
            return Value.ApproxEquals(value, EqualityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(Vector2 value)
        {
            return !DefaultValue.ApproxEquals(value, EqualityTolerance);
        }

        /// <summary>
        /// Called after <see cref="EqualityTolerance"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(EqualityTolerance))]
        protected virtual void OnAfterEqualityToleranceChange()
        {
            Receive(Value);
        }
    }
}