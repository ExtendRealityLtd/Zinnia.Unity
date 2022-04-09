namespace Zinnia.Data.Collection.List
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

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
        /// Emitted when the element at a given index is obtained.
        /// </summary>
        [Header("List Contents Events")]
        public TEvent Obtained = new TEvent();
        /// <summary>
        /// Emitted when the searched element is found.
        /// </summary>
        public TEvent Found = new TEvent();
        /// <summary>
        /// Emitted when the searched element is not found.
        /// </summary>
        public TEvent NotFound = new TEvent();
        /// <summary>
        /// Emitted when the collection contents is checked but is empty.
        /// </summary>
        public UnityEvent IsEmpty = new UnityEvent();
        /// <summary>
        /// Emitted when the collection contents is checked and is populated.
        /// </summary>
        public UnityEvent IsPopulated = new UnityEvent();
        #endregion

        #region List Mutation Events
        /// <summary>
        /// Emitted when the first element is added to the collection.
        /// </summary>
        [Header("List Mutation Events")]
        public TEvent Populated = new TEvent();
        /// <summary>
        /// Emitted when an element is added to the collection.
        /// </summary>
        public TEvent Added = new TEvent();
        /// <summary>
        /// Emitted when an element is removed from the collection.
        /// </summary>
        public TEvent Removed = new TEvent();
        /// <summary>
        /// Emitted when the last element is removed from the collection.
        /// </summary>
        public TEvent Emptied = new TEvent();
        #endregion

        [Header("List Settings")]
        [Tooltip("The index to use in methods specifically specifying to use it. In case this index is out of bounds for the collection it will be clamped within the index bounds.")]
        [SerializeField]
        private int currentIndex;
        /// <summary>
        /// The index to use in methods specifically specifying to use it. In case this index is out of bounds for the collection it will be clamped within the index bounds.
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
            set
            {
                currentIndex = value;
            }
        }

        /// <summary>
        /// The elements to observe changes of, accessible from components that *are* keeping in sync with the state of the collection by subscribing to the list mutation events. Alternatively use <see cref="NonSubscribableElements"/> instead.
        /// </summary>
        public virtual HeapAllocationFreeReadOnlyList<TElement> SubscribableElements => wasStartCalled ? (HeapAllocationFreeReadOnlyList<TElement>)Elements : Array.Empty<TElement>();
        /// <summary>
        /// The elements to observe changes of, accessible from components that are *not* interested in keeping in sync with the state of the collection. Alternatively use <see cref="SubscribableElements"/> instead.
        /// </summary>
        public virtual HeapAllocationFreeReadOnlyList<TElement> NonSubscribableElements => Elements;

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        protected abstract List<TElement> Elements { get; set; }
        /// <summary>
        /// Whether <see cref="Start"/> was called.
        /// </summary>
        protected bool wasStartCalled;

        /// <summary>
        /// Gets the element at the given index.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="index">The index in the collection to retrieve from. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        /// <returns>The element at the given index.</returns>
        public virtual TElement Get(int index)
        {
            index = Elements.ClampIndex(index);

            if (this.IsValidState())
            {
                Obtained?.Invoke(Elements[index]);
            }

            return Elements[index];
        }

        /// <summary>
        /// Gets the element at the given index.
        /// </summary>
        /// <remarks>
        /// Allows the use of a clamped index to prevent indices being out of bounds and doing negative queries such as `-1` sets the last element.
        /// </remarks>
        /// <param name="index">The index in the collection to retrieve from. In case this index is out of bounds for the collection it will be clamped within the index bounds.</param>
        public virtual void DoGet(int index)
        {
            Get(index);
        }

        /// <summary>
        /// Checks to see if the collection contains the given element.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        /// <returns>Whether the element is found.</returns>
        public virtual bool Contains(TElement element)
        {
            if (Elements.Contains(element))
            {
                if (this.IsValidState())
                {
                    Found?.Invoke(element);
                }
                return true;
            }

            if (this.IsValidState())
            {
                NotFound?.Invoke(element);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the collection contains the given element.
        /// </summary>
        /// <param name="element">The element to search for.</param>
        public virtual void DoContains(TElement element)
        {
            Contains(element);
        }

        /// <summary>
        /// Checks to see if the collection is currently empty.
        /// </summary>
        /// <returns>Whether the collection is empty.</returns>
        public virtual bool IsListEmpty()
        {
            if (Elements.Count == 0)
            {
                if (this.IsValidState())
                {
                    IsEmpty?.Invoke();
                }
                return true;
            }
            else
            {
                if (this.IsValidState())
                {
                    IsPopulated?.Invoke();
                }
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
        /// Gets the index of the given element.
        /// </summary>
        /// <param name="element">The element to check for.</param>
        /// <returns>The index of the found element.</returns>
        public virtual int IndexOf(TElement element)
        {
            return Elements.IndexOf(element);
        }

        /// <summary>
        /// Adds an element to the end of the collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public virtual void Add(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Elements.Add(element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Adds an element to the end of the collection as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to add.</param>
        public virtual void AddUnique(TElement element)
        {
            if (!this.IsValidState() || Elements.Contains(element))
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
        public virtual void InsertAt(TElement element, int index)
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void InsertUniqueAt(TElement element, int index)
        {
            if (!this.IsValidState() || Elements.Contains(element))
            {
                return;
            }

            InsertAt(element, index);
        }

        /// <summary>
        /// Inserts an element at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="element">The element to insert.</param>
        public virtual void InsertAtCurrentIndex(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            InsertAt(element, CurrentIndex);
        }

        /// <summary>
        /// Adds an element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to add.</param>
        public virtual void AddUniqueAtCurrentIndex(TElement element)
        {
            if (!this.IsValidState() || Elements.Contains(element))
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
        public virtual void SetAt(TElement element, int index)
        {
            if (!this.IsValidState() || Elements.Count == 0)
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
        public virtual void SetUniqueAt(TElement element, int index)
        {
            if (!this.IsValidState() || Elements.Contains(element))
            {
                return;
            }

            SetAt(element, index);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="element">The element to set.</param>
        public virtual void SetAtCurrentIndex(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            SetAt(element, CurrentIndex);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection.
        /// </summary>
        /// <param name="element">The unique element to set.</param>
        public virtual void SetUniqueAtCurrentIndex(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void SetAtOrAddIfEmpty(TElement element, int index)
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void SetUniqueAtOrAddIfEmpty(TElement element, int index)
        {
            if (!this.IsValidState() || Elements.Contains(element))
            {
                return;
            }

            SetAtOrAddIfEmpty(element, index);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/> or adds the element if the collection is empty.
        /// </summary>
        /// <param name="element">The element to set.</param>
        public virtual void SetAtCurrentIndexOrAddIfEmpty(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            SetAtOrAddIfEmpty(element, CurrentIndex);
        }

        /// <summary>
        /// Sets the given element at the <see cref="CurrentIndex"/> as long as it does not already exist in the collection or adds the element if the collection is empty.
        /// </summary>
        /// <param name="element">The unique element to set.</param>
        public virtual void SetUniqueAtCurrentIndexOrAddIfEmpty(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

            SetUniqueAtOrAddIfEmpty(element, CurrentIndex);
        }

        /// <summary>
        /// Removes the first occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public virtual bool Remove(TElement element)
        {
            if (!this.IsValidState())
            {
                return default;
            }

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
        public virtual void RemoveLastOccurrence(TElement element)
        {
            if (!this.IsValidState())
            {
                return;
            }

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
        public virtual void RemoveAt(int index)
        {
            if (!this.IsValidState() || Elements.Count == 0)
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
        public virtual void RemoveAtCurrentIndex()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RemoveAt(CurrentIndex);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        /// <param name="removeFromEndToStart">Whether to reverse the collection when clearing so the removal goes from end to start.</param>
        public virtual void Clear(bool removeFromEndToStart = false)
        {
            if (!this.IsValidState() || Elements.Count == 0)
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
            if (!wasStartCalled)
            {
                return;
            }

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