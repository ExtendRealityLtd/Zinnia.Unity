namespace VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Publishes itself to all <see cref="ActiveCollisionConsumer"/> components found within the current <see cref="ActiveCollisions"/> collection.
    /// </summary>
    public class ActiveCollisionPublisher : MonoBehaviour
    {
        /// <summary>
        /// The container of the source that is initiating the collision.
        /// </summary>
        [Tooltip("The container of the source that is initiating the collision.")]
        public GameObject sourceContainer;

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
        /// Publishes itself and the current collision to all <see cref="ActiveCollisionConsumer"/> components found on any of the active collisions.
        /// </summary>
        public virtual void Publish()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            foreach (CollisionNotifier.EventData currentCollision in ActiveCollisions)
            {
                foreach (ActiveCollisionConsumer consumer in GetConsumers(currentCollision))
                {
                    consumer.Consume(this, currentCollision);
                }
            }
        }

        protected virtual IEnumerable<ActiveCollisionConsumer> GetConsumers(CollisionNotifier.EventData data)
        {
            Transform reference = data.collider.GetContainingTransform();

            if (transform.IsChildOf(reference))
            {
                return Enumerable.Empty<ActiveCollisionConsumer>();
            }

            return reference.GetComponentsInChildren<ActiveCollisionConsumer>();
        }
    }
}