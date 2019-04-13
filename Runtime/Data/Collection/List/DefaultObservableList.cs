namespace Zinnia.Data.Collection.List
{
    using UnityEngine.Events;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// An intermediate that defines the <see cref="Elements"/> collection to prevent needing to redefine in every concrete class.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the <see cref="List{T}"/>.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class DefaultObservableList<TElement, TEvent> : ObservableList<TElement, TEvent> where TEvent : UnityEvent<TElement>, new()
    {
        /// <summary>
        /// The collection to observe changes of.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        protected override List<TElement> Elements { get; set; } = new List<TElement>();
    }
}
