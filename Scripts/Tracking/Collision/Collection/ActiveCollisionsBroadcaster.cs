namespace VRTK.Core.Tracking.Collision.Collection
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Broadcasts a message to all <see cref="ActiveCollisionsBroadcastReceiver"/> found on the colliding <see cref="GameObject"/> containing the initiating <see cref="GameObject"/> of the collision.
    /// </summary>
    public class ActiveCollisionsBroadcaster : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="GameObject"/> that initiated the collision.
        /// </summary>
        [Tooltip("The GameObject that initiated the collision.")]
        public GameObject collisionInitiator;

        /// <summary>
        /// The active collisions.
        /// </summary>
        public List<CollisionNotifier.EventData> ActiveCollisions
        {
            get;
            protected set;
        } = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// Sets the active collisions by copying the given data.
        /// </summary>
        /// <param name="data">The data to copy.</param>
        public virtual void SetActiveCollisions(ActiveCollisionsContainer.EventData data)
        {
            if (data == null || data.activeCollisions == null || !isActiveAndEnabled)
            {
                return;
            }

            ActiveCollisions.Clear();
            ActiveCollisions.AddRange(data.activeCollisions);
        }

        /// <summary>
        /// Broadcasts a message to all <see cref="ActiveCollisionsBroadcastReceiver"/> components found on any of the active collisions.
        /// </summary>
        public virtual void Broadcast()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            foreach (CollisionNotifier.EventData currentCollision in ActiveCollisions)
            {
                foreach (ActiveCollisionsBroadcastReceiver receiver in GetNotifiers(currentCollision))
                {
                    if (receiver.isActiveAndEnabled)
                    {
                        receiver.Receive(this, currentCollision);
                    }
                }
            }
        }

        protected virtual IEnumerable<ActiveCollisionsBroadcastReceiver> GetNotifiers(CollisionNotifier.EventData data)
        {
            Transform reference = data.collider.GetContainingTransform();

            if (transform.IsChildOf(reference))
            {
                return Enumerable.Empty<ActiveCollisionsBroadcastReceiver>();
            }

            return reference.GetComponentsInChildren<ActiveCollisionsBroadcastReceiver>();
        }
    }
}