namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine.Events;
    using System.Collections.Generic;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Aggregates the values in a collection based on a specified operation.
    /// </summary>
    /// <typeparam name="TInput">The variable type that will be input into transformation.</typeparam>
    /// <typeparam name="TOutput">The variable type that will be output from the result of the transformation.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the transformation will emit.</typeparam>
    public abstract class CollectionAggregator<TInput, TOutput, TEvent> : Transformer<TInput, TOutput, TEvent>
        where TEvent : UnityEvent<TOutput>, new()
    {
        /// <summary>
        /// The collection to aggregate.
        /// </summary>
        [DocumentedByXml]
        public List<TInput> collection = new List<TInput>();

        /// <summary>
        /// The current collection index to add the given value to.
        /// </summary>
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = collection.GetWrappedAndClampedIndex(value); }
        }
        private int _currentIndex;

        /// <summary>
        /// Updates the collection at the <see cref="CurrentIndex"/> with the given <see cref="TInput"/>.
        /// </summary>
        /// <param name="input">The element to update the collection with.</param>
        public virtual void SetElement(TInput input)
        {
            collection[CurrentIndex] = input;
        }

        /// <summary>
        /// Updates the element at the given index without updating the collection <see cref="CurrentIndex"/>.
        /// </summary>
        /// <param name="index">The index in the collection to update at.</param>
        /// <param name="input">The element to update the collection with.</param>
        public virtual void SetElement(int index, TInput input)
        {
            collection[collection.GetWrappedAndClampedIndex(index)] = input;
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="index">The index of the collection to set the value at.</param>
        /// <param name="input">The value to add to the collection.</param>
        /// <returns>The summed value of all values in the collection.</returns>
        public virtual TOutput Transform(int index, TInput input)
        {
            CurrentIndex = index;
            return Transform(input);
        }

        /// <summary>
        /// Processes the given input by adding it to the collection at the given index and sums the collection total.
        /// </summary>
        /// <param name="index">The index of the collection to set the value at.</param>
        /// <param name="input">The value to add to the collection.</param>
        public virtual void DoTransform(int index, TInput input)
        {
            Transform(index, input);
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
            if (CurrentIndex < 0 || CurrentIndex >= collection.Count)
            {
                return default;
            }

            SetElement(input);
            return ProcessCollection();
        }
    }
}