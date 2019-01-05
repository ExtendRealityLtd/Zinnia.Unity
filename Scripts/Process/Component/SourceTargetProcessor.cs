namespace Zinnia.Process.Component
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// An <see cref="IProcessable"/> that runs a set method on each (or the first active) source collection against a collection of targets.
    /// </summary>
    public abstract class SourceTargetProcessor<TSource, TTarget> : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Ceases the processing of the source collection after the first valid source is processed.
        /// </summary>
        [Header("Process Settings")]
        public bool ceaseAfterFirstSourceProcessed = true;

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
            return (source != null);
        }

        /// <summary>
        /// Determines if the given target is valid to process.
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <returns><see langword="true"/> if the target is valid to process.</returns>
        protected virtual bool IsTargetValid(TTarget target)
        {
            return (target != null);
        }

        /// <summary>
        /// Applies each (or the first active) source data to every (or only active) targets.
        /// </summary>
        /// <param name="sources">The sources to apply the data from.</param>
        /// <param name="targets">The targets to apply the data to.</param>
        protected virtual void ApplySourcesToTargets(List<TSource> sources, List<TTarget> targets)
        {
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

                ActiveSource = currentSource;
                if (ceaseAfterFirstSourceProcessed)
                {
                    break;
                }
            }
        }
    }
}