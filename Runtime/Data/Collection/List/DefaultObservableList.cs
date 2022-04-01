namespace Zinnia.Data.Collection.List
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// An intermediate that defines the <see cref="Elements"/> collection to prevent needing to redefine in every concrete class.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="List{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class DefaultObservableList<TElement, TEvent> : ObservableList<TElement, TEvent> where TEvent : UnityEvent<TElement>, new()
    {
        [Tooltip("The collection to observe changes of.")]
        [SerializeField]
        private List<TElement> elements = new List<TElement>();
        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        protected override List<TElement> Elements
        {
            get
            {
                return elements;
            }
            set
            {
                elements = value;
            }
        }
    }
}
