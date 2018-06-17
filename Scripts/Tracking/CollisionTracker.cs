namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Data.Attribute;

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
        /// The types of collisions that events will be emitted for.
        /// </summary>
        [Flags]
        public enum CollisionTypes
        {
            /// <summary>
            /// A regular, non-trigger collision.
            /// </summary>
            Collision = 1 << 0,
            /// <summary>
            /// A trigger collision
            /// </summary>
            Trigger = 1 << 1
        }

        /// <summary>
        /// The types of collisions that events will be emitted for.
        /// </summary>
        [UnityFlag]
        [Tooltip("The types of collisions that events will be emitted for.")]
        public CollisionTypes emittedTypes = (CollisionTypes)(-1);

        /// <summary>
        /// Emitted when a collision starts.
        /// </summary>
        public UnityEvent CollisionStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision changes.
        /// </summary>
        public UnityEvent CollisionChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision stops.
        /// </summary>
        public UnityEvent CollisionStopped = new UnityEvent();

        protected EventData eventData = new EventData();

        protected virtual void OnCollisionEnter(Collision collision)
        {
            CollisionStarted?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            CollisionChanged?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            CollisionStopped?.Invoke(eventData.Set(false, collision, collision.collider));
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            CollisionStarted?.Invoke(eventData.Set(true, null, collider));
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            CollisionChanged?.Invoke(eventData.Set(true, null, collider));
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            CollisionStopped?.Invoke(eventData.Set(true, null, collider));
        }
    }
}
