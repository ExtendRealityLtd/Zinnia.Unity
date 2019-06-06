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
        /// A containing <see cref="Transform"/> collection with a nested collection of each collision occuring.
        /// </summary>
        protected Dictionary<Transform, List<CollisionNotifier.EventData>> containingTransformCollisions = new Dictionary<Transform, List<CollisionNotifier.EventData>>();
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
            Transform currentCollisionContainingTransform = collisionData.ColliderData.GetContainingTransform();
            if (!IsValidCollision(currentCollisionContainingTransform))
            {
                return;
            }

            if (!AreExistingCollisions(collisionData, currentCollisionContainingTransform))
            {
                CollisionNotifier.EventData clonedCollisionData = CloneEventData(collisionData);
                AddUniqueElement(clonedCollisionData);

                if (!containingTransformCollisions.TryGetValue(currentCollisionContainingTransform, out List<CollisionNotifier.EventData> existingTransformCollisions))
                {
                    existingTransformCollisions = new List<CollisionNotifier.EventData>();
                    containingTransformCollisions[currentCollisionContainingTransform] = existingTransformCollisions;
                }
                existingTransformCollisions.Add(clonedCollisionData);

                Added?.Invoke(collisionData);
                if (Elements.Count == 1)
                {
                    FirstStarted?.Invoke(collisionData);
                }
                CountChanged?.Invoke(eventData.Set(Elements));
                ProcessContentsChanged();
            }
        }

        /// <summary>
        /// Removes the given collision from being an active collision.
        /// </summary>
        /// <param name="collisionData">The collision data.</param>
        [RequiresBehaviourState]
        public virtual void Remove(CollisionNotifier.EventData collisionData)
        {
            Transform currentCollisionContainingTransform = collisionData.ColliderData.GetContainingTransform();
            if (!containingTransformCollisions.TryGetValue(currentCollisionContainingTransform, out List<CollisionNotifier.EventData> foundCollisionElements))
            {
                return;
            }

            foundCollisionElements.Remove(collisionData);
            Elements.Remove(collisionData);

            if (!HasRemainingCollisions(foundCollisionElements))
            {
                containingTransformCollisions.Remove(currentCollisionContainingTransform);
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
            containingTransformCollisions.Clear();
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
        /// Determines if the current containing <see cref="Transform"/> in the collision is valid.
        /// </summary>
        /// <param name="containingTransform">The <see cref="Transform"/> to check.</param>
        /// <returns>The validity result of the collision.</returns>
        protected virtual bool IsValidCollision(Transform containingTransform)
        {
            return containingTransform != null && CollisionValidity.Accepts(containingTransform.gameObject);
        }

        /// <summary>
        /// Adds the given data to the <see cref="Elements"/> collection if it is not already present.
        /// </summary>
        /// <param name="data">The data to add.</param>
        protected virtual void AddUniqueElement(CollisionNotifier.EventData data)
        {
            if (!Elements.Contains(data))
            {
                Elements.Add(data);
            }
        }

        /// <summary>
        /// Updates current containing <see cref="Transform"/> collision occurring by removing any existing nested collisions and adding the current collision in the given collision data.
        /// </summary>
        /// <param name="collisionData">The collision data to add.</param>
        /// <param name="existingCollisionData">The existing collision data to remove from <see cref="Elements"/>.</param>
        protected virtual void UpdateContainingTransformCollisions(CollisionNotifier.EventData collisionData, List<CollisionNotifier.EventData> existingCollisionData)
        {
            foreach (CollisionNotifier.EventData data in existingCollisionData)
            {
                Elements.Remove(data);
            }

            CollisionNotifier.EventData clonedCollisionData = CloneEventData(collisionData);
            existingCollisionData.Add(clonedCollisionData);
            AddUniqueElement(clonedCollisionData);
        }

        /// <summary>
        /// Determines if the containing <see cref="Transform"/> is already being collided with by a different nested <see cref="Collider"/>.
        /// </summary>
        /// <remarks>
        /// Updates the <see cref="Elements"/> collection with the latest collision data for an existing containing <see cref="Transform"/> that is being collided with.
        /// </remarks>
        /// <param name="collisionData">The collision data containing the <see cref="Collider"/> to check.</param>
        /// <param name="currentCollisionContainingTransform">The containing <see cref="Transform"/> for the current collider.</param>
        /// <returns>Whether there are any existing collisions occuring with the containing <see cref="Transform"/>.</returns>
        protected virtual bool AreExistingCollisions(CollisionNotifier.EventData collisionData, Transform currentCollisionContainingTransform)
        {
            if (!containingTransformCollisions.TryGetValue(currentCollisionContainingTransform, out List<CollisionNotifier.EventData> foundCollisionElements))
            {
                return false;
            }

            if (foundCollisionElements.Contains(collisionData))
            {
                return true;
            }

            UpdateContainingTransformCollisions(collisionData, foundCollisionElements);
            return true;
        }

        /// <summary>
        /// Determines if the collisions collection still contains any valid collisions and updates the <see cref="Elements"/> collection with the next available collision data.
        /// </summary>
        /// <param name="foundCollisionElements">The collection to check.</param>
        /// <returns>Whether the collection has remaining collisions.</returns>
        protected virtual bool HasRemainingCollisions(List<CollisionNotifier.EventData> foundCollisionElements)
        {
            if (foundCollisionElements.Count > 0)
            {
                AddUniqueElement(foundCollisionElements[foundCollisionElements.Count - 1]);
                return true;
            }
            return false;
        }
    }
}