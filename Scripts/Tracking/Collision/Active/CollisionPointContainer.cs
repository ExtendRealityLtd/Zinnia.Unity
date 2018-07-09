namespace VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Extension;

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
        /// Emitted when the collision point container is created.
        /// </summary>
        public UnityEvent Created = new UnityEvent();
        /// <summary>
        /// Emitted when the collision point container is cleared.
        /// </summary>
        public UnityEvent Cleared = new UnityEvent();

        /// <summary>
        /// The created container;
        /// </summary>
        public GameObject Container
        {
            get;
            protected set;
        }

        /// <summary>
        /// Creates a new container at the point of the collision in the given event data.
        /// </summary>
        /// <param name="eventData">Contains data about the collision.</param>
        public virtual void Create(ActiveCollisionConsumer.EventData eventData)
        {
            GameObject collisionInitiator = eventData.publisher == null ? null : eventData.publisher.sourceContainer;
            GameObject collidingObject = eventData.currentCollision?.collider == null ? null : eventData.currentCollision.collider.GetContainingTransform().gameObject;
            if (!isActiveAndEnabled || collisionInitiator == null || collidingObject == null)
            {
                return;
            }

            DestroyContainer();
            Container = new GameObject($"[VRTK][CollisionPointContainer][{collisionInitiator.name}][{collidingObject.name}]");
            Container.transform.SetParent(collisionInitiator.transform);
            Container.transform.position = collidingObject.transform.position;
            Container.transform.rotation = collidingObject.transform.rotation;
            Container.transform.localScale = Vector3.one;

            Created?.Invoke(Container);
        }

        /// <summary>
        /// Clears the existing created container data.
        /// </summary>
        public virtual void Clear()
        {
            Cleared?.Invoke(Container);
            DestroyContainer();
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