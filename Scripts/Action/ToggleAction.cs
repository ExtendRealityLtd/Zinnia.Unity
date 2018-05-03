namespace VRTK.Core.Action
{
    /// <summary>
    /// The BooleanAction emits a boolean value.
    /// </summary>
    public class ToggleAction : BooleanAction
    {
        /// <summary>
        /// The Receive method allows an action to receive the payload from another action to enable action chaining.
        /// </summary>
        /// <param name="value">The value from the action.</param>
        /// <param name="sender">The sender of the action.</param>
        public override void Receive(bool value, object sender = null)
        {
            previousValue = Value;
            Value = value;

            if (!state)
            {
                OnActivated(true);
            }
            else
            {
                OnDeactivated(false);
            }

            OnChanged(value);
        }
    }
}