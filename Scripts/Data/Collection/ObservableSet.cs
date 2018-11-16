namespace VRTK.Core.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

    /// <summary>
    /// Allows observing changes to a <see cref="HashSet{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="HashSet{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableSet<TElement, TEvent> : MonoBehaviour where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// Emitted when a searched element is found.
        /// </summary>
        public TEvent ElementFound = new TEvent();
        /// <summary>
        /// Emitted when a searched element is not found.
        /// </summary>
        public TEvent ElementNotFound = new TEvent();
        /// <summary>
        /// Emitted when the collection has the last element removed from it and becomes empty.
        /// </summary>
        public TEvent BecameEmpty = new TEvent();
        /// <summary>
        /// Emitted when the collection has the first element added to it and becomes populated.
        /// </summary>
        public TEvent BecamePopulated = new TEvent();

        /// <summary>
        /// The elements currently in the set.
        /// </summary>
        public HashSet<TElement> Elements
        {
            get;
            protected set;
        } = new HashSet<TElement>();


        /// <summary>
        /// Adds the given element to the set and if it is the first element added <see cref="BecamePopulated"/> is emitted with the added element.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public virtual void AddElement(TElement element)
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
        public virtual void RemoveElement(TElement element)
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
            BecameEmpty?.Invoke(default(TElement));
        }

        /// <summary>
        /// Determines if the given element is contained within the set and emits the appropriate event based on the result.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        /// <returns><see langword="true"/> if the element is found.</returns>
        public virtual bool Contains(TElement element)
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
        public virtual void DoContains(TElement element)
        {
            Contains(element);
        }
    }
}