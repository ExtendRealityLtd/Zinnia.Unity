namespace VRTK.Core.Tracking.Collision.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Utility;

    /// <summary>
    /// Receives a broadcast from the <see cref="ActiveCollisionsBroadcaster"/>.
    /// </summary>
    public class ActiveCollisionsBroadcastReceiver : MonoBehaviour
    {
        /// <summary>
        /// Determines whether to digest the received broadcast from specific broadcasters.
        /// </summary>
        [Tooltip("Determines whether to digest the received broadcast from specific broadcasters.")]
        public ExclusionRule broadcasterValidity;

        /// <summary>
        /// The broadcaster that last initiated the receive.
        /// </summary>
        public ActiveCollisionsBroadcaster BroadcastSource
        {
            get;
            protected set;
        }

        /// <summary>
        /// Defines the event with a <see cref="GameObject"/> that is initiating the collision.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the collision initiator is set from a broadcast.
        /// </summary>
        public UnityEvent CollisionInitiatorSet = new UnityEvent();
        /// <summary>
        /// Emitted when the collision initiator is unset from a broadcast.
        /// </summary>
        public UnityEvent CollisionInitiatorUnset = new UnityEvent();
        /// <summary>
        /// Emitted when the receiver is cleared.
        /// </summary>
        public UnityEvent Cleared = new UnityEvent();

        /// <summary>
        /// Receives a broadcast from a <see cref="ActiveCollisionsBroadcaster"/>.
        /// </summary>
        /// <param name="broadcaster">The broadcaster of the message.</param>
        public virtual void Receive(ActiveCollisionsBroadcaster broadcaster)
        {
            if (!isActiveAndEnabled || ExclusionRule.ShouldExclude(broadcaster.gameObject, broadcasterValidity))
            {
                return;
            }

            BroadcastSource = broadcaster;
            if (BroadcastSource.collisionInitiator != null)
            {
                CollisionInitiatorSet?.Invoke(BroadcastSource.collisionInitiator);
            }
            else
            {
                Clear();
            }
        }

        /// <summary>
        /// Clears the existing received broadcast.
        /// </summary>
        public virtual void Clear()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Cleared?.Invoke(BroadcastSource?.collisionInitiator);
            CollisionInitiatorUnset?.Invoke(null);
            BroadcastSource = null;
        }
    }
}