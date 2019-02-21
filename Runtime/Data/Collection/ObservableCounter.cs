namespace Zinnia.Data.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// Allows counting the amount of attempts an element is added or removed from a <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements to count.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public class ObservableCounter<TElement, TEvent> : MonoBehaviour where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// Emitted when an element is added for the first time.
        /// </summary>
        [DocumentedByXml]
        public TEvent ElementAdded = new TEvent();
        /// <summary>
        /// Emitted when an element is removed completely.
        /// </summary>
        [DocumentedByXml]
        public TEvent ElementRemoved = new TEvent();

        /// <summary>
        /// The elements being counted.
        /// </summary>
        public Dictionary<TElement, int> ElementsCounter
        {
            get;
            protected set;
        } = new Dictionary<TElement, int>();

        /// <summary>
        /// Increases the count of the given element.
        /// </summary>
        /// <param name="element">The element to count.</param>
        [RequiresBehaviourState]
        public virtual void IncreaseCount(TElement element)
        {
            if (element == null)
            {
                return;
            }

            if (!ElementsCounter.ContainsKey(element))
            {
                ElementsCounter.Add(element, 0);
            }

            ElementsCounter[element]++;
            if (ElementsCounter[element] == 1)
            {
                ElementAdded?.Invoke(element);
            }
        }

        /// <summary>
        /// Decreases the count of the given element.
        /// </summary>
        /// <param name="element">The element to count.</param>
        [RequiresBehaviourState]
        public virtual void DecreaseCount(TElement element)
        {
            int currentValue = 0;
            if (element == null || !ElementsCounter.TryGetValue(element, out currentValue) || currentValue <= 0)
            {
                return;
            }

            ElementsCounter[element]--;
            if (ElementsCounter[element] <= 0)
            {
                ElementsCounter.Remove(element);
                ElementRemoved?.Invoke(element);
            }
        }

        /// <summary>
        /// Removes the element from the counter.
        /// </summary>
        /// <param name="element">The element to clear.</param>
        [RequiresBehaviourState]
        public virtual void RemoveFromCount(TElement element)
        {
            if (element == null || !ElementsCounter.Remove(element))
            {
                return;
            }

            ElementRemoved?.Invoke(element);
        }

        /// <summary>
        /// Clears all elements from the counter.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Clear()
        {
            foreach (TElement element in ElementsCounter.Keys)
            {
                if (!Equals(element, default(TElement)))
                {
                    ElementRemoved?.Invoke(element);
                }
            }

            ElementsCounter.Clear();
        }

        /// <summary>
        /// How often an element was added to the counter without being removed.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <returns>The count of the given element.</returns>
        public virtual int GetCount(TElement element)
        {
            int countValue = 0;
            ElementsCounter.TryGetValue(element, out countValue);
            return countValue;
        }
    }
}