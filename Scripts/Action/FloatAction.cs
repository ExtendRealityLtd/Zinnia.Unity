namespace VRTK.Core.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="float"/> value.
    /// </summary>
    public class FloatAction : BaseAction<float, FloatAction.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="float"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The tolerance of equality between two <see cref="float"/> values.
        /// </summary>
        [Tooltip("The tolerance of equality between two float values.")]
        public float equalityTolerance = float.Epsilon;

        /// <inheritdoc />
        protected override bool IsValueEqual(float value)
        {
            return Value.ApproxEquals(value, equalityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(float value)
        {
            return !defaultValue.ApproxEquals(value, equalityTolerance);
        }
    }
}