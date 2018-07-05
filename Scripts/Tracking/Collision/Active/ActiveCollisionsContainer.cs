namespace VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

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
        /// Defines the event with the <see cref="List{CollisionNotifier.EventData}"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// Emitted when the first collision occurs.
        /// </summary>
        public UnityEvent FirstStarted = new UnityEvent();
        /// <summary>
        /// Emitted when the collision count has changed.
        /// </summary>
        public UnityEvent CountChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the collision contents have changed.
        /// </summary>
        public UnityEvent ContentsChanged = new UnityEvent();
        /// <summary>
        /// Emitted when there are no more collisions occuring.
        /// </summary>
        public UnityEvent AllStopped = new UnityEvent();

        // <summary>
        /// The current active collisions.
        /// </summary>
        public List<CollisionNotifier.EventData> Elements
        {
            get;
            protected set;
        } = new List<CollisionNotifier.EventData>();

        protected EventData eventData = new EventData();

        /// <summary>
        /// Adds the given collision as an active collision.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        public virtual void Add(CollisionNotifier.EventData collisionData)
        {
            if (!isActiveAndEnabled)
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
        public virtual void Remove(CollisionNotifier.EventData collisionData)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (Elements.Remove(collisionData))
            {
                if ((Elements.Count == 0))
                {
                    AllStopped?.Invoke(eventData.Set(Elements));
                }
                CountChanged?.Invoke(eventData.Set(Elements));
                ProcessContentsChanged();
            }
        }

        /// <summary>
        /// Processes any changes to the contents of existing collisions.
        /// </summary>
        public virtual void ProcessContentsChanged()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            ContentsChanged?.Invoke(eventData.Set(Elements));
        }

        protected virtual CollisionNotifier.EventData CloneEventData(CollisionNotifier.EventData input)
        {
            return new CollisionNotifier.EventData().Set(input);
        }
    }
}