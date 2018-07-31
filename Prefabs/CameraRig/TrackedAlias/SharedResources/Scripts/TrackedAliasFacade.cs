namespace VRTK.Core.Prefabs.CameraRig.TrackedAlias
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;
    using VRTK.Core.Tracking;
    using VRTK.Core.Tracking.Velocity;
    using System.Linq;
    using System;

    /// <summary>
    /// The public interface into the Tracked Alias Prefab.
    /// </summary>
    public class TrackedAliasFacade : MonoBehaviour
    {
        [Header("Tracked Alias Settings")]

        /// <summary>
        /// The linked CameraRigs to track.
        /// </summary>
        [Tooltip("The linked CameraRigs to track.")]
        public List<CameraRigAliasLinker> cameraRigs = new List<CameraRigAliasLinker>();

        [Header("Internal Settings")]

        /// <summary>
        /// **DO NOT CHANGE** - The linked Internal Setup.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked Internal Setup.")]
        public TrackedAliasInternalSetup internalSetup;

        /// <summary>
        /// Retrieves the active PlayArea that the TrackedAlias is using.
        /// </summary>
        public GameObject ActivePlayArea => PlayAreas.Select(element => element.gameObject).FirstOrDefault(gameObject => gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Headset that the TrackedAlias is using.
        /// </summary>
        public GameObject ActiveHeadset => Headsets.Select(element => element.gameObject).FirstOrDefault(gameObject => gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Headset Camera that the TrackedAlias is using.
        /// </summary>
        public Camera ActiveHeadsetCamera => HeadsetCameras.Select(element => element).FirstOrDefault(camera => camera.gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Headset Velocity Tracker that the TrackedAlias is using.
        /// </summary>
        public VelocityTracker ActiveHeadsetVelocity => HeadsetVelocityTrackers.Select(element => element).FirstOrDefault(velocityTracker => velocityTracker.gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Left Controller that the TrackedAlias is using.
        /// </summary>
        public GameObject ActiveLeftController => LeftControllers.Select(element => element.gameObject).FirstOrDefault(gameObject => gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Left Controller Velocity Tracker that the TrackedAlias is using.
        /// </summary>
        public VelocityTracker ActiveLeftControllerVelocity => LeftControllerVelocityTrackers.Select(element => element).FirstOrDefault(velocityTracker => velocityTracker.gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Right Controller that the TrackedAlias is using.
        /// </summary>
        public GameObject ActiveRightController => RightControllers.Select(element => element.gameObject).FirstOrDefault(gameObject => gameObject.activeInHierarchy);
        /// <summary>
        /// Retrieves the active Right Controller Velocity Tracker that the TrackedAlias is using.
        /// </summary>
        public VelocityTracker ActiveRightControllerVelocity => RightControllerVelocityTrackers.Select(element => element).FirstOrDefault(velocityTracker => velocityTracker.gameObject.activeInHierarchy);

        /// <summary>
        /// Retreives all of the linked CameraRig PlayAreas.
        /// </summary>
        public List<Component> PlayAreas => cameraRigs.Select(rig => rig.PlayArea).Where(value => value != null).Select(GameObjectExtensions.TryGetComponent).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Headsets.
        /// </summary>
        public List<Component> Headsets => cameraRigs.Select(rig => rig.Headset).Where(value => value != null).Select(GameObjectExtensions.TryGetComponent).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Headset Cameras.
        /// </summary>
        public List<Camera> HeadsetCameras => cameraRigs.Select(rig => rig.HeadsetCamera).Where(value => value != null).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Headset Velocity Trackers.
        /// </summary>
        public List<VelocityTracker> HeadsetVelocityTrackers => cameraRigs.Select(rig => rig.HeadsetVelocity).Where(value => value != null).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Left Controllers.
        /// </summary>
        public List<Component> LeftControllers => cameraRigs.Select(rig => rig.LeftController).Where(value => value != null).Select(GameObjectExtensions.TryGetComponent).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Right Controllers.
        /// </summary>
        public List<Component> RightControllers => cameraRigs.Select(rig => rig.RightController).Where(value => value != null).Select(GameObjectExtensions.TryGetComponent).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Left Controller Velocity Trackers.
        /// </summary>
        public List<VelocityTracker> LeftControllerVelocityTrackers => cameraRigs.Select(rig => rig.LeftControllerVelocity).Where(value => value != null).ToList();
        /// <summary>
        /// Retreives all of the linked CameraRig Right Controller Velocity Trackers.
        /// </summary>
        public List<VelocityTracker> RightControllerVelocityTrackers => cameraRigs.Select(rig => rig.RightControllerVelocity).Where(value => value != null).ToList();
    }
}