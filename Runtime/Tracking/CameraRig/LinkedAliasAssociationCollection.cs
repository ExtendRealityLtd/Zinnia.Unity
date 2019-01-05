namespace Zinnia.Tracking.CameraRig
{
    using UnityEngine;
    using Zinnia.Tracking.Velocity;

    /// <summary>
    /// Provides the basis for describing a CameraRig Alias Association by providing the linked elements of the CameraRig.
    /// </summary>
    public class LinkedAliasAssociationCollection : AliasAssociationCollection
    {
        #region PlayArea Settings
        /// <summary>
        /// The associated PlayArea.
        /// </summary>
        [Header("PlayArea Settings"), SerializeField]
        protected GameObject playArea;
        #endregion

        #region Headset Settings
        /// <summary>
        /// The associated Headset.
        /// </summary>
        [Header("Headset Settings"), SerializeField]
        protected GameObject headset;
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        [SerializeField]
        protected Camera headsetCamera;
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        [SerializeField]
        protected VelocityTracker headsetVelocity;
        #endregion

        #region Left Controller Settings
        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        [Header("Left Controller Settings"), SerializeField]
        protected GameObject leftController;
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        [SerializeField]
        protected VelocityTracker leftControllerVelocity;
        #endregion

        #region Right Controller Settings
        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        [Header("Right Controller Settings"), SerializeField]
        protected GameObject rightController;
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        [SerializeField]
        protected VelocityTracker rightControllerVelocity;
        #endregion

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