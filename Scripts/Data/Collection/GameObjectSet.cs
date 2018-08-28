namespace VRTK.Core.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Collects GameObjects into an unordered set and emits relevant events when the set data changes or is searched.
    /// </summary>
    public class GameObjectSet : MonoBehaviour
    {
        /// <summary>
        /// Defines the event for the output <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject>
        {
        }

        /// <summary>
        /// Emitted when a searched element is found.
        /// </summary>
        public UnityEvent ElementFound = new UnityEvent();
        /// <summary>
        /// Emitted when a searched element is not found.
        /// </summary>
        public UnityEvent ElementNotFound = new UnityEvent();
        /// <summary>
        /// Emitted when the collection has the last element removed from it and becomes empty.
        /// </summary>
        public UnityEvent BecameEmpty = new UnityEvent();
        /// <summary>
        /// Emitted when the collection has the first element added to it and becomes populated.
        /// </summary>
        public UnityEvent BecamePopulated = new UnityEvent();

        /// <summary>
        /// The elements currently in the set.
        /// </summary>
        public HashSet<GameObject> Elements
        {
            get;
            protected set;
        } = new HashSet<GameObject>();


        /// <summary>
        /// Adds the given element to the set and if it is the first element added <see cref="BecamePopulated"/> is emitted with the added element.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public virtual void AddElement(GameObject element)
        {
            if (!isActiveAndEnabled || element == null)
            {
                return;
            }

            if (Elements.Count == 0)
            {
                BecamePopulated?.Invoke(element);
            }
            Elements.Add(element);
        }

        /// <summary>
        /// Removes the given element from the set and if it the last element removed <see cref="BecameEmpty"/> is emitted with the removed element.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public virtual void RemoveElement(GameObject element)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (Elements.Remove(element) && Elements.Count == 0)
            {
                BecameEmpty?.Invoke(element);
            }
        }

        /// <summary>
        /// Clears all elements from the set and emits <see cref="BecameEmpty"/> with a null element.
        /// </summary>
        public virtual void ClearElements()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Elements.Clear();
            BecameEmpty?.Invoke(null);
        }

        /// <summary>
        /// Determines if the given element is contained within the set and emits the appropriate event based on the result.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        /// <returns><see langword="true"/> if the element is found.</returns>
        public virtual bool Contains(GameObject element)
        {
            if (!isActiveAndEnabled)
            {
                return false;
            }

            bool result = Elements.Contains(element);
            if (result)
            {
                ElementFound?.Invoke(element);
            }
            else
            {
                ElementNotFound?.Invoke(element);
            }
            return result;
        }

        /// <summary>
        /// Determines if the given element is contained within the set and emits the appropriate event based on the result.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        public virtual void DoContains(GameObject element)
        {
            Contains(element);
        }
    }
}