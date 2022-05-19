namespace Zinnia.Cast.Operation.Extraction
{
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;

    /// <summary>
    /// Extracts and emits the concrete implementation for data derrived from the <see cref="PointsCast.EventData"/>.
    /// </summary>
    /// <typeparam name="TResultElement">The result type.</typeparam>
    /// <typeparam name="TEvent">The unity event to emit.</typeparam>
    /// <typeparam name="TEventElement">The event element type.</typeparam>
    public abstract class PointsCastEventDataExtractor<TResultElement, TEvent, TEventElement> : ValueExtractor<TResultElement, PointsCast.EventData, TEvent, TEventElement> where TEvent : UnityEvent<TEventElement>, new()
    {
        /// <summary>
        /// The validity state the event data needs to be in to extract.
        /// </summary>
        public enum ExtractionState
        {
            /// <summary>
            /// Ignore the validity setting and always extract.
            /// </summary>
            AlwaysExtract,
            /// <summary>
            /// Only extract when the event data is not valid.
            /// </summary>
            OnlyWhenNotValid,
            /// <summary>
            /// Only extract when the event data is valid.
            /// </summary>
            OnlyWhenValid
        }

        [Tooltip("The validity state of the event data to determine whether to extract or not.")]
        [SerializeField]
        private ExtractionState extractWhen = ExtractionState.AlwaysExtract;
        /// <summary>
        /// The validity state of the event data to determine whether to extract or not.
        /// </summary>
        public ExtractionState ExtractWhen
        {
            get
            {
                return extractWhen;
            }
            set
            {
                extractWhen = value;
            }
        }

        /// <summary>
        /// Determines if the extraction can be executed.
        /// </summary>
        /// <returns>Whether to execute the extraction.</returns>
        protected virtual bool CanExtract()
        {
            switch (ExtractWhen)
            {
                case ExtractionState.AlwaysExtract:
                    return true;
                case ExtractionState.OnlyWhenNotValid:
                    return !Source.IsValid;
                case ExtractionState.OnlyWhenValid:
                    return Source.IsValid;
            }
            return false;
        }
    }
}