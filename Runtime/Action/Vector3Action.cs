namespace Zinnia.Action
{
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

        [Tooltip("The tolerance of equality between two Vector3 values.")]
        [SerializeField]
        private float equalityTolerance = float.Epsilon;
        /// <summary>
        /// The tolerance of equality between two <see cref="Vector3"/> values.
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
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterEqualityToleranceChange();
                }
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
        protected virtual void OnAfterEqualityToleranceChange()
        {
            Receive(Value);
        }
    }
}