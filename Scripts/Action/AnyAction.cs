namespace Zinnia.Action
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when any given actions are in their active state.
    /// </summary>
    public class AnyAction : BooleanAction
    {
        /// <summary>
        /// Actions to check the active state on.
        /// </summary>
        public List<Action> actions = new List<Action>();

        protected virtual void Update()
        {
            bool areAnyActionsActivated = actions.EmptyIfNull().Any(action => action.IsActivated);
            if (areAnyActionsActivated != IsActivated)
            {
                Receive(areAnyActionsActivated);
            }
        }
    }
}