namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="List{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableList<TElement, TEvent> : MonoBehaviour where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// Emitted when the searched element is found.
        /// </summary>
        [Header("List Events"), DocumentedByXml]
        public TEvent Found = new TEvent();
        /// <summary>
        /// Emitted when the searched element is not found.
        /// </summary>
        [DocumentedByXml]
        public TEvent NotFound = new TEvent();
        /// <summary>
        /// Emitted when the first element is added to the collection.
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
        /// Emitted when the last element is removed from the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent BecameEmpty = new TEvent();

        /// <summary>
        /// The index to use in methods specifically specifying to use it. In case this index is out of bounds for the collection it will wrap around.
        /// </summary>
        [Serialized]
        [field: Header("List Settings"), DocumentedByXml]
        public int CurrentIndex { get; set; }

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        public IReadOnlyList<TElement> ReadOnlyElements => wasStartCalled
            ? (IReadOnlyList<TElement>)Elements
            : Array.Empty<TElement>();

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        protected abstract List<TElement> Elements { get; set; }

        /// <summary>
        /// Whether <see cref="Start"/> was called.
        /// </summary>
        protected bool wasStartCalled;

        /// <summary>
        /// Checks to see if the collection contains the given element.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        /// <returns>Whether the element is found.</returns>
        [RequiresBehaviourState]
        public virtual bool Contains(TElement element)
        {
            if (Elements.Contains(element))
            {
                Found?.Invoke(element);
                return true;
            }

            NotFound?.Invoke(element);
            return false;
        }

        /// <summary>
        /// Checks to see if the collection contains the given element.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        [RequiresBehaviourState]
        public virtual void DoContains(TElement element)
        {
            Contains(element);
        }

        /// <summary>
        /// Adds an element to the end of the collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        [RequiresBehaviourState]
        public virtual void Add(TElement element)
        {
            Elements.Add(element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Adds an element to the given index of the collection.
        /// </summary>
        /// <remarks>
        /// Allows the use of a wrapped and clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The element to add.</param>
        /// <param name="index">The index to add at. In case this index is out of bounds for the collection it will wrap around.</param>
        [RequiresBehaviourState]
        public virtual void AddAt(TElement element, int index)
        {
            if (ReadOnlyElements.Count == 0)
            {
                Add(element);
                return;
            }

            index = Elements.GetWrappedAndClampedIndex(index);
            Elements.Insert(index, element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Adds an element at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="element">The element to add.</param>
        [RequiresBehaviourState]
        public virtual void AddAtCurrentIndex(TElement element)
        {
            AddAt(element, CurrentIndex);
        }

        /// <summary>
        /// Sets the given element at the given index.
        /// </summary>
        /// <remarks>
        /// Allows the use of a wrapped and clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The element to set.</param>
        /// <param name="index">The index in the collection to set at. In case this index is out of bounds for the collection it will wrap around.</param>
        [RequiresBehaviourState]
        public virtual void SetAt(TElement element, int index)
        {
            if (ReadOnlyElements.Count == 0)
            {
                return;
            }

            index = Elements.GetWrappedAndClampedIndex(index);

            EmitRemoveEvents(Elements[index]);
            Elements[index] = element;
            EmitAddEvents(element);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="element">The element to set.</param>
        [RequiresBehaviourState]
        public virtual void SetAtCurrentIndex(TElement element)
        {
            SetAt(element, CurrentIndex);
        }

        /// <summary>
        /// Removes the first occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        [RequiresBehaviourState]
        public virtual void Remove(TElement element)
        {
            if (Elements.Remove(element))
            {
                EmitRemoveEvents(element);
            }
        }

        /// <summary>
        /// Removes the last occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        [RequiresBehaviourState]
        public virtual void RemoveLastOccurrence(TElement element)
        {
            int index = Elements.LastIndexOf(element);
            if (index == -1)
            {
                return;
            }

            RemoveAt(index);
        }

        /// <summary>
        /// Removes an element at the given index from the collection.
        /// </summary>
        /// <remarks>
        /// Allows the use of a wrapped and clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="index">The index to remove at. In case this index is out of bounds for the collection it will wrap around.</param>
        [RequiresBehaviourState]
        public virtual void RemoveAt(int index)
        {
            if (ReadOnlyElements.Count == 0)
            {
                return;
            }

            index = Elements.GetWrappedAndClampedIndex(index);
            TElement removedElement = ReadOnlyElements[index];
            Elements.RemoveAt(index);
            EmitRemoveEvents(removedElement);
        }

        /// <summary>
        /// Removes an element at the <see cref="CurrentIndex"/> from the collection.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void RemoveAtCurrentIndex()
        {
            RemoveAt(CurrentIndex);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        /// <param name="removeFromFront">Whether to start the removal from the start or the end of the collection.</param>
        [RequiresBehaviourState]
        public virtual void Clear(bool removeFromFront)
        {
            if (ReadOnlyElements.Count == 0)
            {
                return;
            }

            if (!removeFromFront)
            {
                Elements.Reverse();
            }

            foreach (TElement element in ReadOnlyElements)
            {
                ElementRemoved?.Invoke(element);
            }

            Elements.Clear();
            BecameEmpty?.Invoke(default);
        }

        protected virtual void Start()
        {
            wasStartCalled = true;

            if (ReadOnlyElements == null)
            {
                return;
            }

            for (int index = 0; index < ReadOnlyElements.Count; index++)
            {
                TElement element = ReadOnlyElements[index];
                if (EqualityComparer<TElement>.Default.Equals(element, default))
                {
                    continue;
                }

                ElementAdded?.Invoke(element);

                if (index == 0)
                {
                    BecamePopulated?.Invoke(element);
                }
            }
        }

        /// <summary>
        /// Always emits <see cref="ElementAdded"/> and additionally <see cref="BecamePopulated"/> if the first element was added to the collection.
        /// </summary>
        /// <param name="element">The element that was added.</param>
        protected virtual void EmitAddEvents(TElement element)
        {
            ElementAdded?.Invoke(element);

            if (ReadOnlyElements.Count == 1)
            {
                BecamePopulated?.Invoke(element);
            }
        }

        /// <summary>
        /// Always emits <see cref="ElementRemoved"/> and additionally <see cref="BecameEmpty"/> if the last element was removed from the collection.
        /// </summary>
        /// <param name="element">The element that was removed.</param>
        protected virtual void EmitRemoveEvents(TElement element)
        {
            ElementRemoved?.Invoke(element);

            if (ReadOnlyElements.Count == 0)
            {
                BecameEmpty?.Invoke(element);
            }
        }
    }
}