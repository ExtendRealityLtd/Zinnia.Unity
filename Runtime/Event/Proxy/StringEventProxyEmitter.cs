namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="string"/> payload whenever the <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> method is called.
    /// </summary>
    public class StringEventProxyEmitter : RestrictableSingleEventProxyEmitter<string, StringEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<string> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload;
        }
    }
}