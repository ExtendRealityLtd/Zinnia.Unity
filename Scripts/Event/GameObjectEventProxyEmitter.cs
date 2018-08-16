namespace VRTK.Core.Event
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Rule;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a UnityEvent with a GameObject payload whenever the Receive method is called.
    /// </summary>
    public class GameObjectEventProxyEmitter : SingleEventProxyEmitter<GameObject, GameObjectEventProxyEmitter.UnityEvent>
    {
        /// <summary>
        /// Determines whether the received <see cref="GameObject"/> is valid to be re-emitted.
        /// </summary>
        [Tooltip("Determines whether the received GameObject is valid to be re-emitted.")]
        public RuleContainer receiveValidity;

        /// <summary>
        /// Defines the event with the specified state.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <inheritdoc />
        protected override bool IsValid()
        {
            return (base.IsValid() && receiveValidity.Accepts(Payload));
        }
    }
}