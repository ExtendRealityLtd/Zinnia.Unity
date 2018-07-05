namespace VRTK.Core.Tracking.Collision.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Extracts the Collision Initiator from a given <see cref="ActiveCollisionsBroadcaster"/>.
    /// </summary>
    public class ActiveCollisionsBroadcasterCollisionInitiatorExtractor : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the collision initiator <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when the collision initiator is set from a broadcast.
        /// </summary>
        public UnityEvent Extracted = new UnityEvent();

        public GameObject CollisionInitiator
        {
            get;
            protected set;
        }

        public virtual GameObject Extract(ActiveCollisionsBroadcaster broadcaster)
        {
            if (!isActiveAndEnabled || broadcaster == null)
            {
                CollisionInitiator = null;
                return null;
            }

            CollisionInitiator = broadcaster.collisionInitiator;
            Extracted?.Invoke(CollisionInitiator);
            return CollisionInitiator;
        }

        public virtual GameObject Extract(ActiveCollisionsBroadcastReceiver.EventData receiverData)
        {
            return Extract(receiverData?.broadcaster);
        }

        public virtual void DoExtract(ActiveCollisionsBroadcaster broadcaster)
        {
            Extract(broadcaster);
        }

        public virtual void DoExtract(ActiveCollisionsBroadcastReceiver.EventData receiverData)
        {
            Extract(receiverData?.broadcaster);
        }
    }
}