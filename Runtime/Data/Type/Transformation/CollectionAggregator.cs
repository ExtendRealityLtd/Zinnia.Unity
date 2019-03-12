﻿namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine.Events;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Data.Collection;

    /// <summary>
    /// Aggregates the values in a collection based on a specified operation.
    /// </summary>
    /// <typeparam name="TInput">The variable type that will be input into transformation.</typeparam>
    /// <typeparam name="TOutput">The variable type that will be output from the result of the transformation.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the transformation will emit.</typeparam>
    public abstract class CollectionAggregator<TInput, TOutput, TEvent, TCollection, TCollectionEvent> : Transformer<TInput, TOutput, TEvent>
        where TEvent : UnityEvent<TOutput>, new() where TCollection : ObservableList<TInput, TCollectionEvent> where TCollectionEvent : UnityEvent<TInput>, new()
    {
        /// <summary>
        /// The collection to aggregate.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public TCollection Collection { get; set; }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <param name="index">The index of the collection to set the value at.</param>
        /// <returns>The summed value of all values in the collection.</returns>
        public virtual TOutput Transform(TInput input, int index)
        {
            Collection.CurrentIndex = index;
            return Transform(input);
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <param name="index">The index of the collection to set the value at.</param>
        public virtual void DoTransform(TInput input, int index)
        {
            Transform(input, index);
        }

        /// <summary>
        /// Process the collection and return the output;
        /// </summary>
        /// <returns>The processed collection result.</returns>
        protected abstract TOutput ProcessCollection();

        /// <summary>
        /// Processes the given input by adding it to the collection at the <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="input">The value to add to the collection.</param>
        /// <returns>The summed value of all values in the collection.</returns>
        protected override TOutput Process(TInput input)
        {
            if (Collection.NonSubscribableElements.Count == 0)
            {
                return default;
            }

            Collection.SetAtCurrentIndex(input);
            return ProcessCollection();
        }
    }
}