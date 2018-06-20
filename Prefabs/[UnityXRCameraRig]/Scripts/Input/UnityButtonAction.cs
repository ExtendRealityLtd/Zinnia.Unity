namespace VRTK.Core.Prefabs.UnityXRCameraRig.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// Listens for the specified key state and emits the appropriate action.
    /// </summary>
    public class UnityButtonAction : BooleanAction
    {
        /// <summary>
        /// The <see cref="KeyCode"/> to listen for state changes on.
        /// </summary>
        [Tooltip("The key code to listen for state changes on.")]
        public KeyCode keyCode;

        protected virtual void Update()
        {
            Receive(Input.GetKey(keyCode));
        }
    }
}