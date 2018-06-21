namespace VRTK.Core.Tracking.Collision
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Holds a collection of collisions created by a <see cref="CollisionNotifier"/>.
    /// </summary>
    public class CollisionList : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="List{CollisionNotifier.EventData}"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<List<CollisionNotifier.EventData>>
        {
        }

        /// <summary>
        /// Emitted when the collision collection is no longer empty.
        /// </summary>
        public UnityEvent NotEmpty = new UnityEvent();
        /// <summary>
        /// Emitted when the collision collection count has changed.
        /// </summary>
        public UnityEvent CountChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the collision collection contents have changed.
        /// </summary>
        public UnityEvent ContentsChanged = new UnityEvent();
        /// <summary>
        /// Emitted when the collision collection has become empty.
        /// </summary>
        public UnityEvent Empty = new UnityEvent();

        /// <summary>
        /// The collisions currently stored in the collection.
        /// </summary>
        public List<CollisionNotifier.EventData> Elements
        {
            get;
            protected set;
        } = new List<CollisionNotifier.EventData>();

        /// <summary>
        /// Adds the given collision to the collection.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        public virtual void Add(CollisionNotifier.EventData collisionData)
        {
            bool wasEmpty = (Elements.Count == 0);
            Elements.Add(new CollisionNotifier.EventData().Set(collisionData));
            if (wasEmpty)
            {
                NotEmpty?.Invoke(Elements);
            }
            CountChanged?.Invoke(Elements);
            ProcessContentsChanged();
        }

        /// <summary>
        /// Removes the given collision from the collection.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        public virtual void Remove(CollisionNotifier.EventData collisionData)
        {
            Elements.Remove(collisionData);
            if ((Elements.Count == 0))
            {
                Empty?.Invoke(Elements);
            }
            CountChanged?.Invoke(Elements);
            ProcessContentsChanged();
        }

        /// <summary>
        /// Processes any changes to the contents of the collection.
        /// </summary>
        public virtual void ProcessContentsChanged()
        {
            ContentsChanged?.Invoke(Elements);
        }
    }
}