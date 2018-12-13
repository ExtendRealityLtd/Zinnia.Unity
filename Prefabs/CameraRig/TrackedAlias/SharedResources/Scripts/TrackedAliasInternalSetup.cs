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
        #region Facade Settings
        /// <summary>
        /// The public interface facade.
        /// </summary>
        [Header("Facade Settings"), Tooltip("The public interface facade.")]
        public TrackedAliasFacade facade;
        #endregion

        #region Object Follow Settings
        /// <summary>
        /// The <see cref="ObjectFollower"/> component for the PlayArea.
        /// </summary>
        [Header("Object Follow Settings"), Tooltip("The ObjectFollower component for the PlayArea.")]
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
        #endregion

        #region Velocity Tracker Settings
        /// <summary>
        /// The <see cref="VelocityTrackerProcessor"/> component containing the Headset Velocity Trackers.
        /// </summary>
        [Header("Velocity Tracker Settings"), Tooltip("The VelocityTrackerProcessor component containing the Headset Velocity Trackers.")]
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
        #endregion

        #region Other Settings
        /// <summary>
        /// The <see cref="CameraList"/> component containing the valid scene cameras.
        /// </summary>
        [Header("Other Settings"), Tooltip("The CameraList component containing the valid scene cameras.")]
        public CameraList sceneCameras;
        #endregion

        /// <summary>
        /// Sets up the TrackedAlias prefab with the specified settings.
        /// </summary>
        public virtual void Setup()
        {
            if (InvalidParameters())
            {
                return;
            }

            playArea.targets.AddRange(facade.PlayAreas);
            headset.sources.AddRange(facade.Headsets);
            sceneCameras.cameras.AddRange(facade.HeadsetCameras);
            headsetVelocityTrackers.velocityTrackers.AddRange(facade.HeadsetVelocityTrackers);
            leftController.sources.AddRange(facade.LeftControllers);
            rightController.sources.AddRange(facade.RightControllers);
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

            playArea.ClearTargets();
            headset.ClearSources();
            sceneCameras.cameras.Clear();
            headsetVelocityTrackers.velocityTrackers.Clear();
            leftController.ClearSources();
            rightController.ClearSources();
            leftControllerVelocityTrackers.velocityTrackers.Clear();
            rightControllerVelocityTrackers.velocityTrackers.Clear();
        }

        /// <summary>
        /// Notifies that the headset has started being tracked.
        /// </summary>
        public virtual void NotifyHeadsetTrackingBegun()
        {
            facade.HeadsetTrackingBegun?.Invoke();
        }

        /// <summary>
        /// Notifies that the left controller has started being tracked.
        /// </summary>
        public virtual void NotifyLeftControllerTrackingBegun()
        {
            facade.LeftControllerTrackingBegun?.Invoke();
        }

        /// <summary>
        /// Notifies that the right controller has started being tracked.
        /// </summary>
        public virtual void NotifyRightControllerTrackingBegun()
        {
            facade.RightControllerTrackingBegun?.Invoke();
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