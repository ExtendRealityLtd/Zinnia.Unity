namespace VRTK.Core.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// The UnityButtonAction listens for the specified key state and emits the appropriate action.
    /// </summary>
    public class UnityButtonAction : BooleanAction
    {
        [Tooltip("The key code to listen for state changes on.")]
        public KeyCode keyCode;

        protected virtual void Update()
        {
            Value = Input.GetKey(keyCode);
            if (Input.GetKeyDown(keyCode))
            {
                OnActivated(true);
            }

            if (HasChanged())
            {
                OnChanged(Value);
            }

            if (Input.GetKeyUp(keyCode))
            {
                OnDeactivated(false);
            }
            previousValue = Value;
        }
    }
}