namespace VRTK.Core.Action
{
    using UnityEngine;

    /// <summary>
    /// The CompoundAndAction emits an event when all given actions are in their active state.
    /// </summary>
    public class CompoundAndAction : BooleanAction
    {
        [Tooltip("An array of BaseActions to check the active state on.")]
        public BaseAction[] actions;

        /// <summary>
        /// The Receive method is not used.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(bool value, object sender = null)
        {
        }

        protected virtual void Update()
        {
            bool isValid = true;
            foreach (BaseAction action in actions)
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