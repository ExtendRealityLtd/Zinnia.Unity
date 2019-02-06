namespace Zinnia.Action
{
    /// <summary>
    /// Emits an Activated event on the first time Receive is called and emits Deactivated on the second time Receive is called to provide a toggle state.
    /// </summary>
    public class ToggleAction : BooleanAction
    {
        /// <inheritdoc />
        public override void Receive(bool value)
        {
            base.Receive(!Value);
        }
    }
}