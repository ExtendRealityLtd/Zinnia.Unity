namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Core.Prefabs.CameraRig.UnityXRCameraRig.Input;

    /// <summary>
    /// Sets up the key binding display.
    /// </summary>
    public class KeyBindingDisplay : MonoBehaviour
    {
        public Text keyBindingText;
        public UnityButtonAction forward;
        public UnityButtonAction backward;
        public UnityButtonAction strafeLeft;
        public UnityButtonAction strafeRight;
        public UnityButtonAction button1;
        public UnityButtonAction button2;
        public UnityButtonAction button3;
        public UnityButtonAction switchToPlayer;
        public UnityButtonAction switchToLeftController;
        public UnityButtonAction switchToRightController;
        public UnityButtonAction resetPlayer;
        public UnityButtonAction resetControllers;
        public UnityButtonAction toggleInstructions;
        public UnityButtonAction lockCursorToggle;

        protected string instructions = @"<b>Simulator Key Bindings</b>

<b>Movement:</b>
Forward: {0}
Backward: {1}
Strafe Left: {2}
Strafe Right: {3}

<b>Buttons</b>
Button 1: {4}
Button 2: {5}
Button 3: {6}

<b>Object Control</b>
Move PlayArea: {7}
Move Left Controller: {8}
Move Right Controller: {9}
Reset Player: Position {10}
Reset Controller Position: {11}

<b>Misc</b>
Toggle Help Window: {12}
Lock Mouse Cursor: {13}";

        protected virtual void OnEnable()
        {
            keyBindingText.text = string.Format(
                instructions,
                forward.keyCode,
                backward.keyCode,
                strafeLeft.keyCode,
                strafeRight.keyCode,
                button1.keyCode,
                button2.keyCode,
                button3.keyCode,
                switchToPlayer.keyCode,
                switchToLeftController.keyCode,
                switchToRightController.keyCode,
                resetPlayer.keyCode,
                resetControllers.keyCode,
                toggleInstructions.keyCode,
                lockCursorToggle.keyCode
                );
        }
    }
}