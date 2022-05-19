namespace Zinnia.Data.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Attribute;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts and emits the <see cref="Component"/> found in relation to the <see cref="Source"/>.
    /// </summary>
    /// <typeparam name="TResultElement">The result type.</typeparam>
    /// <typeparam name="TEvent">The unity event to emit.</typeparam>
    /// <typeparam name="TEventElement">The event element type.</typeparam>
    public abstract class ComponentExtractor<TResultElement, TEvent, TEventElement> : ValueExtractor<TResultElement, Component, TEvent, TEventElement> where TEvent : UnityEvent<TEventElement>, new()
    {
        /// <summary>
        /// The criteria to search for the related <see cref="Component"/>.
        /// </summary>
        [Flags]
        public enum SearchCriteria
        {
            /// <summary>
            /// Search for the <see cref="TResultElement"/> component on the current <see cref="Source"/> and on any descendant.
            /// </summary>
            IncludeDescendants = 1 << 0,
            /// <summary>
            /// Search for the <see cref="TResultElement"/> component on the current <see cref="Source"/> and on any ancestor.
            /// </summary>
            IncludeAncestors = 1 << 1
        }

        [Tooltip("Determines whether to search for the TResultElement component on the ancestors and or descendants of the Source.")]
        [SerializeField]
        [UnityFlags]
        private SearchCriteria searchAlsoOn = (SearchCriteria)(-1);
        /// <summary>
        /// Determines whether to search for the <see cref="TResultElement"/> component on the ancestors and or descendants of the <see cref="Source"/>.
        /// </summary>
        public SearchCriteria SearchAlsoOn
        {
            get
            {
                return searchAlsoOn;
            }
            set
            {
                searchAlsoOn = value;
            }
        }

        /// <summary>
        /// Attempts to set the <see cref="Source"/> via a <see cref="GameObject"/>.
        /// </summary>
        /// <param name="source">The source of the <see cref="GameObject"/> component.</param>
        public virtual void SetSource(GameObject source)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = source != null ? source.transform : null;
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/> from the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        /// <returns>The extracted data.</returns>
        public virtual TResultElement Extract(GameObject data)
        {
            if (!this.IsValidState())
            {
                return default;
            }

            SetSource(data);
            return Extract();
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/> from the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        public virtual void DoExtract(GameObject data)
        {
            Extract(data);
        }
    }
}
