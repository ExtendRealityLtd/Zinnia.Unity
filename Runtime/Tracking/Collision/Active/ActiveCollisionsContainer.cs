namespace Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
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
            [DocumentedByXml]
            public List<CollisionNotifier.EventData> activeCollisions = new List<CollisionNotifier.EventData>();

            public EventData Set(EventData source)
            {
                return Set(source.activeCollisions);
            }

            public EventData Set(List<CollisionNotifier.EventData> activeCollisions)
            {
                this.activeCollisions.Clear();
                this.activeCollisions.AddRange(activeCollisions);
                return this;
            }

            public void Clear()
            {
                activeCollisions.Clear();
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
        /// Determines whether the collision is valid and to add it to the active collision collection.
        /// </summary>
        [DocumentedByXml]
        public RuleContainer collisionValidity;

        /// <summary>
        /// Emitted when the first collision occurs.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent FirstStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the collision count has changed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent CountChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the collision contents have changed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent ContentsChanged = new UnityEvent();
        /// <summary>
        /// Emitted when there are no more collisions occuring.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent AllStopped = new UnityEvent();

        /// <summary>
        /// The current active collisions.
        /// </summary>
        public List<CollisionNotifier.EventData> Elements
        {
            get;
            protected set;
        } = new List<CollisionNotifier.EventData>();

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

            bool wasEmpty = (Elements.Count == 0);
            Elements.Add(CloneEventData(collisionData));
            if (wasEmpty)
            {
                FirstStarted?.Invoke(eventData.Set(Elements));
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
            Elements.Clear();
            EmitEmptyEvents();
        }

        /// <summary>
        /// Emits the appropriate events when the collection is emptied.
        /// </summary>
        protected virtual void EmitEmptyEvents()
        {
            if ((Elements.Count == 0))
            {
                AllStopped?.Invoke(eventData.Set(Elements));
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
            return (collisionData.collider != null && collisionValidity.Accepts(collisionData.collider.gameObject));
        }

    }
}