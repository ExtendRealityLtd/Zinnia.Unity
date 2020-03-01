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
    /// Emits a <see cref="Vector3"/> value.
    /// </summary>
    public class Vector3Action : Action<Vector3Action, Vector3, Vector3Action.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="Vector3"/> state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }

        /// <summary>
        /// The tolerance of equality between two <see cref="Vector3"/> values.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

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