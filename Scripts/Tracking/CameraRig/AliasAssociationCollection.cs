namespace Zinnia.Tracking.CameraRig
{
    using UnityEngine;
    using Zinnia.Tracking.Velocity;

    /// <summary>
    /// Provides a basis for describing a common VR based CameraRig for linking to a TrackedAlias.
    /// </summary>
    public abstract class AliasAssociationCollection : MonoBehaviour
    {
        /// <summary>
        /// The CameraRig PlayArea.
        /// </summary>
        public GameObject PlayArea => GetPlayArea();
        /// <summary>
        /// The CameraRig Headset
        /// </summary>
        public GameObject Headset => GetHeadset();
        /// <summary>
        /// The CameraRig Headset Camera.
        /// </summary>
        public Camera HeadsetCamera => GetHeadsetCamera();
        /// <summary>
        /// The CameraRig Headset Velocity Tracker.
        /// </summary>
        public VelocityTracker HeadsetVelocity => GetHeadsetVelocity();
        /// <summary>
        /// The CameraRig Left Controller.
        /// </summary>
        public GameObject LeftController => GetLeftController();
        /// <summary>
        /// The CameraRig Left Controller Velocity Tracker.
        /// </summary>
        public VelocityTracker LeftControllerVelocity => GetLeftControllerVelocity();
        /// <summary>
        /// The CameraRig Right Controller.
        /// </summary>
        public GameObject RightController => GetRightController();
        /// <summary>
        /// The CameraRig Right Controller Velocity Tracker.
        /// </summary>
        public VelocityTracker RightControllerVelocity => GetRightControllerVelocity();

        /// <summary>
        /// Retrieves the PlayArea.
        /// </summary>
        /// <returns>The PlayArea.</returns>
        protected abstract GameObject GetPlayArea();
        /// <summary>
        /// Retrieves the Headset.
        /// </summary>
        /// <returns>The Headset.</returns>
        protected abstract GameObject GetHeadset();
        /// <summary>
        /// Retrieves the Headset Camera.
        /// </summary>
        /// <returns>The Headset Camera.</returns>
        protected abstract Camera GetHeadsetCamera();
        /// <summary>
        /// Retrieves the Headset Velocity Tracker.
        /// </summary>
        /// <returns>The Headset Velocity Tracker.</returns>
        protected abstract VelocityTracker GetHeadsetVelocity();
        /// <summary>
        /// Retrieves the Left Controller.
        /// </summary>
        /// <returns>The Left Controller.</returns>
        protected abstract GameObject GetLeftController();
        /// <summary>
        /// Retrieves the Left Controller Velocity Tracker.
        /// </summary>
        /// <returns>The Left Controller Velocity Tracker.</returns>
        protected abstract VelocityTracker GetLeftControllerVelocity();
        /// <summary>
        /// Retrieves the Right Controller.
        /// </summary>
        /// <returns>The Right Controller.</returns>
        protected abstract GameObject GetRightController();
        /// <summary>
        /// Retrieves the Right Controller Velocity Tracker.
        /// </summary>
        /// <returns>The Right Controller Velocity Tracker.</returns>
        protected abstract VelocityTracker GetRightControllerVelocity();
    }
}