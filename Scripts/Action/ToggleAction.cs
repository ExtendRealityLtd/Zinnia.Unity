namespace VRTK.Core.Action
{
    /// <summary>
    /// The ToggleAction emits an Activated event on the first time Receive is called and emits Deactivated on the second time Receive is called to provide a toggle state.
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

            if (!State)
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