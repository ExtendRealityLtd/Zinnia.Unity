namespace VRTK.Core.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// The UnityAxis2DAction listens for the specified axes and emits the appropriate action.
    /// </summary>
    public class UnityAxis2DAction : Vector2Action
    {
        [Tooltip("The named x axis to listen for state changes on.")]
        public string xAxisName;
        [Tooltip("The named y axis to listen for state changes on.")]
        public string yAxisName;

        protected virtual void Update()
        {
            Value = new Vector2(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName));
            EmitEvents();
            state = IsActive();
            previousValue = Value;
        }
    }
}