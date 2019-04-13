namespace Zinnia.Data.Collection.Counter
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
        public TEvent Added = new TEvent();
        /// <summary>
        /// Emitted when an element is removed completely.
        /// </summary>
        [DocumentedByXml]
        public TEvent Removed = new TEvent();

        /// <summary>
        /// The elements being counted.
        /// </summary>
        public Dictionary<TElement, int> ElementsCounter { get; protected set; } = new Dictionary<TElement, int>();

        /// <summary>
        /// Increases the count of the given element.
        /// </summary>
        /// <param name="element">The element to count.</param>
        [RequiresBehaviourState]
        public virtual void IncreaseCount(TElement element)
        {
            if (EqualityComparer<TElement>.Default.Equals(element, default))
            {
                return;
            }

            if (ElementsCounter.ContainsKey(element))
            {
                ElementsCounter[element]++;
            }
            else
            {
                ElementsCounter.Add(element, 1);
                Added?.Invoke(element);
            }
        }

        /// <summary>
        /// Decreases the count of the given element.
        /// </summary>
        /// <param name="element">The element to count.</param>
        [RequiresBehaviourState]
        public virtual void DecreaseCount(TElement element)
        {
            if (EqualityComparer<TElement>.Default.Equals(element, default))
            {
                return;
            }

            if (!ElementsCounter.TryGetValue(element, out int counter))
            {
                return;
            }

            counter--;
            ElementsCounter[element] = counter;

            if (counter > 0)
            {
                return;
            }

            ElementsCounter.Remove(element);
            Removed?.Invoke(element);
        }

        /// <summary>
        /// Removes the element from the counter.
        /// </summary>
        /// <param name="element">The element to clear.</param>
        [RequiresBehaviourState]
        public virtual void RemoveFromCount(TElement element)
        {
            if (EqualityComparer<TElement>.Default.Equals(element, default) || !ElementsCounter.Remove(element))
            {
                return;
            }

            Removed?.Invoke(element);
        }

        /// <summary>
        /// Clears all elements from the counter.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Clear()
        {
            foreach (TElement element in ElementsCounter.Keys)
            {
                if (!EqualityComparer<TElement>.Default.Equals(element, default))
                {
                    Removed?.Invoke(element);
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
            ElementsCounter.TryGetValue(element, out int countValue);
            return countValue;
        }
    }
}