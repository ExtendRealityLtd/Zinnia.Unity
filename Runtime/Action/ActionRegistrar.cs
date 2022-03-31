namespace Zinnia.Action
{
    using Malimbe.MemberChangeMethod;
    using System;
    using UnityEngine;
    using Zinnia.Action.Collection;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Allows actions to dynamically register listeners to other actions.
    /// </summary>
    public class ActionRegistrar : MonoBehaviour
    {
        /// <summary>
        /// A source action to register a listener against.
        /// </summary>
        [Serializable]
        public struct ActionSource
        {
            /// <summary>
            /// Determines if the source can be used.
            /// </summary>
            [Tooltip("Determines if the source can be used.")]
            [SerializeField]
            private bool _enabled;
            public bool Enabled
            {
                get
                {
                    return _enabled;
                }
                set
                {
                    _enabled = value;
                }
            }
            /// <summary>
            /// The main container of the action.
            /// </summary>
            [Tooltip("The main container of the action.")]
            [SerializeField]
            private GameObject _container;
            public GameObject Container
            {
                get
                {
                    return _container;
                }
                set
                {
                    _container = value;
                }
            }
            /// <summary>
            /// The action to subscribe to.
            /// </summary>
            [Tooltip("The action to subscribe to.")]
            [SerializeField]
            private Action _action;
            public Action Action
            {
                get
                {
                    return _action;
                }
                set
                {
                    _action = value;
                }
            }
        }

        /// <summary>
        /// The action that will have the sources populated by the given <see cref="Sources"/>.
        /// </summary>
        [Tooltip("The action that will have the sources populated by the given Sources.")]
        [SerializeField]
        private Action _target;
        public Action Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }
        /// <summary>
        /// A list of <see cref="ActionSource"/>s to populate the target sources list with.
        /// </summary>
        [Tooltip("A list of ActionSources to populate the target sources list with.")]
        [SerializeField]
        private ActionRegistrarSourceObservableList _sources;
        public ActionRegistrarSourceObservableList Sources
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
            }
        }
        /// <summary>
        /// A list of <see cref="GameObject"/>s that are the limits of <see cref="Sources"/> by matching against <see cref="ActionSource.Container"/>.
        /// </summary>
        [Tooltip("A list of GameObjects that are the limits of Sources by matching against ActionSource.Container.")]
        [SerializeField]
        private GameObjectObservableList _sourceLimits;
        public GameObjectObservableList SourceLimits
        {
            get
            {
                return _sourceLimits;
            }
            set
            {
                _sourceLimits = value;
            }
        }

        protected virtual void OnEnable()
        {
            AddSourcesListeners();
            AddSourceLimitsListeners();
            TryAddTargetSources();
        }

        protected virtual void OnDisable()
        {
            RemoveSourcesListeners();
            RemoveSourcesLimitsListeners();
            ClearTargetSources();
        }

        /// <summary>
        /// Subscribes to events of <see cref="Sources"/>.
        /// </summary>
        protected virtual void AddSourcesListeners()
        {
            if (Sources == null)
            {
                return;
            }

            Sources.Added.AddListener(OnSourceAdded);
            Sources.Removed.AddListener(OnSourceRemoved);
        }

        /// <summary>
        /// Unsubscribes from events of <see cref="Sources"/>.
        /// </summary>
        protected virtual void RemoveSourcesListeners()
        {
            if (Sources == null)
            {
                return;
            }

            Sources.Added.RemoveListener(OnSourceAdded);
            Sources.Removed.RemoveListener(OnSourceRemoved);
        }

        /// <summary>
        /// Subscribes to events of <see cref="SourceLimits"/>.
        /// </summary>
        protected virtual void AddSourceLimitsListeners()
        {
            if (SourceLimits == null)
            {
                return;
            }

            SourceLimits.Added.AddListener(OnSourceLimitAdded);
            SourceLimits.Removed.AddListener(OnSourceLimitRemoved);
        }

        /// <summary>
        /// Unsubscribes from events of <see cref="SourceLimits"/>.
        /// </summary>
        protected virtual void RemoveSourcesLimitsListeners()
        {
            if (SourceLimits == null)
            {
                return;
            }

            SourceLimits.Added.RemoveListener(OnSourceLimitAdded);
            SourceLimits.Removed.RemoveListener(OnSourceLimitRemoved);
        }

        /// <summary>
        /// Adds all action sources from <see cref="Sources"/> if their <see cref="ActionSource.Container"/> matches any <see cref="SourceLimits"/>.
        /// </summary>
        protected virtual void TryAddTargetSources()
        {
            if (Sources == null || SourceLimits == null)
            {
                return;
            }

            // It is expected that we have less SourceLimits than we have Sources, so this order of nesting the loops is the preferred one.
            foreach (GameObject limit in SourceLimits.SubscribableElements)
            {
                foreach (ActionSource source in Sources.SubscribableElements)
                {
                    TryAddTargetSource(source, limit);
                }
            }
        }

        /// <summary>
        /// Clears all sources on <see cref="Target"/>.
        /// </summary>
        protected virtual void ClearTargetSources()
        {
            if (Target != null)
            {
                Target.ClearSources();
            }
        }

        /// <summary>
        /// Adds the given source if its <see cref="ActionSource.Container"/> matches the given limit.
        /// </summary>
        /// <param name="source">The source to try to add.</param>
        /// <param name="limit">The limit to try to match against any <see cref="ActionSource.Container"/>.</param>
        protected virtual void TryAddTargetSource(ActionSource source, GameObject limit)
        {
            if (source.Enabled && (limit == null || limit == source.Container))
            {
                Target.AddSource(source.Action);
            }
        }

        /// <summary>
        /// Removes the given source if its <see cref="ActionSource.Container"/> matches the given limit.
        /// </summary>
        /// <param name="source">The source to try to remove.</param>
        /// <param name="limit">The limit to try to match against any <see cref="ActionSource.Container"/>.</param>
        protected virtual void TryRemoveTargetSource(ActionSource source, GameObject limit)
        {
            if (limit == source.Container)
            {
                Target.RemoveSource(source.Action);
            }
        }

        /// <summary>
        /// Called after an element is added to <see cref="Sources"/>.
        /// </summary>
        /// <param name="source">The element added to the collection.</param>
        protected virtual void OnSourceAdded(ActionSource source)
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (GameObject limit in SourceLimits.SubscribableElements)
            {
                TryAddTargetSource(source, limit);
            }
        }

        /// <summary>
        /// Called after an element is removed from <see cref="Sources"/>.
        /// </summary>
        /// <param name="source">The element removed from the collection.</param>
        protected virtual void OnSourceRemoved(ActionSource source)
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (GameObject limit in SourceLimits.SubscribableElements)
            {
                TryRemoveTargetSource(source, limit);
            }
        }

        /// <summary>
        /// Called after an element is added to <see cref="SourceLimits"/>.
        /// </summary>
        /// <param name="limit">The element added to the collection.</param>
        protected virtual void OnSourceLimitAdded(GameObject limit)
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (ActionSource source in Sources.SubscribableElements)
            {
                TryAddTargetSource(source, limit);
            }
        }

        /// <summary>
        /// Called after an element is removed from <see cref="SourceLimits"/>.
        /// </summary>
        /// <param name="limit">The element removed from the collection.</param>
        protected virtual void OnSourceLimitRemoved(GameObject limit)
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (ActionSource source in Sources.SubscribableElements)
            {
                TryRemoveTargetSource(source, limit);
            }
        }

        /// <summary>
        /// Called before <see cref="Target"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(Target))]
        protected virtual void OnBeforeTargetChange()
        {
            ClearTargetSources();
        }

        /// <summary>
        /// Called after <see cref="Target"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Target))]
        protected virtual void OnAfterTargetChange()
        {
            if (Target != null)
            {
                TryAddTargetSources();
            }
        }

        /// <summary>
        /// Called before <see cref="Sources"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(Sources))]
        protected virtual void OnBeforeSourcesChange()
        {
            if (Sources == null)
            {
                return;
            }

            RemoveSourcesListeners();
            foreach (ActionSource source in Sources.SubscribableElements)
            {
                OnSourceRemoved(source);
            }
        }

        /// <summary>
        /// Called after <see cref="Sources"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Sources))]
        protected virtual void OnAfterSourcesChange()
        {
            AddSourcesListeners();
            TryAddTargetSources();
        }

        /// <summary>
        /// Called before <see cref="SourceLimits"/> has been changed.
        /// </summary>
        [CalledBeforeChangeOf(nameof(SourceLimits))]
        protected virtual void OnBeforeSourceLimitsChange()
        {
            if (SourceLimits == null)
            {
                return;
            }

            RemoveSourcesLimitsListeners();
            foreach (GameObject limit in SourceLimits.SubscribableElements)
            {
                OnSourceLimitRemoved(limit);
            }
        }

        /// <summary>
        /// Called after <see cref="SourceLimits"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(SourceLimits))]
        protected virtual void OnAfterSourceLimitsChange()
        {
            AddSourceLimitsListeners();
            TryAddTargetSources();
        }
    }
}