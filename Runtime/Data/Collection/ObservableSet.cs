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
    public abstract class ObservableSet<TElement, TEvent> : MonoBehaviour, ISerializationCallbackReceiver where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        [SerializeField, DocumentedByXml]
        private List<TElement> _elements = new List<TElement>();

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
        /// Emitted when the collection has the first element added to it and becomes populated.
        /// </summary>
        [DocumentedByXml]
        public TEvent BecamePopulated = new TEvent();
        /// <summary>
        /// Emitted when an element is added to the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent ElementAdded = new TEvent();
        /// <summary>
        /// Emitted when an element is removed from the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent ElementRemoved = new TEvent();
        /// <summary>
        /// Emitted when the collection has the last element removed from it and becomes empty.
        /// </summary>
        [DocumentedByXml]
        public TEvent BecameEmpty = new TEvent();

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        public IReadOnlyCollection<TElement> Elements => elements;

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        protected readonly HashSet<TElement> elements = new HashSet<TElement>();

        /// <summary>
        /// Adds the given element to the set.
        /// </summary>
        /// <param name="element">The element to add.</param>
        [RequiresBehaviourState]
        public virtual void AddElement(TElement element)
        {
            if (Equals(element, default(TElement)))
            {
                return;
            }

            if (!elements.Add(element))
            {
                return;
            }

            ElementAdded?.Invoke(element);
            if (elements.Count == 1)
            {
                BecamePopulated?.Invoke(element);
            }
        }

        /// <summary>
        /// Removes the given element from the set.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        [RequiresBehaviourState]
        public virtual void RemoveElement(TElement element)
        {
            if (!elements.Remove(element))
            {
                return;
            }

            ElementRemoved?.Invoke(element);
            if (elements.Count == 0)
            {
                BecameEmpty?.Invoke(element);
            }
        }

        /// <summary>
        /// Clears all elements from the set.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ClearElements()
        {
            if (elements.Count == 0)
            {
                return;
            }

            foreach (TElement element in elements)
            {
                ElementRemoved?.Invoke(element);
            }

            elements.Clear();
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
            bool result = elements.Contains(element);
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

        /// <inheritdoc />
        public virtual void OnBeforeSerialize()
        {
            _elements.Clear();
            _elements.AddRange(elements);
        }

        /// <inheritdoc />
        public virtual void OnAfterDeserialize()
        {
            elements.Clear();
            foreach (TElement element in _elements)
            {
                elements.Add(element);
            }
        }

        protected virtual void Start()
        {
            bool becamePopulatedAlready = false;

            foreach (TElement element in elements)
            {
                ElementAdded?.Invoke(element);
                if (becamePopulatedAlready)
                {
                    continue;
                }

                BecamePopulated?.Invoke(element);
                becamePopulatedAlready = true;
            }
        }
    }
}