namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;

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
        [DocumentedByXml]
        public TEvent ElementFound = new TEvent();
        /// <summary>
        /// Emitted when a searched element is not found.
        /// </summary>
        [DocumentedByXml]
        public TEvent ElementNotFound = new TEvent();
        /// <summary>
        /// Emitted when the collection has the last element removed from it and becomes empty.
        /// </summary>
        [DocumentedByXml]
        public TEvent BecameEmpty = new TEvent();
        /// <summary>
        /// Emitted when the collection has the first element added to it and becomes populated.
        /// </summary>
        [DocumentedByXml]
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
        [RequiresBehaviourState]
        public virtual void AddElement(TElement element)
        {
            if (element == null)
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
        [RequiresBehaviourState]
        public virtual void RemoveElement(TElement element)
        {
            if (Elements.Remove(element) && Elements.Count == 0)
            {
                BecameEmpty?.Invoke(element);
            }
        }

        /// <summary>
        /// Clears all elements from the set and emits <see cref="BecameEmpty"/> with a null element.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ClearElements()
        {
            Elements.Clear();
            BecameEmpty?.Invoke(default);
        }

        /// <summary>
        /// Determines if the given element is contained within the set and emits the appropriate event based on the result.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        /// <returns><see langword="true"/> if the element is found.</returns>
        [RequiresBehaviourState]
        public virtual bool Contains(TElement element)
        {
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