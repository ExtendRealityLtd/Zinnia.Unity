namespace VRTK.Core.Data.Collection
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="List{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableListBase<TElement, TEvent> : MonoBehaviour where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        [Tooltip("The list to observe changes of."), SerializeField]
        protected List<TElement> elements = new List<TElement>();

        /// <summary>
        /// Emitted when the first element is added to the collection.
        /// </summary>
        public TEvent BecamePopulated = new TEvent();
        /// <summary>
        /// Emitted when an element is added to the collection.
        /// </summary>
        public TEvent ElementAdded = new TEvent();
        /// <summary>
        /// Emitted when an element is removed from the collection.
        /// </summary>
        public TEvent ElementRemoved = new TEvent();
        /// <summary>
        /// Emitted when the last element is removed from the collection.
        /// </summary>
        public TEvent BecameEmpty = new TEvent();

        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        public IReadOnlyList<TElement> Elements => elements;

        /// <summary>
        /// Adds an element to the start of the collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public virtual void AddToStart(TElement element)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            elements.Insert(0, element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Adds an element to the end of the collection.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public virtual void AddToEnd(TElement element)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            elements.Add(element);
            EmitAddEvents(element);
        }

        /// <summary>
        /// Removes the first occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public virtual void RemoveFirst(TElement element)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            elements.Remove(element);
            EmitRemoveEvents(element);
        }

        /// <summary>
        /// Removes the last occurrence of an element from the collection.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public virtual void RemoveLast(TElement element)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            int index = elements.LastIndexOf(element);
            if (index != -1)
            {
                elements.RemoveAt(index);
            }

            EmitRemoveEvents(element);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        /// <param name="removeFromFront">Whether to start the removal from the start or the end of the collection.</param>
        public virtual void Clear(bool removeFromFront)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (!removeFromFront)
            {
                elements.Reverse();
            }

            foreach (TElement element in elements)
            {
                ElementRemoved?.Invoke(element);
            }

            elements.Clear();
            BecameEmpty?.Invoke(default(TElement));
        }

        protected virtual void Start()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (i == 0)
                {
                    BecamePopulated?.Invoke(Elements[i]);
                }
                ElementAdded?.Invoke(Elements[i]);
            }
        }

        /// <summary>
        /// Always emits <see cref="ElementAdded"/> and additionally <see cref="BecamePopulated"/> if the first element was added to the collection.
        /// </summary>
        /// <param name="element">The element that was added.</param>
        protected virtual void EmitAddEvents(TElement element)
        {
            ElementAdded?.Invoke(element);

            if (elements.Count == 1)
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

            if (elements.Count == 0)
            {
                BecameEmpty?.Invoke(element);
            }
        }
    }
}