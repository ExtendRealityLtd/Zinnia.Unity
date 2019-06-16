namespace Zinnia.Data.Collection.List
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;
    using Zinnia.Data.Type;

    /// <summary>
    /// The basis for all Observable List types.
    /// </summary>
    public abstract class ObservableList : MonoBehaviour { }

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="List{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableList<TElement, TEvent> : ObservableList where TEvent : UnityEvent<TElement>, new()
    {
        #region List Contents Events
        /// <summary>
        /// Emitted when the searched element is found.
        /// </summary>
        [Header("List Contents Events"), DocumentedByXml]
        public TEvent Found = new TEvent();
        /// <summary>
        /// Emitted when the searched element is not found.
        /// </summary>
        [DocumentedByXml]
        public TEvent NotFound = new TEvent();
        /// <summary>
        /// Emitted when the collection contents is checked but is empty.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent IsEmpty = new UnityEvent();
        /// <summary>
        /// Emitted when the collection contents is checked and is populated.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent IsPopulated = new UnityEvent();
        #endregion

        #region List Mutation Events
        /// <summary>
        /// Emitted when the first element is added to the collection.
        /// </summary>
        [Header("List Mutation Events"), DocumentedByXml]
        public TEvent Populated = new TEvent();
        /// <summary>
        /// Emitted when an element is added to the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent Added = new TEvent();
        /// <summary>
        /// Emitted when an element is removed from the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent Removed = new TEvent();
        /// <summary>
        /// Emitted when the last element is removed from the collection.
        /// </summary>
        [DocumentedByXml]
        public TEvent Emptied = new TEvent();
        #endregion

        /// <summary>
        /// The index to use in methods specifically specifying to use it. In case this index is out of bounds for the collection it will be clamped within the index bounds.
        /// </summary>
        [Serialized]
        [field: Header("List Settings"), DocumentedByXml]
        public int CurrentIndex { get; set; }

        /// <summary>
        /// The elements to observe changes of, accessible from components that *are* keeping in sync with the state of the collection by subscribing to the list mutation events. Alternatively use <see cref="NonSubscribableElements"/> instead.
        /// </summary>
        public HeapAllocationFreeReadOnlyList<TElement> SubscribableElements => wasStartCalled ? (HeapAllocationFreeReadOnlyList<TElement>)Elements : Array.Empty<TElement>();
        /// <summary>
        /// The elements to observe changes of, accessible from components that are *not* interested in keeping in sync with the state of the collection. Alternatively use <see cref="SubscribableElements"/> instead.
        /// </summary>
        public HeapAllocationFreeReadOnlyList<TElement> NonSubscribableElements => Elements;

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
        /// Checks to see if the collection is currently empty.
        /// </summary>
        /// <returns>Whether the collection is empty.</returns>
        [RequiresBehaviourState]
        public virtual bool IsListEmpty()
        {
            if (Elements.Count == 0)
            {
                IsEmpty?.Invoke();
                return true;
            }
            else
            {
                IsPopulated?.Invoke();
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the collection is currently empty.
        /// </summary>
        public virtual void DoIsListEmpty()
        {
            IsListEmpty();
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
        /// Adds an element to the end of the collection as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to add.</param>
        [RequiresBehaviourState]
        public virtual void AddUnique(TElement element)
        {
            if (Elements.Contains(element))
            {
                return;
            }

            Add(element);
        }

        /// <summary>
        /// Inserts an element to the given index of the collection.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The element to insert.</param>
        /// <param name="index">The index to insert at. If the index is below the lower bounds it will be clamped at the lower bound of the index, if the index is above the upper bounds then a new element will be added to the end of the collection.</param>
        [RequiresBehaviourState]
        public virtual void InsertAt(TElement element, int index)
        {
            if (Elements.Count == 0 || index >= Elements.Count)
            {
                Add(element);
                return;
            }

            index = Elements.ClampIndex(index);
            Elements.Insert(index, element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Inserts an element to the given index of the collection as long as it does not already exist in the collection.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The unique element to insert.</param>
        /// <param name="index">The index to insert at. If the index is below the lower bounds it will be clamped at the lower bound of the index, if the index is above the upper bounds then a new element will be added to the end of the collection.</param>
        [RequiresBehaviourState]
        public virtual void InsertUniqueAt(TElement element, int index)
        {
            if (Elements.Contains(element))
            {
                return;
            }

            InsertAt(element, index);
        }

        /// <summary>
        /// Inserts an element at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="element">The element to insert.</param>
        [RequiresBehaviourState]
        public virtual void InsertAtCurrentIndex(TElement element)
        {
            InsertAt(element, CurrentIndex);
        }

        /// <summary>
        /// Adds an element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to add.</param>
        [RequiresBehaviourState]
        public virtual void AddUniqueAtCurrentIndex(TElement element)
        {
            if (Elements.Contains(element))
            {
                return;
            }

            InsertAtCurrentIndex(element);
        }

        /// <summary>
        /// Sets the given element at the given index.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The element to set.</param>
        /// <param name="index">The index in the collection to set at. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        [RequiresBehaviourState]
        public virtual void SetAt(TElement element, int index)
        {
            if (Elements.Count == 0)
            {
                return;
            }

            index = Elements.ClampIndex(index);
            RemoveAt(index);
            InsertAt(element, index);
        }

        /// <summary>
        /// Sets the given element at the given index as long as it does not already exist in the collection.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The unique element to set.</param>
        /// <param name="index">The index in the collection to set at. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        [RequiresBehaviourState]
        public virtual void SetUniqueAt(TElement element, int index)
        {
            if (Elements.Contains(element))
            {
                return;
            }

            SetAt(element, index);
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
        /// Sets the given element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to set.</param>
        [RequiresBehaviourState]
        public virtual void SetUniqueAtCurrentIndex(TElement element)
        {
            SetUniqueAt(element, CurrentIndex);
        }

        /// <summary>
        /// Sets the given element at the given index or adds the element if the collection is empty.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The element to set.</param>
        /// <param name="index">The index in the collection to set at. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        [RequiresBehaviourState]
        public virtual void SetAtOrAddIfEmpty(TElement element, int index)
        {
            if (Elements.Count == 0)
            {
                Add(element);
                return;
            }

            SetAt(element, index);
        }

        /// <summary>
        /// Sets the given element at the given index as long as it does not already exist in the collection or adds the element if the collection is empty.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="element">The unique element to set.</param>
        /// <param name="index">The index in the collection to set at. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        [RequiresBehaviourState]
        public virtual void SetUniqueAtOrAddIfEmpty(TElement element, int index)
        {
            if (Elements.Contains(element))
            {
                return;
            }

            SetAtOrAddIfEmpty(element, index);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/> or adds the element if the collection is empty.
        /// </summary>
        /// <param name="element">The element to set.</param>
        [RequiresBehaviourState]
        public virtual void SetAtCurrentIndexOrAddIfEmpty(TElement element)
        {
            SetAtOrAddIfEmpty(element, CurrentIndex);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection or adds the element if the collection is empty.
        /// </summary>
        /// <param name="element">The unique element to set.</param>
        [RequiresBehaviourState]
        public virtual void SetUniqueAtCurrentIndexOrAddIfEmpty(TElement element)
        {
            SetUniqueAtOrAddIfEmpty(element, CurrentIndex);
        }

        /// <summary>
        /// Removes the first occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        [RequiresBehaviourState]
        public virtual bool Remove(TElement element)
        {
            if (Elements.Remove(element))
            {
                EmitRemoveEvents(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the first occurrence of an element from the collection.
        /// </summary>
        public virtual void DoRemove(TElement element)
        {
            Remove(element);
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
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="index">The index to remove at. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        [RequiresBehaviourState]
        public virtual void RemoveAt(int index)
        {
            if (Elements.Count == 0)
            {
                return;
            }

            index = Elements.ClampIndex(index);
            TElement removedElement = Elements[index];
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
        /// <param name="removeFromEndToStart">Whether to reverse the collection when clearing so the removal goes from end to start.</param>
        [RequiresBehaviourState]
        public virtual void Clear(bool removeFromEndToStart = false)
        {
            if (Elements.Count == 0)
            {
                return;
            }

            if (removeFromEndToStart)
            {
                Elements.Reverse();
            }

            foreach (TElement element in Elements)
            {
                Removed?.Invoke(element);
            }

            Elements.Clear();
            Emptied?.Invoke(default);
        }

        protected virtual void Start()
        {
            wasStartCalled = true;

            if (Elements == null)
            {
                return;
            }

            for (int index = 0; index < Elements.Count; index++)
            {
                TElement element = Elements[index];
                if (EqualityComparer<TElement>.Default.Equals(element, default))
                {
                    continue;
                }

                Added?.Invoke(element);

                if (index == 0)
                {
                    Populated?.Invoke(element);
                }
            }
        }

        /// <summary>
        /// Always emits <see cref="Added"/> and additionally <see cref="Populated"/> if the first element was added to the collection.
        /// </summary>
        /// <param name="element">The element that was added.</param>
        protected virtual void EmitAddEvents(TElement element)
        {
            Added?.Invoke(element);

            if (Elements.Count == 1)
            {
                Populated?.Invoke(element);
            }
        }

        /// <summary>
        /// Always emits <see cref="Removed"/> and additionally <see cref="Emptied"/> if the last element was removed from the collection.
        /// </summary>
        /// <param name="element">The element that was removed.</param>
        protected virtual void EmitRemoveEvents(TElement element)
        {
            Removed?.Invoke(element);

            if (Elements.Count == 0)
            {
                Emptied?.Invoke(element);
            }
        }
    }
}