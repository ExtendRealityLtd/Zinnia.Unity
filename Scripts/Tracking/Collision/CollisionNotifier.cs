namespace Zinnia.Tracking.Collision
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zinnia.Data.Attribute;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// Allows emitting collision data via events.
    /// </summary>
    public class CollisionNotifier : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="CollisionTracker"/> event.
        /// </summary>
        [Serializable]
        public class EventData : IEquatable<EventData>
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

            /// <inheritdoc />
            public bool Equals(EventData other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Equals(collider.GetContainingTransform(), other.collider.GetContainingTransform());
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((EventData)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return collider.GetContainingTransform().GetHashCode();
            }

            public static bool operator ==(EventData left, EventData right) => Equals(left, right);
            public static bool operator !=(EventData left, EventData right) => !Equals(left, right);
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
        [Tooltip("The types of collisions that events will be emitted for."), UnityFlags]
        public CollisionTypes emittedTypes = (CollisionTypes)(-1);
        /// <summary>
        /// Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.
        /// </summary>
        [Tooltip("Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.")]
        public RuleContainer forwardingSourceValidity;

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
                    || forwardingSourceValidity.Accepts(data.forwardSource.gameObject));
        }

        /// <summary>
        /// Returns a <see cref="CollisionNotifier"/> collection for the given <see cref="EventData"/> containing <see cref="Transform"/>
        /// </summary>
        /// <param name="data">The <see cref="EventData"/> that holds the containing <see cref="Transform"/></param>
        /// <returns>A <see cref="CollisionNotifier"/> collection for items found on the containing <see cref="Transform"/> component.</returns>
        protected virtual IEnumerable<CollisionNotifier> GetNotifiers(EventData data)
        {
            Transform reference = data.collider.GetContainingTransform();

            if (transform.IsChildOf(reference))
            {
                return Enumerable.Empty<CollisionNotifier>();
            }

            return reference.GetComponentsInChildren<CollisionNotifier>();
        }

        protected virtual void OnCollisionStarted(EventData data)
        {
            if (!CanEmit(data))
            {
                return;
            }

            CollisionStarted?.Invoke(data);

            foreach (CollisionNotifier notifier in GetNotifiers(data))
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

            foreach (CollisionNotifier notifier in GetNotifiers(data))
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

            foreach (CollisionNotifier notifier in GetNotifiers(data))
            {
                notifier.OnCollisionStopped(data);
            }
        }
    }
}