namespace VRTK.Core.Action
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

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
        /// The action that will have the Sources populated by the given actionSources.
        /// </summary>
        [Tooltip("The action that will have the Sources populated by the given actionSources.")]
        public BaseAction target;
        /// <summary>
        /// A list of action sources to populate the target sources list with.
        /// </summary>
        [Tooltip("A list of action sources to populate the target sources list with.")]
        public List<ActionSource> actionSources = new List<ActionSource>();

        /// <summary>
        /// The current <see cref="GameObject"/> that is being used to retrieve the list of action sources from.
        /// </summary>
        public GameObject ActiveSource
        {
            get;
            protected set;
        }

        /// <summary>
        /// Registers the actions on the given valid source <see cref="GameObject"/> if it is contained in the <see cref="actionSources"/> list.
        /// </summary>
        /// <param name="validSource"></param>
        public void Register(GameObject validSource = null)
        {
            ActiveSource = validSource;
            target.UnsubscribeFromSources();
            target.ClearSources();
            foreach (ActionSource actionSource in actionSources)
            {
                if (validSource == null || validSource == actionSource.container)
                {
                    target.AddSource(actionSource.action);
                }
            }
            target.SubscribeToSources();
        }

        /// <summary>
        /// Unsubscribes the target from all sources and clears the previous given sources.
        /// </summary>
        public void Clear()
        {
            target.UnsubscribeFromSources();
            target.ClearSources();
            ActiveSource = null;
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