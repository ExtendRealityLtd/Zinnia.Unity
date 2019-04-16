namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Rule;
    using Zinnia.Extension;

    /// <summary>
    /// Holds a collection of the current collisions raised by a <see cref="CollisionNotifier"/>.
    /// </summary>
    public class ActiveCollisionsContainer : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="CollisionTracker"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The current active collisions.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public List<CollisionNotifier.EventData> ActiveCollisions { get; set; } = new List<CollisionNotifier.EventData>();

            public EventData Set(EventData source)
            {
                return Set(source.ActiveCollisions);
            }

            public EventData Set(List<CollisionNotifier.EventData> activeCollisions)
            {
                ActiveCollisions.Clear();
                ActiveCollisions.AddRange(activeCollisions);
                return this;
            }

            public void Clear()
            {
                ActiveCollisions.Clear();
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class ActiveCollisionUnityEvent : UnityEvent<EventData>
        {
        }

        #region Validity Settings
        /// <summary>
        /// Determines whether the collision is valid and to add it to the active collision collection.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Validity Settings"), DocumentedByXml]
        public RuleContainer CollisionValidity { get; set; }
        #endregion

        #region Collection Events
        /// <summary>
        /// Emitted when the collision count has changed.
        /// </summary>
        [Header("Collection Events"), DocumentedByXml]
        public ActiveCollisionUnityEvent CountChanged = new ActiveCollisionUnityEvent();
        /// <summary>
        /// Emitted when the collision contents have changed.
        /// </summary>
        [DocumentedByXml]
        public ActiveCollisionUnityEvent ContentsChanged = new ActiveCollisionUnityEvent();
        #endregion

        #region Collision Events
        /// <summary>
        /// Emitted when the first collision occurs.
        /// </summary>
        [Header("Collision Events"), DocumentedByXml]
        public CollisionNotifier.UnityEvent FirstStarted = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when a collision is added.
        /// </summary>
        [DocumentedByXml]
        public CollisionNotifier.UnityEvent Added = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when a collision is removed.
        /// </summary>
        [DocumentedByXml]
        public CollisionNotifier.UnityEvent Removed = new CollisionNotifier.UnityEvent();
        /// <summary>
        /// Emitted when there are no more collisions occuring.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent AllStopped = new UnityEvent();
        #endregion

        /// <summary>
        /// The current active collisions.
        /// </summary>
        public List<CollisionNotifier.EventData> Elements { get; protected set; } = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// The event data emitted on active collision events.
        /// </summary>
        protected readonly EventData eventData = new EventData();

        /// <summary>
        /// Adds the given collision as an active collision.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        [RequiresBehaviourState]
        public virtual void Add(CollisionNotifier.EventData collisionData)
        {
            if (!IsValidCollision(collisionData))
            {
                return;
            }

            Elements.Add(CloneEventData(collisionData));
            Added?.Invoke(collisionData);
            if (Elements.Count == 1)
            {
                FirstStarted?.Invoke(collisionData);
            }
            CountChanged?.Invoke(eventData.Set(Elements));
            ProcessContentsChanged();
        }

        /// <summary>
        /// Removes the given collision from being an active collision.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        [RequiresBehaviourState]
        public virtual void Remove(CollisionNotifier.EventData collisionData)
        {
            if (Elements.Remove(collisionData))
            {
                Removed?.Invoke(collisionData);
                EmitEmptyEvents();
            }
        }

        /// <summary>
        /// Processes any changes to the contents of existing collisions.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ProcessContentsChanged()
        {
            ContentsChanged?.Invoke(eventData.Set(Elements));
        }

        protected virtual void OnDisable()
        {
            if (Elements.Count == 0)
            {
                return;
            }

            foreach (CollisionNotifier.EventData element in Elements)
            {
                Removed?.Invoke(element);
            }

            Elements.Clear();
            EmitEmptyEvents();
        }

        /// <summary>
        /// Emits the appropriate events when the collection is emptied.
        /// </summary>
        protected virtual void EmitEmptyEvents()
        {
            if (Elements.Count == 0)
            {
                AllStopped?.Invoke();
            }

            CountChanged?.Invoke(eventData.Set(Elements));
            ProcessContentsChanged();
        }

        /// <summary>
        /// Clones the given event data.
        /// </summary>
        /// <param name="input">The data to clone.</param>
        /// <returns>The cloned data.</returns>
        protected virtual CollisionNotifier.EventData CloneEventData(CollisionNotifier.EventData input)
        {
            return new CollisionNotifier.EventData().Set(input);
        }

        /// <summary>
        /// Determines if the current collision is valid.
        /// </summary>
        /// <param name="collisionData">The collision to check.</param>
        /// <returns>The validity result of the collision.</returns>
        protected virtual bool IsValidCollision(CollisionNotifier.EventData collisionData)
        {
            Transform containingTransform = collisionData.ColliderData.GetContainingTransform();
            return containingTransform != null && CollisionValidity.Accepts(containingTransform.gameObject);
        }

    }
}