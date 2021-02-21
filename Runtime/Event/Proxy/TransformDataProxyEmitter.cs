namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine.Events;
    using Zinnia.Data.Type;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="TransformData"/> payload whenever <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> is called.
    /// </summary>
    public class TransformDataProxyEmitter : RestrictableSingleEventProxyEmitter<TransformData, TransformDataProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<TransformData> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload?.Transform != null ? Payload?.Transform.gameObject : null;
        }
    }
}