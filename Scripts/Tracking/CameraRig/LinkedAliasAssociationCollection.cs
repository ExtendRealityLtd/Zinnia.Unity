namespace VRTK.Core.Tracking.CameraRig
{
    using UnityEngine;
    using VRTK.Core.Tracking.Velocity;

    /// <summary>
    /// Provides the basis for describing a CameraRig Alias Association by providing the linked elements of the CameraRig.
    /// </summary>
    public class LinkedAliasAssociationCollection : BaseAliasAssociationCollection
    {
        [Header("PlayArea Settings")]

        /// <summary>
        /// The associated PlayArea.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated PlayArea.")]
        protected GameObject playArea;

        [Header("Headset Settings")]

        /// <summary>
        /// The associated Headset.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Headset.")]
        protected GameObject headset;
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Headset Camera.")]
        protected Camera headsetCamera;
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Headset Velocity Tracker.")]
        protected VelocityTracker headsetVelocity;

        [Header("Left Controller Settings")]

        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Left Controller.")]
        protected GameObject leftController;
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Left Controller Velocity Tracker.")]
        protected VelocityTracker leftControllerVelocity;

        [Header("Right Controller Settings")]

        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Right Controller.")]
        protected GameObject rightController;
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        [SerializeField]
        [Tooltip("The associated Right Controller Velocity Tracker.")]
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