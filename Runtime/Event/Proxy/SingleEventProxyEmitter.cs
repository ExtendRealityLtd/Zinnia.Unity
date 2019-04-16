namespace Zinnia.Event.Proxy
{
    using UnityEngine.Events;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Emits a UnityEvent with a single payload whenever the Receive method is called.
    /// </summary>
    /// <typeparam name="TValue">The value for Receive,</typeparam>
    /// <typeparam name="TEvent">The event type to emit.</typeparam>
    public abstract class SingleEventProxyEmitter<TValue, TEvent> : EventProxyEmitter where TEvent : UnityEvent<TValue>, new()
    {
        /// <summary>
        /// The most recent received payload.
        /// </summary>
        public TValue Payload { get; protected set; }

        /// <summary>
        /// Is emitted when Receive is called.
        /// </summary>
        [DocumentedByXml]
        public TEvent Emitted = new TEvent();

        /// <summary>
        /// Attempts to emit the Emitted event with the given payload.
        /// </summary>
        /// <param name="payload"></param>
        [RequiresBehaviourState]
        public virtual void Receive(TValue payload)
        {
            TValue previousPayloadValue = Payload;
            Payload = payload;

            if (!IsValid())
            {
                Payload = previousPayloadValue;
                return;
            }

            EmitPayload();
        }

        /// <summary>
        /// Emits the last received payload.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EmitPayload()
        {
            if (!IsValid())
            {
                return;
            }

            Emitted?.Invoke(Payload);
        }
    }
}