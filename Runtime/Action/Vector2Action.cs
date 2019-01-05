namespace Zinnia.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
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
        public class UnityEvent : UnityEvent<Vector2>
        {
        }

        /// <summary>
        /// The tolerance of equality between two <see cref="Vector2"/> values.
        /// </summary>
        public float equalityTolerance = float.Epsilon;

        /// <inheritdoc />
        protected override bool IsValueEqual(Vector2 value)
        {
            return Value.ApproxEquals(value, equalityTolerance);
        }

        /// <inheritdoc />
        protected override bool ShouldActivate(Vector2 value)
        {
            return !defaultValue.ApproxEquals(value, equalityTolerance);
        }
    }
}