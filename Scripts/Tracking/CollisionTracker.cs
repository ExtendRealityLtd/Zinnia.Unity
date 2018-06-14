namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;

    /// <summary>
    /// Tracks collisions on the <see cref="GameObject"/> this component is on.
    /// </summary>
    public class CollisionTracker : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="CollisionTracker"/> event.
        /// </summary>
        [Serializable]
        public class EventData
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

            public EventData Set(EventData source)
            {
                return Set(source.isTrigger, source.collision, source.collider);
            }

            public EventData Set(bool isTrigger, Collision collision, Collider collider)
            {
                this.isTrigger = isTrigger;
                this.collision = collision;
                this.collider = collider;
                return this;
            }

            public void Clear()
            {
                Set(default(bool), default(Collision), default(Collider));
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
        /// Emitted when entering a collision.
        /// </summary>
        public UnityEvent CollisionEnter = new UnityEvent();
        /// <summary>
        /// Emitted as long as a collision happens.
        /// </summary>
        public UnityEvent CollisionStay = new UnityEvent();
        /// <summary>
        /// Emitted when exiting a collision.
        /// </summary>
        public UnityEvent CollisionExit = new UnityEvent();
        /// <summary>
        /// Emitted when entering a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
        /// </summary>
        public UnityEvent TriggerEnter = new UnityEvent();
        /// <summary>
        /// Emitted as long as a collision with a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set happens.
        /// </summary>
        public UnityEvent TriggerStay = new UnityEvent();
        /// <summary>
        /// Emitted when exiting a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
        /// </summary>
        public UnityEvent TriggerExit = new UnityEvent();

        protected EventData eventData = new EventData();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            CollisionEnter?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            CollisionStay?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            CollisionExit?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            TriggerEnter?.Invoke(eventData.Set(true, null, collider));
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            TriggerStay?.Invoke(eventData.Set(true, null, collider));
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            TriggerExit?.Invoke(eventData.Set(true, null, collider));
        }
    }
}
