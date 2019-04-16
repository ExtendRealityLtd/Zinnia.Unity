namespace Zinnia.Event.Proxy
{
    using UnityEngine.Events;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Rule;
    using Zinnia.Extension;

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