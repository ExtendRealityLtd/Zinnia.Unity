namespace VRTK.Core.Action
{
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when all given actions are in their active state.
    /// </summary>
    public class AllAction : BooleanAction
    {
        /// <summary>
        /// Actions to check the active state on.
        /// </summary>
        [Tooltip("Actions to check the active state on.")]
        public List<Action> actions = new List<Action>();

        protected virtual void Update()
        {
            bool areAllActionsActivated = actions.EmptyIfNull().All(action => action.IsActivated);
            if (areAllActionsActivated != IsActivated)
            {
                Receive(areAllActionsActivated);
            }
        }
    }
}