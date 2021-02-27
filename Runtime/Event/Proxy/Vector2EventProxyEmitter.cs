namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a <see cref="UnityEvent"/> with a <see cref="Vector2"/> payload whenever the <see cref="SingleEventProxyEmitter{TValue,TEvent}.Receive"/> method is called.
    /// </summary>
    public class Vector2EventProxyEmitter : RestrictableSingleEventProxyEmitter<Vector2, Vector2EventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload;
        }
    }
}