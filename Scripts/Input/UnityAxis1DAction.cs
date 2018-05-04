namespace VRTK.Core.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// The UnityAxis1DAction listens for the specified axis and emits the appropriate action.
    /// </summary>
    public class UnityAxis1DAction : FloatAction
    {
        [Tooltip("The named axis to listen for state changes on.")]
        public string axisName;

        protected virtual void Update()
        {
            Value = Input.GetAxis(axisName);
            EmitEvents();
            State = IsActive();
            previousValue = Value;
        }
    }
}