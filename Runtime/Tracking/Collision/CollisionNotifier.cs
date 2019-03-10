namespace Zinnia.Tracking.Collision
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Rule;
    using Zinnia.Extension;
    using Zinnia.Data.Attribute;

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
            [Serialized, Cleared]
            [field: DocumentedByXml]
            public Component ForwardSource { get; set; }
            /// <summary>
            /// Whether the collision was observed through a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsTrigger { get; set; }
            /// <summary>
            /// The observed <see cref="Collision"/>. <see langword="null"/> if <see cref="IsTrigger"/> is <see langword="true"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public Collision CollisionData { get; set; }
            /// <summary>
            /// The observed <see cref="Collider"/>.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public Collider ColliderData { get; set; }

            public EventData Set(EventData source)
            {
                return Set(source.ForwardSource, source.IsTrigger, source.CollisionData, source.ColliderData);
            }

            public EventData Set(Component forwardSource, bool isTrigger, Collision collision, Collider collider)
            {
                ForwardSource = forwardSource;
                IsTrigger = isTrigger;
                CollisionData = collision;
                ColliderData = collider;
                return this;
            }

            public void Clear()
            {
                Set(default, default, default, default);
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

                return Equals(ColliderData.GetContainingTransform(), other.ColliderData.GetContainingTransform());
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
                return ColliderData.GetContainingTransform().GetHashCode();
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
        [Serialized]
        [field: DocumentedByXml, UnityFlags]
        public CollisionTypes EmittedTypes { get; set; } = (CollisionTypes)(-1);
        /// <summary>
        /// Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public RuleContainer ForwardingSourceValidity { get; set; }

        /// <summary>
        /// Emitted when a collision starts.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent CollisionStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision changes.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent CollisionChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision stops.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent CollisionStopped = new UnityEvent();

        /// <summary>
        /// A reused instance to use when raising any of the events.
        /// </summary>
        protected readonly EventData eventData = new EventData();
        /// <summary>
        /// A reused instance to use when looking up <see cref="CollisionNotifier"/> components.
        /// </summary>
        protected readonly List<CollisionNotifier> collisionNotifiers = new List<CollisionNotifier>();

        /// <summary>
        /// Determines whether events should be emitted.
        /// </summary>
        /// <param name="data">The data to check.</param>
        /// <returns><see langword="true"/> if events should be emitted.</returns>
        protected virtual bool CanEmit(EventData data)
        {
            return (data.IsTrigger && (EmittedTypes & CollisionTypes.Trigger) != 0
                    || !data.IsTrigger && (EmittedTypes & CollisionTypes.Collision) != 0)
                && (data.ForwardSource == null
                    || ForwardingSourceValidity.Accepts(data.ForwardSource.gameObject));
        }

        /// <summary>
        /// Returns a <see cref="CollisionNotifier"/> collection for the given <see cref="EventData"/> containing <see cref="Transform"/>
        /// </summary>
        /// <param name="data">The <see cref="EventData"/> that holds the containing <see cref="Transform"/></param>
        /// <returns>A <see cref="CollisionNotifier"/> collection for items found on the containing <see cref="Transform"/> component.</returns>
        protected virtual List<CollisionNotifier> GetNotifiers(EventData data)
        {
            Transform reference = data.ColliderData.GetContainingTransform();

            if (transform.IsChildOf(reference))
            {
                collisionNotifiers.Clear();
            }
            else
            {
                reference.GetComponentsInChildren(collisionNotifiers);
            }

            return collisionNotifiers;
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