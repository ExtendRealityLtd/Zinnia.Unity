namespace VRTK.Core.Action
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when all given actions are in their active state.
    /// </summary>
    public class CompoundAndAction : BooleanAction
    {
        /// <summary>
        /// BaseActions to check the active state on.
        /// </summary>
        [Tooltip("BaseActions to check the active state on.")]
        public List<BaseAction> actions = new List<BaseAction>();

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(bool value, object sender = null)
        {
        }

        protected virtual void Update()
        {
            bool isValid = true;
            foreach (BaseAction action in actions.EmptyIfNull())
            {
                if (!action.State)
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid && !State)
            {
                OnActivated(true);
            }

            if (!isValid && State)
            {
                OnDeactivated(true);
            }

            State = isValid;
            previousValue = Value;
            Value = State;

            if (HasChanged())
            {
                OnChanged(Value);
            }
        }
    }
}