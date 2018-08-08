namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig.Input
{
    using UnityEngine;
    using VRTK.Core.Action;

    /// <summary>
    /// Listens for input from the mouse and converts into Vector2 data.
    /// </summary>
    public class MouseVector2DAction : Vector2Action
    {
        /// <summary>
        /// The named x axis of the mouse.
        /// </summary>
        [Tooltip("The named x axis of the mouse.")]
        public string xAxisName = "Mouse X";
        /// <summary>
        /// The named y axis of the mouse.
        /// </summary>
        [Tooltip("The named y axis of the mouse.")]
        public string yAxisName = "Mouse Y";
        /// <summary>
        /// Determines whether to lock the cursor in the game window.
        /// </summary>
        [Tooltip("Determines whether to lock the cursor in the game window.")]
        public bool lockCursor = false;
        /// <summary>
        /// Multiplies the speed at which the unlocked cursor moves the axis.
        /// </summary>
        [Tooltip("Multiplies the speed at which the unlocked cursor moves the axis.")]
        public float cursorMultiplier = 1f;
        /// <summary>
        /// Multiplies the speed at which the locked cursor moves the axis.
        /// </summary>
        [Tooltip("Multiplies the speed at which the locked cursor moves the axis.")]
        public float lockedCursorMultiplier = 2f;

        protected Vector3 previousMousePosition;

        /// <summary>
        /// Toggles the <see cref="lockCursor"/> parameter state.
        /// </summary>
        public virtual void ToggleLockCursorState()
        {
            lockCursor = !lockCursor;
        }

        protected override void OnEnable()
        {
            previousMousePosition = Input.mousePosition;
            base.OnEnable();
        }

        protected virtual void Update()
        {
            Cursor.lockState = (lockCursor ? CursorLockMode.Locked : CursorLockMode.None);
            Vector3 mouseData = GetMouseDelta();
            Receive(new Vector2(mouseData.x, mouseData.y));
        }

        protected virtual Vector3 GetMouseDelta()
        {
            Vector3 difference = Input.mousePosition - previousMousePosition;
            previousMousePosition = Input.mousePosition;

            return (Cursor.lockState == CursorLockMode.Locked
                ? new Vector3(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName)) * lockedCursorMultiplier
                : difference * cursorMultiplier);
        }
    }
}