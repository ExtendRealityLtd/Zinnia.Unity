namespace Zinnia.Process.Component
{
    using UnityEngine;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    /*using Malimbe.PropertySetterMethod;*/
    using Zinnia.Extension;
    using Zinnia.Rule;

    public abstract class GameObjectSourceTargetProcessor : SourceTargetProcessor<GameObject, GameObject>
    {
        #region Processor Settings
        /// <summary>
        /// A <see cref="GameObject"/> collection of sources to apply data from.
        /// </summary>
        [Header("Entity Settings")]
        public List<GameObject> sources = new List<GameObject>();
        /// <summary>
        /// Allows to optionally determine which sources should be processed based on the set rules.
        /// </summary>
        public RuleContainer sourceValidity;
        /// <summary>
        /// A <see cref="GameObject"/> collection of targets to apply data to.
        /// </summary>
        public List<GameObject> targets = new List<GameObject>();
        /// <summary>
        /// Allows to optionally determine which targets should be processed based on the set rules.
        /// </summary>
        public RuleContainer targetValidity;
        #endregion

        /// <summary>
        /// The current <see cref="sources"/> collection index.
        /// </summary>
        public int CurrentSourcesIndex { get; set; }

        /// <summary>
        /// The current <see cref="targets"/> collection index.
        /// </summary>
        public int CurrentTargetsIndex { get; set; }

        /// <inheritdoc />
        [RequiresBehaviourState]
        public override void Process()
        {
            ApplySourcesToTargets(sources, targets);
        }

        /// <summary>
        /// Adds the given source to the sources collection.
        /// </summary>
        /// <param name="source">The source to add.</param>
        public virtual void AddSource(GameObject source)
        {
            sources.Add(source);
        }

        /// <summary>
        /// Removes the given source from the sources collection.
        /// </summary>
        /// <param name="source">The source to remove.</param>
        public virtual void RemoveSource(GameObject source)
        {
            sources.Remove(source);
        }

        /// <summary>
        /// Sets the given source at the current sources index.
        /// </summary>
        /// <param name="source">The source to set.</param>
        public virtual void SetSourceAtCurrentIndex(GameObject source)
        {
            if (sources.Count == 0)
            {
                sources.Add(source);
            }
            else
            {
                sources[CurrentSourcesIndex] = source;
            }
        }

        /// <summary>
        /// Clears the sources collection.
        /// </summary>
        public virtual void ClearSources()
        {
            sources.Clear();
        }

        /// <summary>
        /// Adds the given target to the targets collection.
        /// </summary>
        /// <param name="target">The target to add.</param>
        public virtual void AddTarget(GameObject target)
        {
            targets.Add(target);
        }

        /// <summary>
        /// Removes the given target from the targets collection.
        /// </summary>
        /// <param name="target">The target to remove.</param>
        public virtual void RemoveTarget(GameObject target)
        {
            targets.Remove(target);
        }

        /// <summary>
        /// Sets the given target at the current targets index.
        /// </summary>
        /// <param name="target">The target to set.</param>
        public virtual void SetTargetAtCurrentIndex(GameObject target)
        {
            if (targets.Count == 0)
            {
                targets.Add(target);
            }
            else
            {
                targets[CurrentTargetsIndex] = target;
            }
        }

        /// <summary>
        /// Clears the targets collection.
        /// </summary>
        public virtual void ClearTargets()
        {
            targets.Clear();
        }

        /// <inheritdoc />
        protected override void SetCurrentIndices(int sourceIndex, int targetIndex)
        {
            CurrentSourcesIndex = sourceIndex;
            CurrentTargetsIndex = targetIndex;
        }

        /// <inheritdoc />
        protected override bool IsSourceValid(GameObject source)
        {
            return (base.IsSourceValid(source) && sourceValidity.Accepts(source));
        }

        /// <inheritdoc />
        protected override bool IsTargetValid(GameObject target)
        {
            return (base.IsTargetValid(target) && targetValidity.Accepts(target));
        }

        /// <summary>
        /// Handles changes to <see cref="CurrentSourcesIndex"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        /*[CalledBySetter(nameof(CurrentSourcesIndex))]*/
        protected virtual void OnCurrentSourcesIndexChange(int previousValue, ref int newValue)
        {
            newValue = sources.GetWrappedAndClampedIndex(newValue);
        }

        /// <summary>
        /// Handles changes to <see cref="CurrentTargetsIndex"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        /*[CalledBySetter(nameof(CurrentTargetsIndex))]*/
        protected virtual void OnCurrentTargetsIndexChange(int previousValue, ref int newValue)
        {
            newValue = targets.GetWrappedAndClampedIndex(newValue);
        }
    }
}