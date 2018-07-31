namespace VRTK.Core.Event
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a UnityEvent with a single payload whenever the Receive method is called.
    /// </summary>
    /// <typeparam name="TSelf">The type of ProxyEmitter to emit.</typeparam>
    /// <typeparam name="TValue">The value for Receive,</typeparam>
    /// <typeparam name="TEvent">The event type to emit.</typeparam>
    public abstract class SingleEventProxyEmitter<TSelf, TValue, TEvent> : MonoBehaviour where TSelf : SingleEventProxyEmitter<TSelf, TValue, TEvent> where TEvent : UnityEvent<TValue>, new()
    {
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
            Emitted?.Invoke(payload);
        }
    }
}