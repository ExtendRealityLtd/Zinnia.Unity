namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Zinnia.Extension;

    /// <summary>
    /// Contains a <see cref="GameObject"/> at the point of a collision from the event data of an <see cref="ActiveCollisionConsumer"/>.
    /// </summary>
    public class CollisionPointContainer : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the generated collision point <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Determines whether the collision point parent is the <see cref="GameObject"/> that contains a <see cref="CollisionNotifier"/> or just to search for the containing <see cref="Transform"/>.
        /// </summary>
        public bool parentIsCollisionNotifier;

        /// <summary>
        /// Emitted when the collision point container is created.
        /// </summary>
        public UnityEvent Created = new UnityEvent();
        /// <summary>
        /// Emitted when the collision point container is destroyed.
        /// </summary>
        public UnityEvent Destroyed = new UnityEvent();

        /// <summary>
        /// The created container.
        /// </summary>
        public GameObject Container
        {
            get;
            protected set;
        }

        /// <summary>
        /// Creates a new container if it already doesn't exist at the point of the collision in the given event data.
        /// </summary>
        /// <param name="eventData">Contains data about the collision.</param>
        public virtual void Create(ActiveCollisionConsumer.EventData eventData)
        {
            GameObject collisionInitiator = eventData.publisher == null ? null : eventData.publisher.sourceContainer;

            if (!isActiveAndEnabled || collisionInitiator == null || eventData.currentCollision?.collider == null || Container != null)
            {
                return;
            }

            GameObject collidingObject = (parentIsCollisionNotifier ? eventData.currentCollision.collider.GetComponentInParent<CollisionNotifier>().gameObject : eventData.currentCollision.collider.GetContainingTransform().gameObject);

            Container = new GameObject($"[Zinnia][CollisionPointContainer][{collisionInitiator.name}]");
            Container.transform.SetParent(collidingObject.transform);
            Container.transform.position = collisionInitiator.transform.position;
            Container.transform.rotation = collisionInitiator.transform.rotation;
            Container.transform.localScale = Vector3.one;

            Created?.Invoke(Container);
        }

        /// <summary>
        /// Destroys the existing created container data.
        /// </summary>
        public virtual void Destroy()
        {
            if (Container != null)
            {
                Destroyed?.Invoke(Container);
                DestroyContainer();
            }
        }

        /// <summary>
        /// Destroys the container object.
        /// </summary>
        protected virtual void DestroyContainer()
        {
            Destroy(Container);
            Container = null;
        }
    }
}