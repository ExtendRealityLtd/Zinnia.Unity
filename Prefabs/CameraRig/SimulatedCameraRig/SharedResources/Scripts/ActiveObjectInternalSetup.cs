namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Data.Type.Transformation;
    using VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig.Input;

    /// <summary>
    /// Sets up the active controllable objects.
    /// </summary>
    public class ActiveObjectInternalSetup : MonoBehaviour
    {
        /// <summary>
        /// The speed to move the free mouse cursor.
        /// </summary>
        [Tooltip("The speed to move the free mouse cursor.")]
        public float freeCursorRotationSpeed = 0.2f;
        /// <summary>
        /// The speed to move the locked mouse cursor.
        /// </summary>
        [Tooltip("The speed to move the locked mouse cursor.")]
        public float lockedCursorRotationSpeed = 3f;
        /// <summary>
        /// The speed to move the object at.
        /// </summary>
        [Tooltip("The speed to move the object at.")]
        public float movementSpeed = 0.025f;
        /// <summary>
        /// The mouse input action to update.
        /// </summary>
        [Tooltip("The mouse input action to update.")]
        public MouseVector2DAction mouseInput;
        /// <summary>
        /// A collection of multipliers that deal with positive floats.
        /// </summary>
        [Tooltip("A collection of multipliers that deal with positive floats.")]
        public List<FloatMultiplier> positiveMultipliers = new List<FloatMultiplier>();
        /// <summary>
        /// A collection of multipliers that deal with negative floats.
        /// </summary>
        [Tooltip("A collection of multipliers that deal with negative floats.")]
        public List<FloatMultiplier> negativeMultipliers = new List<FloatMultiplier>();

        protected virtual void OnEnable()
        {
            if (mouseInput != null)
            {
                mouseInput.cursorMultiplier = freeCursorRotationSpeed;
                mouseInput.lockedCursorMultiplier = lockedCursorRotationSpeed;
            }

            foreach (FloatMultiplier positiveMultiplier in positiveMultipliers)
            {
                positiveMultiplier.SetMultiplier(movementSpeed);
            }

            foreach (FloatMultiplier negativeMultiplier in negativeMultipliers)
            {
                negativeMultiplier.SetMultiplier(-movementSpeed);
            }
        }
    }
}