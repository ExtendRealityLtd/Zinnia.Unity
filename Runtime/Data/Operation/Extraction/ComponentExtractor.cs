namespace Zinnia.Data.Operation.Extraction
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Attribute;

    /// <summary>
    /// Extracts and emits the <see cref="Component"/> found in relation to the <see cref="Source"/>.
    /// </summary>
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

        /// <summary>
        /// Determines whether to search for the <see cref="TResultElement"/> component on the ancestors and or descendants of the <see cref="Source"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml, UnityFlags]
        public SearchCriteria SearchAlsoOn { get; set; } = (SearchCriteria)(-1);

        /// <summary>
        /// Attempts to set the <see cref="Source"/> via a <see cref="GameObject"/>.
        /// </summary>
        /// <param name="source">The source of the <see cref="GameObject"/> component.</param>
        [RequiresBehaviourState]
        public virtual void SetSource(GameObject source)
        {
            Source = source != null ? source.transform : null;
        }

        /// <summary>
        /// Extracts the <see cref="TResultElement"/> from the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to extract from.</param>
        /// <returns>The extracted data.</returns>
        [RequiresBehaviourState]
        public virtual TResultElement Extract(GameObject data)
        {
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
