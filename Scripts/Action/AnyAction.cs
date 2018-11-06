namespace VRTK.Core.Action
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when any given actions are in their active state.
    /// </summary>
    public class AnyAction : BooleanAction
    {
        /// <summary>
        /// BaseActions to check the active state on.
        /// </summary>
        [Tooltip("BaseActions to check the active state on.")]
        public List<BaseAction> actions = new List<BaseAction>();

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