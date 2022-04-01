namespace Zinnia.Process.Component
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// A <see cref="SourceTargetProcessor{TSource, TTarget}"/> that specifically processes a <see cref="GameObject"/>.
    /// </summary>
    public abstract class GameObjectSourceTargetProcessor : SourceTargetProcessor<GameObject, GameObject, GameObjectSourceTargetProcessor.GameObjectUnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class GameObjectUnityEvent : UnityEvent<GameObject> { }

        #region Processor Settings
        [Header("Entity Settings")]
        [Tooltip("A GameObject collection of sources to apply data from.")]
        [SerializeField]
        private GameObjectObservableList sources;
        /// <summary>
        /// A <see cref="GameObject"/> collection of sources to apply data from.
        /// </summary>
        public GameObjectObservableList Sources
        {
            get
            {
                return sources;
            }
            set
            {
                sources = value;
            }
        }
        [Tooltip("Allows to optionally determine which sources should be processed based on the set rules.")]
        [SerializeField]
        private RuleContainer sourceValidity;
        /// <summary>
        /// Allows to optionally determine which sources should be processed based on the set rules.
        /// </summary>
        public RuleContainer SourceValidity
        {
            get
            {
                return sourceValidity;
            }
            set
            {
                sourceValidity = value;
            }
        }
        [Tooltip("A GameObject collection of targets to apply data to.")]
        [SerializeField]
        private GameObjectObservableList targets;
        /// <summary>
        /// A <see cref="GameObject"/> collection of targets to apply data to.
        /// </summary>
        public GameObjectObservableList Targets
        {
            get
            {
                return targets;
            }
            set
            {
                targets = value;
            }
        }
        [Tooltip("Allows to optionally determine which targets should be processed based on the set rules.")]
        [SerializeField]
        private RuleContainer targetValidity;
        /// <summary>
        /// Allows to optionally determine which targets should be processed based on the set rules.
        /// </summary>
        public RuleContainer TargetValidity
        {
            get
            {
                return targetValidity;
            }
            set
            {
                targetValidity = value;
            }
        }
        #endregion

        /// <summary>
        /// Clears <see cref="SourceValidity"/>.
        /// </summary>
        public virtual void ClearSourceValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            SourceValidity = default;
        }

        /// <summary>
        /// Clears <see cref="TargetValidity"/>.
        /// </summary>
        public virtual void ClearTargetValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            TargetValidity = default;
        }

        /// <inheritdoc />
        public override void Process()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ApplySourcesToTargets(Sources.NonSubscribableElements, Targets.NonSubscribableElements);
        }

        /// <inheritdoc />
        protected override void SetCurrentIndices(int sourceIndex, int targetIndex)
        {
            Sources.CurrentIndex = sourceIndex;
            Targets.CurrentIndex = targetIndex;
        }

        /// <inheritdoc />
        protected override bool IsSourceValid(GameObject source)
        {
            return base.IsSourceValid(source) && SourceValidity.Accepts(source);
        }

        /// <inheritdoc />
        protected override bool IsTargetValid(GameObject target)
        {
            return base.IsTargetValid(target) && TargetValidity.Accepts(target);
        }
    }
}