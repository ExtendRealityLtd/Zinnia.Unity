namespace Zinnia.Event.Proxy
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="Vector3"/> payload whenever the <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> method is called.
    /// </summary>
    public class Vector3EventProxyEmitter : SingleEventProxyEmitter<Vector3, Vector3EventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3>
        {
        }
    }
}