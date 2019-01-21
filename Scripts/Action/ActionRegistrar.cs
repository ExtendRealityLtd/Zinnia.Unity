namespace VRTK.Core.Action
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

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
            public bool enabled;
            /// <summary>
            /// The main container of the action.
            /// </summary>
            [Tooltip("The main container of the action.")]
            public GameObject container;
            /// <summary>
            /// The action to subscribe to.
            /// </summary>
            [Tooltip("The action to subscribe to.")]
            public Action action;
        }

        /// <summary>
        /// Registers the action sources when the component is enabled.
        /// </summary>
        [Tooltip("Registers the action sources when the component is enabled.")]
        public bool registerOnEnable = true;
        /// <summary>
        /// The action that will have the sources populated by the given <see cref="sources"/>.
        /// </summary>
        [Tooltip("The action that will have the sources populated by the given sources.")]
        public Action target;
        /// <summary>
        /// A list of action sources to populate the target sources list with.
        /// </summary>
        [Tooltip("A list of action sources to populate the target sources list with.")]
        public List<ActionSource> sources = new List<ActionSource>();

        /// <summary>
        /// The current <see cref="GameObject"/> that is the limit of the action list.
        /// </summary>
        public GameObject SourceLimit
        {
            get;
            protected set;
        }

        /// <summary>
        /// Enables the <see cref="ActionSource"/> that has a container matching the source <see cref="GameObject"/>.
        /// </summary>
        /// <param name="source">The source to match the container to enable.</param>
        public virtual void EnableSource(GameObject source)
        {
            SetSourceEnabledState(source, true);
        }

        /// <summary>
        /// Disables the <see cref="ActionSource"/> that has a container matching the source <see cref="GameObject"/>.
        /// </summary>
        /// <param name="source">The source to match the container to disable.</param>
        public virtual void DisableSource(GameObject source)
        {
            SetSourceEnabledState(source, false);
        }

        /// <summary>
        /// Enables all <see cref="ActionSource"/> elements.
        /// </summary>
        public virtual void EnableAllSources()
        {
            SetSourceEnabledState(null, true, true);
        }

        /// <summary>
        /// Disables all <see cref="ActionSource"/> elements.
        /// </summary>
        public virtual void DisableAllSources()
        {
            SetSourceEnabledState(null, false, true);
        }

        /// <summary>
        /// <param name="sourceLimit">A container of actions to limit the action subscription to or <see langword="null"/> to not limit.</param>
        /// </summary>
        /// <param name="sourceLimit">A container of actions to limit the action subscription to.</param>
        public virtual void Register(GameObject sourceLimit = null)
        {
            SourceLimit = sourceLimit;
            foreach (ActionSource actionSource in sources)
            {
                if (actionSource.enabled && (sourceLimit == null || sourceLimit == actionSource.container))
                {
                    target.AddSource(actionSource.action);
                }
            }
        }

        /// <summary>
        /// Unregisters the actions from the given active source.
        /// </summary>
        /// <param name="sourceContainer">The source containing the actions to unregister the subscriptions from.</param>
        public virtual void Unregister(GameObject sourceContainer)
        {
            foreach (ActionSource actionSource in sources)
            {
                if (sourceContainer != actionSource.container)
                {
                    continue;
                }

                target.RemoveSource(actionSource.action);
                if (SourceLimit == sourceContainer)
                {
                    SourceLimit = null;
                }
            }
        }

        /// <summary>
        /// Unsubscribes the target from all sources and clears the previous given active source.
        /// </summary>
        public virtual void Clear()
        {
            target.ClearSources();
            SourceLimit = null;
        }

        protected virtual void OnEnable()
        {
            if (registerOnEnable)
            {
                Register();
            }
        }

        /// <summary>
        /// Sets the enabled state of an <see cref="ActionSource"/>.
        /// </summary>
        /// <param name="source">The source to match the container to set the state of.</param>
        /// <param name="state">The state to set enabled to.</param>
        /// <param name="setAll">Determines whether to ignore the source and just set all sources to the given state.</param>
        protected virtual void SetSourceEnabledState(GameObject source, bool state, bool setAll = false)
        {
            for (int index = 0; index < sources.Count; index++)
            {
                ActionSource actionSource = sources[index];
                if (actionSource.container == source || setAll)
                {
                    actionSource.enabled = state;
                }
                sources[index] = actionSource;
            }
        }
    }
}