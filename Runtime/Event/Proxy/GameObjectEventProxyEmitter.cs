namespace Zinnia.Event.Proxy
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Emits a UnityEvent with a GameObject payload whenever the Receive method is called.
    /// </summary>
    public class GameObjectEventProxyEmitter : RestrictableSingleEventProxyEmitter<GameObject, GameObjectEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        /// <inheritdoc />
        protected override object GetTargetToCheck()
        {
            return Payload;
        }
    }
}