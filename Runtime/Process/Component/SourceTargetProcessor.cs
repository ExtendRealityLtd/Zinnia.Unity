namespace Zinnia.Process.Component
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Type;

    /// <summary>
    /// An <see cref="IProcessable"/> that runs a set method on each (or the first active) source collection against a collection of targets.
    /// </summary>
    public abstract class SourceTargetProcessor<TSource, TTarget, TEvent> : MonoBehaviour, IProcessable where TEvent : UnityEvent<TSource>, new()
    {
        /// <summary>
        /// Emitted if the <see cref="ActiveSource"/> value is going to change with the new value as the payload.
        /// </summary>
        public TEvent ActiveSourceChanging = new TEvent();

        [Header("Process Settings")]
        [Tooltip("Whether to cease the processing of the source collection after the first valid source is processed.")]
        [SerializeField]
        private bool ceaseAfterFirstSourceProcessed = true;
        /// <summary>
        /// Whether to cease the processing of the source collection after the first valid source is processed.
        /// </summary>
        public bool CeaseAfterFirstSourceProcessed
        {
            get
            {
                return ceaseAfterFirstSourceProcessed;
            }
            set
            {
                ceaseAfterFirstSourceProcessed = value;
            }
        }

        /// <summary>
        /// The <see cref="TSource"/> that is currently the active source for the process.
        /// </summary>
        public TSource ActiveSource
        {
            get;
            protected set;
        }

        /// <summary>
        /// Executes the relevant process to apply between the source and target.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Sets the current indices of the source and target collections.
        /// </summary>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="targetIndex">The target index.</param>
        protected abstract void SetCurrentIndices(int sourceIndex, int targetIndex);

        /// <summary>
        /// Applies the source data to the target data.
        /// </summary>
        /// <param name="source">The source to apply the data from.</param>
        /// <param name="target">The target to apply the data to.</param>
        protected abstract void ApplySourceToTarget(TSource source, TTarget target);

        /// <summary>
        /// Determines if the given source is valid to process.
        /// </summary>
        /// <param name="source">The source to check.</param>
        /// <returns><see langword="true"/> if the source is valid to process.</returns>
        protected virtual bool IsSourceValid(TSource source)
        {
            return !EqualityComparer<TSource>.Default.Equals(source, default);
        }

        /// <summary>
        /// Determines if the given target is valid to process.
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <returns><see langword="true"/> if the target is valid to process.</returns>
        protected virtual bool IsTargetValid(TTarget target)
        {
            return !EqualityComparer<TTarget>.Default.Equals(target, default);
        }

        /// <summary>
        /// Applies each (or the first active) source data to every (or only active) targets.
        /// </summary>
        /// <param name="sources">The sources to apply the data from.</param>
        /// <param name="targets">The targets to apply the data to.</param>
        protected virtual void ApplySourcesToTargets(HeapAllocationFreeReadOnlyList<TSource> sources, HeapAllocationFreeReadOnlyList<TTarget> targets)
        {
            bool foundValidSource = false;
            for (int sourceIndex = 0; sourceIndex < sources.Count; sourceIndex++)
            {
                TSource currentSource = sources[sourceIndex];
                if (!IsSourceValid(currentSource))
                {
                    continue;
                }

                for (int targetIndex = 0; targetIndex < targets.Count; targetIndex++)
                {
                    TTarget currentTarget = targets[targetIndex];
                    if (!IsTargetValid(currentTarget))
                    {
                        continue;
                    }

                    SetCurrentIndices(sourceIndex, targetIndex);
                    ApplySourceToTarget(currentSource, currentTarget);
                }

                if (!EqualityComparer<TSource>.Default.Equals(ActiveSource, currentSource))
                {
                    ActiveSourceChanging?.Invoke(currentSource);
                }
                ActiveSource = currentSource;
                foundValidSource = true;

                if (CeaseAfterFirstSourceProcessed)
                {
                    break;
                }
            }

            if (!foundValidSource && !EqualityComparer<TSource>.Default.Equals(ActiveSource, default))
            {
                ActiveSource = default;
                ActiveSourceChanging?.Invoke(default);
            }
        }
    }
}