namespace Zinnia.Event.Proxy
{
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a UnityEvent with a single payload whenever the Receive method is called.
    /// </summary>
    /// <typeparam name="TValue">The value for Receive,</typeparam>
    /// <typeparam name="TEvent">The event type to emit.</typeparam>
    public abstract class SingleEventProxyEmitter<TValue, TEvent> : EventProxyEmitter where TEvent : UnityEvent<TValue>, new()
    {
        [Tooltip("The payload data to emit.")]
        [SerializeField]
        private TValue payload;
        /// <summary>
        /// The payload data to emit.
        /// </summary>
        public TValue Payload
        {
            get
            {
                return payload;
            }
            set
            {
                payload = value;
            }
        }

        /// <summary>
        /// Is emitted when Receive is called.
        /// </summary>
        public TEvent Emitted = new TEvent();

        /// <summary>
        /// Attempts to emit the Emitted event with the given payload.
        /// </summary>
        /// <param name="payload"></param>
        public virtual void Receive(TValue payload)
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void EmitPayload()
        {
            if (!this.IsValidState() || !IsValid())
            {
                return;
            }

            Emitted?.Invoke(Payload);
        }

        /// <summary>
        /// Clears the <see cref="Payload"/> to the default value.
        /// </summary>
        public virtual void ClearPayload()
        {
            Payload = default;
        }
    }
}