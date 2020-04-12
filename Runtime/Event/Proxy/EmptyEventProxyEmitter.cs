namespace Zinnia.Event.Proxy
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a UnityEvent with a no payload whenever the Receive method is called.
    /// </summary>
    public class EmptyEventProxyEmitter : EventProxyEmitter
    {
        /// <summary>
        /// Is emitted when Receive is called.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Emitted = new UnityEvent();

        /// <summary>
        /// Attempts to emit the Emitted event.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Receive()
        {
            if (!IsValid())
            {
                return;
            }

            Emitted?.Invoke();
        }
    }
}