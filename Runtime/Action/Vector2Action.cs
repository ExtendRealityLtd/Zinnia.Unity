namespace Zinnia.Action
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
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
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

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