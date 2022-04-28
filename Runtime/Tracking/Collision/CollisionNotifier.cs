namespace Zinnia.Tracking.Collision
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
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
            [Tooltip("The source of this event in case it was forwarded.")]
            [SerializeField]
            private Component forwardSource;
            /// <summary>
            /// The source of this event in case it was forwarded.
            /// </summary>
            /// <remarks><see langword="null"/> if this event wasn't forwarded from anything.</remarks>
            public Component ForwardSource
            {
                get
                {
                    return forwardSource;
                }
                set
                {
                    forwardSource = value;
                }
            }
            [Tooltip("Whether the collision was observed through a Collider with Collider.isTrigger set.")]
            [SerializeField]
            private bool isTrigger;
            /// <summary>
            /// Whether the collision was observed through a <see cref="Collider"/> with <see cref="Collider.isTrigger"/> set.
            /// </summary>
            public bool IsTrigger
            {
                get
                {
                    return isTrigger;
                }
                set
                {
                    isTrigger = value;
                }
            }
            [Tooltip("The observed Collision. null if IsTrigger is true.")]
            [SerializeField]
            private Collision collisionData;
            /// <summary>
            /// The observed <see cref="Collision"/>. <see langword="null"/> if <see cref="IsTrigger"/> is <see langword="true"/>.
            /// </summary>
            public Collision CollisionData
            {
                get
                {
                    return collisionData;
                }
                set
                {
                    collisionData = value;
                }
            }
            [Tooltip("The observed Collider.")]
            [SerializeField]
            private Collider colliderData;
            /// <summary>
            /// The observed <see cref="Collider"/>.
            /// </summary>
            public Collider ColliderData
            {
                get
                {
                    return colliderData;
                }
                set
                {
                    colliderData = value;
                }
            }

            /// <summary>
            /// Clears <see cref="ForwardSource"/>.
            /// </summary>
            public virtual void ClearForwardSource()
            {
                ForwardSource = default;
            }

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
            public override string ToString()
            {
                string[] titles = new string[]
                {
                "ForwardSource",
                "IsTrigger",
                "CollisionData",
                "ColliderData"
                };

                object[] values = new object[]
                {
                ForwardSource,
                IsTrigger,
                CollisionData,
                ColliderData
                };

                return StringExtensions.FormatForToString(titles, values);
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

        #region Collision Settings
        [Header("Collision Settings")]
        [Tooltip("Whether the collisions should be processed when this component is disabled.")]
        [SerializeField]
        private bool processCollisionsWhenDisabled = true;
        /// <summary>
        /// Whether the collisions should be processed when this component is disabled.
        /// </summary>
        public bool ProcessCollisionsWhenDisabled
        {
            get
            {
                return processCollisionsWhenDisabled;
            }
            set
            {
                processCollisionsWhenDisabled = value;
            }
        }
        [Tooltip("The types of collisions that events will be emitted for.")]
        [SerializeField]
        [UnityFlags]
        private CollisionTypes emittedTypes = (CollisionTypes)(-1);
        /// <summary>
        /// The types of collisions that events will be emitted for.
        /// </summary>
        public CollisionTypes EmittedTypes
        {
            get
            {
                return emittedTypes;
            }
            set
            {
                emittedTypes = value;
            }
        }
        [Tooltip("The CollisionStates to process.")]
        [SerializeField]
        [UnityFlags]
        private CollisionStates statesToProcess = (CollisionStates)(-1);
        /// <summary>
        /// The <see cref="CollisionStates"/> to process.
        /// </summary>
        public CollisionStates StatesToProcess
        {
            get
            {
                return statesToProcess;
            }
            set
            {
                statesToProcess = value;
            }
        }
        [Tooltip("Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.")]
        [SerializeField]
        private RuleContainer forwardingSourceValidity;
        /// <summary>
        /// Allows to optionally determine which forwarded collisions to react to based on the set rules for the forwarding sender.
        /// </summary>
        public RuleContainer ForwardingSourceValidity
        {
            get
            {
                return forwardingSourceValidity;
            }
            set
            {
                forwardingSourceValidity = value;
            }
        }
        #endregion

        #region Collision Events
        /// <summary>
        /// Emitted when a collision starts.
        /// </summary>
        [Header("Collision Events")]
        public UnityEvent CollisionStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision changes.
        /// </summary>
        public UnityEvent CollisionChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the current collision stops.
        /// </summary>
        public UnityEvent CollisionStopped = new UnityEvent();
        #endregion

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
        /// The <see cref="Transform"/> of the notifier for the given collider data.
        /// </summary>
        protected Transform notifierColliderTransform;

        /// <summary>
        /// Clears <see cref="ForwardingSourceValidity"/>.
        /// </summary>
        public virtual void ClearForwardingSourceValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ForwardingSourceValidity = default;
        }

        /// <summary>
        /// Processes any collision start events on the given data and propagates it to any linked <see cref="CollisionNotifier"/>.
        /// </summary>
        /// <param name="data">The collision data.</param>
        public virtual void OnCollisionStarted(EventData data)
        {
            if (!CanProcess() || (StatesToProcess & CollisionStates.Enter) == 0 || !CanEmit(data))
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
        public virtual void OnCollisionChanged(EventData data)
        {
            if (!CanProcess() || (StatesToProcess & CollisionStates.Stay) == 0 || !CanEmit(data))
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
        public virtual void OnCollisionStopped(EventData data)
        {
            if (!CanProcess() || (StatesToProcess & CollisionStates.Exit) == 0 || !CanEmit(data))
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

        /// <summary>
        /// Whether to process the collision check.
        /// </summary>
        /// <returns><see langword="true"/> if the collision should be processed.</returns>
        protected virtual bool CanProcess()
        {
            return enabled || ProcessCollisionsWhenDisabled;
        }

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
            if (this == null || data == null || data.ColliderData == null)
            {
                return collisionNotifiers;
            }

            notifierColliderTransform = data.ColliderData.GetContainingTransform();

            if (transform.IsChildOf(notifierColliderTransform))
            {
                collisionNotifiers.Clear();
            }
            else
            {
                notifierColliderTransform.GetComponentsInChildren(collisionNotifiers);
            }

            return collisionNotifiers;
        }
    }
}