namespace Zinnia.Event
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Rule;

    public abstract class RestrictableSingleEventProxyEmitter<TValue, TEvent> : SingleEventProxyEmitter<TValue, TEvent> where TEvent : UnityEvent<TValue>, new()
    {
        /// <summary>
        /// Determines whether the received payload is valid to be re-emitted.
        /// </summary>
        [DocumentedByXml]
        public RuleContainer receiveValidity;

        /// <summary>
        /// Gets the target for the validity check.
        /// </summary>
        /// <returns>The target to check on.</returns>
        protected abstract object GetTargetToCheck();

        /// <inheritdoc />
        protected override bool IsValid()
        {
            return (base.IsValid() && receiveValidity.Accepts(GetTargetToCheck()));
        }
    }
}