namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Holds information about a tracked collision.
    /// </summary>
    public struct CollisionTrackerData
    {
        /// <summary>
        /// Whether the collision was observed through a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
        /// </summary>
        public bool isTrigger;
        /// <summary>
        /// The observed <see cref="Collision"/>. <see langword="null"/> if <see cref="isTrigger"/> is <see langword="true"/>.
        /// </summary>
        public Collision collision;
        /// <summary>
        /// The observed <see cref="Collider"/>.
        /// </summary>
        public Collider collider;
    }

    /// <summary>
    /// Tracks collisions on the <see cref="GameObject"/> this component is on.
    /// </summary>
    public class CollisionTracker : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="CollisionTrackerData"/> and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class CollisionTrackerUnityEvent : UnityEvent<CollisionTrackerData, object>
        {
        }

        /// <summary>
        /// Emitted when entering a collision.
        /// </summary>
        public CollisionTrackerUnityEvent CollisionEnter = new CollisionTrackerUnityEvent();
        /// <summary>
        /// Emitted as long as a collision happens.
        /// </summary>
        public CollisionTrackerUnityEvent CollisionStay = new CollisionTrackerUnityEvent();
        /// <summary>
        /// Emitted when exiting a collision.
        /// </summary>
        public CollisionTrackerUnityEvent CollisionExit = new CollisionTrackerUnityEvent();
        /// <summary>
        /// Emitted when entering a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
        /// </summary>
        public CollisionTrackerUnityEvent TriggerEnter = new CollisionTrackerUnityEvent();
        /// <summary>
        /// Emitted as long as a collision with a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set happens.
        /// </summary>
        public CollisionTrackerUnityEvent TriggerStay = new CollisionTrackerUnityEvent();
        /// <summary>
        /// Emitted when exiting a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
        /// </summary>
        public CollisionTrackerUnityEvent TriggerExit = new CollisionTrackerUnityEvent();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent(
                new CollisionTrackerData
                {
                    isTrigger = false,
                    collision = collision,
                    collider = collision.collider
                });
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            OnCollisionStayEvent(
                new CollisionTrackerData
                {
                    isTrigger = false,
                    collision = collision,
                    collider = collision.collider
                });
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            OnCollisionExitEvent(
                new CollisionTrackerData
                {
                    isTrigger = false,
                    collision = collision,
                    collider = collision.collider
                });
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            OnTriggerEnterEvent(
                new CollisionTrackerData
                {
                    isTrigger = true,
                    collision = null,
                    collider = collider
                });
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            OnTriggerStayEvent(
                new CollisionTrackerData
                {
                    isTrigger = true,
                    collision = null,
                    collider = collider
                });
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            OnTriggerExitEvent(
                new CollisionTrackerData
                {
                    isTrigger = true,
                    collision = null,
                    collider = collider
                });
        }

        protected void OnCollisionEnterEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                CollisionEnter?.Invoke(data, this);
            }
        }

        protected void OnCollisionStayEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                CollisionStay?.Invoke(data, this);
            }
        }

        protected void OnCollisionExitEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                CollisionExit?.Invoke(data, this);
            }
        }

        protected void OnTriggerEnterEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                TriggerEnter?.Invoke(data, this);
            }
        }

        protected void OnTriggerStayEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                TriggerStay?.Invoke(data, this);
            }
        }

        protected void OnTriggerExitEvent(CollisionTrackerData data)
        {
            if (isActiveAndEnabled)
            {
                TriggerExit?.Invoke(data, this);
            }
        }
    }
}
