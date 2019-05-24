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

                return Equals(ForwardSource, other.ForwardSource) && Equals(ColliderData, other.ColliderData);
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

                return obj is EventData other && Equals(other);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return ((ForwardSource != null ? ForwardSource.GetHashCode() : 0) * 397) ^ (ColliderData != null ? ColliderData.GetHashCode() : 0);
            }

            public static bool operator ==(EventData left, EventData right) => Equals(left, right);
            public static bool operator !=(EventData left, EventData right) => !Equals(left, right);
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData> { }

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
        /// The states of a tracked collision.
        /// </summary>
        [Flags]
        public enum CollisionStates
        {
            /// <summary>
            /// When a new collision occurs.
            /// </summary>
            Enter = 1 << 0,
            /// <summary>
            /// When an existing collision continues to exist.
            /// </summary>
            Stay = 1 << 1,
            /// <summary>
            /// When an existing collision ends.
            /// </summary>
            Exit = 1 << 2
        }

        /// <summary>
        /// The types of collisions that events will be emitted for.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, UnityFlags]
        public CollisionTypes EmittedTypes { get; set; } = (CollisionTypes)(-1);
        /// <summary>
        /// The <see cref="CollisionStates"/> to process.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, UnityFlags]
        public CollisionStates StatesToProcess { get; set; } = (CollisionStates)(-1);
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
        /// A reused instance to use when looking up <see cref="CollisionNotifier"/> components on start.
        /// </summary>
        protected readonly List<CollisionNotifier> startCollisionNotifiers = new List<CollisionNotifier>();
        /// <summary>
        /// A reused instance to use when looking up <see cref="CollisionNotifier"/> components on change.
        /// </summary>
        protected readonly List<CollisionNotifier> changeCollisionNotifiers = new List<CollisionNotifier>();
        /// <summary>
        /// A reused instance to use when looking up <see cref="CollisionNotifier"/> components on stop.
        /// </summary>
        protected readonly List<CollisionNotifier> stopCollisionNotifiers = new List<CollisionNotifier>();

        /// <summary>
        /// Whether the <see cref="collisionNotifiers"/> collection is being processed on start.
        /// </summary>
        protected bool isProcessingStartNotifierCollection;
        /// <summary>
        /// Whether the <see cref="collisionNotifiers"/> collection is being processed on change.
        /// </summary>
        protected bool isProcessingChangeNotifierCollection;
        /// <summary>
        /// Whether the <see cref="collisionNotifiers"/> collection is being processed on stop.
        /// </summary>
        protected bool isProcessingStopNotifierCollection;

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
        /// Returns a <see cref="CollisionNotifier"/> collection for the given <see cref="EventData"/> containing <see cref="Transform"/>.
        /// </summary>
        /// <param name="data">The <see cref="EventData"/> that holds the containing <see cref="Transform"/></param>
        /// <returns>A <see cref="CollisionNotifier"/> collection for items found on the containing <see cref="Transform"/> component.</returns>
        protected virtual List<CollisionNotifier> GetNotifiers(EventData data, List<CollisionNotifier> collisionNotifiers)
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

        /// <summary>
        /// Processes any collision start events on the given data and propagates it to any linked <see cref="CollisionNotifier"/>.
        /// </summary>
        /// <param name="data">The collision data.</param>
        protected virtual void OnCollisionStarted(EventData data)
        {
            if ((StatesToProcess & CollisionStates.Enter) == 0 || !CanEmit(data))
            {
                return;
            }

            CollisionStarted?.Invoke(data);

            if (isProcessingStartNotifierCollection)
            {
                return;
            }

            isProcessingStartNotifierCollection = true;
            foreach (CollisionNotifier notifier in GetNotifiers(data, startCollisionNotifiers))
            {
                notifier.OnCollisionStarted(data);
            }
            isProcessingStartNotifierCollection = false;
        }

        /// <summary>
        /// Processes any collision change events on the given data and propagates it to any linked <see cref="CollisionNotifier"/>.
        /// </summary>
        /// <param name="data">The collision data.</param>
        protected virtual void OnCollisionChanged(EventData data)
        {
            if ((StatesToProcess & CollisionStates.Stay) == 0 || !CanEmit(data))
            {
                return;
            }

            CollisionChanged?.Invoke(data);

            if (isProcessingChangeNotifierCollection)
            {
                return;
            }

            isProcessingChangeNotifierCollection = true;
            foreach (CollisionNotifier notifier in GetNotifiers(data, changeCollisionNotifiers))
            {
                notifier.OnCollisionChanged(data);
            }
            isProcessingChangeNotifierCollection = false;
        }

        /// <summary>
        /// Processes any collision stop events on the given data and propagates it to any linked <see cref="CollisionNotifier"/>.
        /// </summary>
        /// <param name="data">The collision data.</param>
        protected virtual void OnCollisionStopped(EventData data)
        {
            if ((StatesToProcess & CollisionStates.Exit) == 0 || !CanEmit(data))
            {
                return;
            }

            CollisionStopped?.Invoke(data);

            if (isProcessingStopNotifierCollection)
            {
                return;
            }

            isProcessingStopNotifierCollection = true;
            foreach (CollisionNotifier notifier in GetNotifiers(data, stopCollisionNotifiers))
            {
                notifier.OnCollisionStopped(data);
            }
            isProcessingStopNotifierCollection = false;
        }
    }
}