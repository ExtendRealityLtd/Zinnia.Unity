namespace VRTK.Core.Prefabs.CameraRig.UnityXRCameraRig.Tracking
{
    using UnityEngine;
    using VRTK.Core.Tracking;
    using VRTK.Core.Tracking.Velocity;

    /// <summary>
    /// Provides the basis for describing the default Unity XR CameraRig prefab.
    /// </summary>
    public class UnityXRCameraRigAliasLinker : CameraRigAliasLinker
    {
        [Header("PlayArea Settings")]

        /// <summary>
        /// The PlayArea associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The PlayArea associated with the Unity XR CameraRig.")]
        protected GameObject playArea;

        [Header("Headset Settings")]

        /// <summary>
        /// The Headset associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Headset associated with the Unity XR CameraRig.")]
        protected GameObject headset;
        /// <summary>
        /// The Headset Camera associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Headset Camera associated with the Unity XR CameraRig.")]
        protected Camera headsetCamera;
        /// <summary>
        /// The Headset Velocity Tracker associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Headset Velocity Tracker associated with the Unity XR CameraRig.")]
        protected VelocityTracker headsetVelocity;

        [Header("Left Controller Settings")]

        /// <summary>
        /// The Left Controller associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Left Controller associated with the Unity XR CameraRig.")]
        protected GameObject leftController;
        /// <summary>
        /// The Left Controller Velocity Tracker associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Left Controller Velocity Tracker associated with the Unity XR CameraRig.")]
        protected VelocityTracker leftControllerVelocity;

        [Header("Right Controller Settings")]

        /// <summary>
        /// The Right Controller associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Right Controller associated with the Unity XR CameraRig.")]
        protected GameObject rightController;
        /// <summary>
        /// The Right Controller Velocity Tracker associated with the Unity XR CameraRig.
        /// </summary>
        [SerializeField]
        [Tooltip("The Right Controller Velocity Tracker associated with the Unity XR CameraRig.")]
        protected VelocityTracker rightControllerVelocity;

        /// <inheritdoc />
        protected override GameObject GetPlayArea()
        {
            return playArea;
        }

        /// <inheritdoc />
        protected override GameObject GetHeadset()
        {
            return headset;
        }

        /// <inheritdoc />
        protected override Camera GetHeadsetCamera()
        {
            return headsetCamera;
        }

        /// <inheritdoc />
        protected override VelocityTracker GetHeadsetVelocity()
        {
            return headsetVelocity;
        }

        /// <inheritdoc />
        protected override GameObject GetLeftController()
        {
            return leftController;
        }

        /// <inheritdoc />
        protected override VelocityTracker GetLeftControllerVelocity()
        {
            return leftControllerVelocity;
        }

        /// <inheritdoc />
        protected override GameObject GetRightController()
        {
            return rightController;
        }

        /// <inheritdoc />
        protected override VelocityTracker GetRightControllerVelocity()
        {
            return rightControllerVelocity;
        }
    }
}