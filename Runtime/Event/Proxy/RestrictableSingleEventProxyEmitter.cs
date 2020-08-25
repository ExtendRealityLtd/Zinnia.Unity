namespace Zinnia.Event.Proxy
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// An Event Proxy Emitter that can be restricted by a <see cref="Rule"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of data for the event.</typeparam>
    /// <typeparam name="TEvent">The event that is emitted.</typeparam>
    public abstract class RestrictableSingleEventProxyEmitter<TValue, TEvent> : SingleEventProxyEmitter<TValue, TEvent> where TEvent : UnityEvent<TValue>, new()
    {
        /// <summary>
        /// Determines whether the received payload is valid to be re-emitted.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer ReceiveValidity { get; set; }

        /// <summary>
        /// Gets the target for the validity check.
        /// </summary>
        /// <returns>The target to check on.</returns>
        protected abstract object GetTargetToCheck();

        /// <inheritdoc />
        protected override bool IsValid()
        {
            return base.IsValid() && ReceiveValidity.Accepts(GetTargetToCheck());
        }
    }
}