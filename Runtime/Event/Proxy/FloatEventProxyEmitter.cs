namespace Zinnia.Event.Proxy
{
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="float"/> payload whenever the <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> method is called.
    /// </summary>
    public class FloatEventProxyEmitter : SingleEventProxyEmitter<float, FloatEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<float>
        {
        }
    }
}