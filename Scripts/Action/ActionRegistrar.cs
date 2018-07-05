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
            /// The main container of the action.
            /// </summary>
            [Tooltip("The main container of the action.")]
            public GameObject container;
            /// <summary>
            /// The action to subscribe to.
            /// </summary>
            [Tooltip("The action to subscribe to.")]
            public BaseAction action;
        }

        /// <summary>
        /// Registers the action sources when the component is enabled.
        /// </summary>
        [Tooltip("Registers the action sources when the component is enabled.")]
        public bool registerOnEnable = true;
        /// <summary>
        /// The action that will have the sources populated by the given <see cref="sources"/>.
        /// </summary>
        [Tooltip("The action that will have the sources populated by the given `actionSources`.")]
        public BaseAction target;
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
        /// Registers the actions on the given active source if it is contained in the <see cref="sources"/> list.
        /// </summary>
        /// <param name="sourceLimit">A container of actions to limit the action subscription to.</param>
        public void Register(GameObject sourceLimit = null)
        {
            SourceLimit = sourceLimit;
            foreach (ActionSource actionSource in sources)
            {
                if (sourceLimit == null || sourceLimit == actionSource.container)
                {
                    target.AddSource(actionSource.action);
                }
            }
        }

        /// <summary>
        /// Unregisters the actions from the given active soure.
        /// </summary>
        /// <param name="sourceContainer">The source containing the actions to unregister the subscriptions from.</param>
        public void Unregister(GameObject sourceContainer)
        {
            foreach (ActionSource actionSource in sources)
            {
                if (sourceContainer == actionSource.container)
                {
                    target.RemoveSource(actionSource.action);
                    if (SourceLimit == sourceContainer)
                    {
                        SourceLimit = null;
                    }
                }
            }
        }

        /// <summary>
        /// Unsubscribes the target from all sources and clears the previous given active source.
        /// </summary>
        public void Clear()
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
    }
}