namespace Zinnia.Event.Proxy
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Type;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="TransformData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class TransformDataProxyEmitter : SingleEventProxyEmitter<TransformData, TransformDataProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TransformData>
        {
        }
    }
}