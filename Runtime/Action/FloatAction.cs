namespace Zinnia.Action
{
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
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
        public class UnityEvent : UnityEvent<float>
        {
        }

        /// <summary>
        /// The tolerance of equality between two <see cref="float"/> values.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float EqualityTolerance { get; set; } = float.Epsilon;

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
        [CalledAfterChangeOf(nameof(EqualityTolerance))]
        protected virtual void OnAfterEqualityToleranceChange()
        {
            Receive(Value);
        }
    }
}