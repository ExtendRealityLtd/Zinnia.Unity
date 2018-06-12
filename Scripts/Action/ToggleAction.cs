namespace VRTK.Core.Action
{
    /// <summary>
    /// Emits an Activated event on the first time Receive is called and emits Deactivated on the second time Receive is called to provide a toggle state.
    /// </summary>
    public class ToggleAction : BooleanAction
    {
        /// <inheritdoc />
        public override void Receive(bool value, object initiator = null)
        {
            previousValue = Value;
            Value = value;

            if (!State)
            {
                OnActivated(true, initiator);
            }
            else
            {
                OnDeactivated(false, initiator);
            }

            OnChanged(value, initiator);
        }
    }
}