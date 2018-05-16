namespace VRTK.Core.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// Listens for the specified axes and emits the appropriate action.
    /// </summary>
    public class UnityAxis2DAction : Vector2Action
    {
        /// <summary>
        /// The named x axis to listen for state changes on.
        /// </summary>
        [Tooltip("The named x axis to listen for state changes on.")]
        public string xAxisName;
        /// <summary>
        /// The named y axis to listen for state changes on.
        /// </summary>
        [Tooltip("The named y axis to listen for state changes on.")]
        public string yAxisName;

        protected virtual void Update()
        {
            Value = new Vector2(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName));
            EmitEvents();
            State = IsActive();
            previousValue = Value;
        }
    }
}