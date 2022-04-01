namespace Zinnia.Action
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="float"/> value.
    /// </summary>
    public class FloatAction : Action<FloatAction, float, FloatAction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float> { }

        [Tooltip("The tolerance of equality between two float values.")]
        [SerializeField]
        private float equalityTolerance = float.Epsilon;
        /// <summary>
        /// The tolerance of equality between two <see cref="float"/> values.
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
        protected override bool IsValueEqual(float value)
        {
            return Value.ApproxEquals(value, EqualityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(float value)
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