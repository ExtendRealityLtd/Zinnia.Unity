namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Utility;

    /// <summary>
    /// Allows emitting collision data via events.
    /// </summary>
    public class CollisionNotifier : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="CollisionTracker"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The source of this event in case it was forwarded.
            /// </summary>
            /// <remarks><see langword="null"/> if this event wasn't forwarded from anything.</remarks>
            public Component forwardSource;
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
                return Set(source.forwardSource, source.isTrigger, source.collision, source.collider);
            }

            public EventData Set(Component forwardSource, bool isTrigger, Collision collision, Collider collider)
            {
                this.forwardSource = forwardSource;
                this.isTrigger = isTrigger;
                this.collision = collision;
                this.collider = collider;
                return this;
            }

            public void Clear()
            {
                Set(default(Component), default(bool), default(Collision), default(Collider));
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
        [UnityFlags]
        [Tooltip("The types of collisions that events will be emitted for.")]
        public CollisionTypes emittedTypes = (CollisionTypes)(-1);
        /// <summary>
        /// Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.
        /// </summary>
        [Tooltip("Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.")]
        public ExclusionRule forwardingSourceValidity;

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

        /// <summary>
        /// Determines whether events should be emitted.
        /// </summary>
        /// <param name="data">The data to check.</param>
        /// <returns><see langword="true"/> if events should be emitted.</returns>
        protected virtual bool CanEmit(EventData data)
        {
            return (data.isTrigger && emittedTypes.HasFlag(CollisionTypes.Trigger)
                    || !data.isTrigger && emittedTypes.HasFlag(CollisionTypes.Collision))
                && (data.forwardSource == null
                    || !ExclusionRule.ShouldExclude(data.forwardSource.gameObject, forwardingSourceValidity));
        }

        protected virtual void OnCollisionStarted(EventData data)
        {
            if (!CanEmit(data))
            {
                return;
            }

            CollisionStarted?.Invoke(data);

            if (transform.IsChildOf(data.collider.transform))
            {
                return;
            }

            foreach (CollisionNotifier notifier in data.collider.gameObject.GetComponentsInChildren<CollisionNotifier>())
            {
                notifier.OnCollisionStarted(data);
            }
        }

        protected virtual void OnCollisionChanged(EventData data)
        {
            if (!CanEmit(data))
            {
                return;
            }

            CollisionChanged?.Invoke(data);

            if (transform.IsChildOf(data.collider.transform))
            {
                return;
            }

            foreach (CollisionNotifier notifier in data.collider.gameObject.GetComponentsInChildren<CollisionNotifier>())
            {
                notifier.OnCollisionChanged(data);
            }
        }

        protected virtual void OnCollisionStopped(EventData data)
        {
            if (!CanEmit(data))
            {
                return;
            }

            CollisionStopped?.Invoke(data);

            if (transform.IsChildOf(data.collider.transform))
            {
                return;
            }

            foreach (CollisionNotifier notifier in data.collider.gameObject.GetComponentsInChildren<CollisionNotifier>())
            {
                notifier.OnCollisionStopped(data);
            }
        }
    }
}