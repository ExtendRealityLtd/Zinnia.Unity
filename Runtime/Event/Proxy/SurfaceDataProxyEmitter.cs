namespace Zinnia.Event.Proxy
{
    using UnityEngine.Events;
    using System;
    using Zinnia.Data.Type;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="SurfaceData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class SurfaceDataProxyEmitter : SingleEventProxyEmitter<SurfaceData, SurfaceDataProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<SurfaceData>
        {
        }
    }
}