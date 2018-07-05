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
        /// Holds data about a <see cref="ActiveCollisionsBroadcastReceiver"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The broadcaster that is calling the receiver.
            /// </summary>
            public ActiveCollisionsBroadcaster broadcaster;
            /// <summary>
            /// The current collision data.
            /// </summary>
            public CollisionNotifier.EventData currentCollision;

            public EventData Set(EventData source)
            {
                return Set(source.broadcaster, source.currentCollision);
            }

            public EventData Set(ActiveCollisionsBroadcaster broadcaster, CollisionNotifier.EventData currentCollision)
            {
                this.broadcaster = broadcaster;
                this.currentCollision = currentCollision;
                return this;
            }

            public void Clear()
            {
                Set(default(ActiveCollisionsBroadcaster), default(CollisionNotifier.EventData));
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

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
        /// The current collision data from the broadcaster.
        /// </summary>
        public CollisionNotifier.EventData CurrentCollision
        {
            get;
            protected set;
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

        protected EventData eventData = new EventData();

        /// <summary>
        /// Receives a broadcast from a <see cref="ActiveCollisionsBroadcaster"/>.
        /// </summary>
        /// <param name="broadcaster">The broadcaster of the message.</param>
        /// <param name="currentCollision">The current collision within the received broadcast.</param>
        public virtual void Receive(ActiveCollisionsBroadcaster broadcaster, CollisionNotifier.EventData currentCollision)
        {
            if (!isActiveAndEnabled || ExclusionRule.ShouldExclude(broadcaster.gameObject, broadcasterValidity))
            {
                return;
            }

            BroadcastSource = broadcaster;
            CurrentCollision = currentCollision;
            if (BroadcastSource.collisionInitiator != null)
            {
                CollisionInitiatorSet?.Invoke(eventData.Set(BroadcastSource, currentCollision));
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

            Cleared?.Invoke(eventData.Set(BroadcastSource, CurrentCollision));
            CollisionInitiatorUnset?.Invoke(eventData.Set(null, null));
            BroadcastSource = null;
            CurrentCollision = null;
        }
    }
}