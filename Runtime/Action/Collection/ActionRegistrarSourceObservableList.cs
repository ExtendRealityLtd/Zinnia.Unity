namespace Zinnia.Action.Collection
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Allows observing changes to a <see cref="List{T}"/> of <see cref="ActionRegistrar.ActionSource"/>s.
    /// </summary>
    public class ActionRegistrarSourceObservableList : DefaultObservableList<ActionRegistrar.ActionSource, ActionRegistrarSourceObservableList.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the <see cref="ActionRegistrar.ActionSource"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<ActionRegistrar.ActionSource> { }

        /// <summary>
        /// Enables the <see cref="ActionRegistrar.ActionSource"/> that has a container matching the given source.
        /// </summary>
        /// <param name="source">The source to match the container against.</param>
        [RequiresBehaviourState]
        public virtual void EnableSource(GameObject source)
        {
            SetSourceEnabledState(source, true, false);
        }

        /// <summary>
        /// Disables the <see cref="ActionRegistrar.ActionSource"/> that has a container matching the given source.
        /// </summary>
        /// <param name="source">The source to match the container against.</param>
        [RequiresBehaviourState]
        public virtual void DisableSource(GameObject source)
        {
            SetSourceEnabledState(source, false, false);
        }

        /// <summary>
        /// Enables all <see cref="ActionRegistrar.ActionSource"/> elements.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void EnableAllSources()
        {
            SetSourceEnabledState(null, true, true);
        }

        /// <summary>
        /// Disables all <see cref="ActionRegistrar.ActionSource"/> elements.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void DisableAllSources()
        {
            SetSourceEnabledState(null, false, true);
        }

        /// <summary>
        /// Sets the enabled state of an <see cref="ActionRegistrar.ActionSource"/>.
        /// </summary>
        /// <param name="source">The source to match the container to set the state of.</param>
        /// <param name="state">The state to set enabled to.</param>
        /// <param name="setAll">Whether to ignore the source and just set all sources to the given state.</param>
        protected virtual void SetSourceEnabledState(GameObject source, bool state, bool setAll)
        {
            for (int index = 0; index < NonSubscribableElements.Count; index++)
            {
                ActionRegistrar.ActionSource actionSource = NonSubscribableElements[index];
                if (actionSource.Container != source && !setAll)
                {
                    continue;
                }

                actionSource.Enabled = state;

                RemoveAt(index);
                InsertAt(actionSource, index);
            }
        }
    }
}