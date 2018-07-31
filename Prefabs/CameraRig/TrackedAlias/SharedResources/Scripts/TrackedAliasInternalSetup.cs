namespace VRTK.Core.Prefabs.CameraRig.TrackedAlias
{
    using UnityEngine;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Tracking.Follow;
    using VRTK.Core.Tracking.Velocity;

    /// <summary>
    /// Sets up the Tracked Alias Prefab based on the provided user settings.
    /// </summary>
    public class TrackedAliasInternalSetup : MonoBehaviour
    {
        [Header("Facade Settings")]

        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Tooltip("The public interface facade.")]
        public TrackedAliasFacade facade;

        [Header("Object Follow Settings")]

        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the PlayArea.
        /// </summary>
        [Tooltip("The ObjectFollower component for the PlayArea.")]
        public ObjectFollower playArea;
        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the Headset.
        /// </summary>
        [Tooltip("The ObjectFollower component for the Headset.")]
        public ObjectFollower headset;
        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the Left Controller.
        /// </summary>
        [Tooltip("The ObjectFollower component for the Left Controller.")]
        public ObjectFollower leftController;
        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the Right Controller.
        /// </summary>
        [Tooltip("The ObjectFollower component for the Right Controller.")]
        public ObjectFollower rightController;

        [Header("Velocity Tracker Settings")]

        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> component containing the Headset Velocity Trackers.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor component containing the Headset Velocity Trackers.")]
        public VelocityTrackerProcessor headsetVelocityTrackers;
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> component containing the Left Controller Velocity Trackers.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor component containing the Left Controller Velocity Trackers.")]
        public VelocityTrackerProcessor leftControllerVelocityTrackers;
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> component containing the Right Controller Velocity Trackers.
        /// </summary>
        [Tooltip("The VelocityTrackerProcessor component containing the Right Controller Velocity Trackers.")]
        public VelocityTrackerProcessor rightControllerVelocityTrackers;

        [Header("Other Settings")]

        /// <summary>
        /// The <see cref="CameraList"/> component containing the valid scene cameras.
        /// </summary>
        [Tooltip("The CameraList component containing the valid scene cameras.")]
        public CameraList sceneCameras;

        /// <summary>
        /// Sets up the TrackedAlias prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            playArea.targetComponents.AddRange(facade.PlayAreas);
            headset.targetComponents.AddRange(facade.Headsets);
            sceneCameras.cameras.AddRange(facade.HeadsetCameras);
            headsetVelocityTrackers.velocityTrackers.AddRange(facade.HeadsetVelocityTrackers);
            leftController.targetComponents.AddRange(facade.LeftControllers);
            rightController.targetComponents.AddRange(facade.RightControllers);
            leftControllerVelocityTrackers.velocityTrackers.AddRange(facade.LeftControllerVelocityTrackers);
            rightControllerVelocityTrackers.velocityTrackers.AddRange(facade.RightControllerVelocityTrackers);
        }

        /// <summary>
        /// Clears all of the settings from the TrackedAlias prefab.
        /// </summary>
        public virtual void Clear()
        {
            if (InvalidParameters())
            {
                return;
            }

            playArea.targetComponents.Clear();
            headset.targetComponents.Clear();
            sceneCameras.cameras.Clear();
            headsetVelocityTrackers.velocityTrackers.Clear();
            leftController.targetComponents.Clear();
            rightController.targetComponents.Clear();
            leftControllerVelocityTrackers.velocityTrackers.Clear();
            rightControllerVelocityTrackers.velocityTrackers.Clear();
        }

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void OnDisable()
        {
            Clear();
        }

        /// <summary>
        /// Determines if the setup parameters are invalid.
        /// </summary>
        /// <returns><see langword="true"/> if the parameters are invalid.</returns>
        protected virtual bool InvalidParameters()
        {
            return (playArea == null || headset == null || sceneCameras == null || headsetVelocityTrackers == null || leftController == null || rightController == null || leftControllerVelocityTrackers == null || rightControllerVelocityTrackers == null || facade == null);
        }
    }
}