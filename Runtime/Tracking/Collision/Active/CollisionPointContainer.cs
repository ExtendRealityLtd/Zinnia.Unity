namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
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
        /// Determines whether the collision point parent is the <see cref="GameObject"/> that contains a <see cref="CollisionNotifier"/> or to just search for the containing <see cref="Transform"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool IsParentCollisionNotifier { get; set; }

        /// <summary>
        /// Emitted when the collision point container is created.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent PointSet = new UnityEvent();
        /// <summary>
        /// Emitted when the collision point container is destroyed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent PointUnset = new UnityEvent();

        /// <summary>
        /// The created container.
        /// </summary>
        public GameObject Container
        {
            get;
            protected set;
        }
        /// <summary>
        /// Whether <see cref="Container"/> is <see cref="Set"/> or <see cref="Unset"/>.
        /// </summary>
        public bool IsSet { get; private set; }

        /// <summary>
        /// Creates a new container if it doesn't already exist and sets it to be at the point of the collision of the given event data.
        /// </summary>
        /// <param name="eventData">Contains data about the collision.</param>
        [RequiresBehaviourState]
        public virtual void Set(ActiveCollisionConsumer.EventData eventData)
        {
            if (IsSet)
            {
                return;
            }

            GameObject collisionInitiator = eventData.Publisher?.SourceContainer;
            Collider collisionCollider = eventData.CurrentCollision?.ColliderData;
            if (collisionInitiator == null || collisionCollider == null)
            {
                return;
            }

            GameObject collidingObject = IsParentCollisionNotifier
                ? collisionCollider.GetComponentInParent<CollisionNotifier>().gameObject
                : collisionCollider.GetContainingTransform().gameObject;

            if (Container == null)
            {
                Container = new GameObject();
            }

            Container.name = $"[Zinnia][CollisionPointContainer][{collisionInitiator.name}]";
            Container.transform.parent = collidingObject.transform;
            Container.transform.position = collisionInitiator.transform.position;
            Container.transform.rotation = collisionInitiator.transform.rotation;
            Container.transform.localScale = Vector3.one;
            Container.SetActive(true);

            IsSet = true;

            PointSet?.Invoke(Container);
        }

        /// <summary>
        /// Unsets the created container.
        /// </summary>
        public virtual void Unset()
        {
            if (!IsSet || Container == null)
            {
                return;
            }


            if (Container.activeInHierarchy && gameObject.activeInHierarchy)
            {
                Container.transform.parent = transform;
            }

            Container.SetActive(false);
            IsSet = false;
            PointUnset?.Invoke(Container);
        }

        protected virtual void OnDisable()
        {
            Unset();
            Destroy(Container);
            Container = null;
        }
    }
}