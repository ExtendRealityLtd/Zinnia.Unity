namespace Zinnia.Action
{
    using Malimbe.MemberChangeMethod;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="Vector3"/> value.
    /// </summary>
    public class Vector3Action : Action<Vector3Action, Vector3, Vector3Action.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> { }

        /// <summary>
        /// The tolerance of equality between two <see cref="Vector3"/> values.
        /// </summary>
        [Tooltip("The tolerance of equality between two Vector3 values.")]
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
        protected override bool IsValueEqual(Vector3 value)
        {
            return Value.ApproxEquals(value, EqualityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(Vector3 value)
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