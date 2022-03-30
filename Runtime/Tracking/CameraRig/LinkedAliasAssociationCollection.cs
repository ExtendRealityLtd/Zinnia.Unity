namespace Zinnia.Tracking.CameraRig
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;
    using Zinnia.Haptics;
    using Zinnia.Haptics.Collection;
    using Zinnia.Tracking.Velocity;

    /// <summary>
    /// Provides the basis for describing a CameraRig Alias Association by providing the linked elements of the CameraRig.
    /// </summary>
    public class LinkedAliasAssociationCollection : MonoBehaviour
    {
        #region PlayArea Settings
        /// <summary>
        /// The associated PlayArea.
        /// </summary>
        [Serialized]
        [field: Header("PlayArea Settings"), DocumentedByXml]
        public GameObject PlayArea { get; set; }
        #endregion

        #region Headset Settings
        /// <summary>
        /// The associated Headset.
        /// </summary>
        [Serialized]
        [field: Header("Headset Settings"), DocumentedByXml]
        public GameObject Headset { get; set; }
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Camera HeadsetCamera { get; set; }
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public VelocityTracker HeadsetVelocityTracker { get; set; }
        /// <summary>
        /// A list of any additional cameras associated with the headset.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public CameraObservableList SupplementHeadsetCameras { get; set; }
        /// <summary>
        /// The details and status of the headset device.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public DeviceDetailsRecord HeadsetDeviceDetails { get; set; }
        /// <summary>
        /// The dominant controller observer.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public DominantControllerObserver DominantController { get; set; }
        #endregion

        #region Left Controller Settings
        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        [Serialized]
        [field: Header("Left Controller Settings"), DocumentedByXml]
        public GameObject LeftController { get; set; }
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public VelocityTracker LeftControllerVelocityTracker { get; set; }
        /// <summary>
        /// The main Left Controller Haptic Process profile.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcess LeftControllerHapticProcess { get; set; }
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of haptic profiles that can be used with the Left Controller.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcessObservableList LeftControllerHapticProfiles { get; set; }
        /// <summary>
        /// The details and status of the left controller device.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public DeviceDetailsRecord LeftControllerDeviceDetails { get; set; }
        #endregion

        #region Right Controller Settings
        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        [Serialized]
        [field: Header("Right Controller Settings"), DocumentedByXml]
        public GameObject RightController { get; set; }
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public VelocityTracker RightControllerVelocityTracker { get; set; }
        /// <summary>
        /// The main Right Controller Haptic Process profile.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcess RightControllerHapticProcess { get; set; }
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of supplement haptic settings that can be used with the Right Controller.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public HapticProcessObservableList RightControllerHapticProfiles { get; set; }
        /// <summary>
        /// The details and status of the right controller device.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public DeviceDetailsRecord RightControllerDeviceDetails { get; set; }
        #endregion

        /// <summary>
        /// Clears <see cref="PlayArea"/>.
        /// </summary>
        public virtual void ClearPlayArea()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PlayArea = default;
        }

        /// <summary>
        /// Clears <see cref="Headset"/>.
        /// </summary>
        public virtual void ClearHeadset()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Headset = default;
        }

        /// <summary>
        /// Clears <see cref="HeadsetCamera"/>.
        /// </summary>
        public virtual void ClearHeadsetCamera()
        {
            if (!this.IsValidState())
            {
                return;
            }

            HeadsetCamera = default;
        }

        /// <summary>
        /// Clears <see cref="HeadsetVelocityTracker"/>.
        /// </summary>
        public virtual void ClearHeadsetVelocityTracker()
        {
            if (!this.IsValidState())
            {
                return;
            }

            HeadsetVelocityTracker = default;
        }

        /// <summary>
        /// Clears <see cref="SupplementHeadsetCameras"/>.
        /// </summary>
        public virtual void ClearSupplementHeadsetCameras()
        {
            if (!this.IsValidState())
            {
                return;
            }

            SupplementHeadsetCameras = default;
        }

        /// <summary>
        /// Clears <see cref="HeadsetDeviceDetails"/>.
        /// </summary>
        public virtual void ClearHeadsetDeviceDetails()
        {
            if (!this.IsValidState())
            {
                return;
            }

            HeadsetDeviceDetails = default;
        }

        /// <summary>
        /// Clears <see cref="DominantController"/>.
        /// </summary>
        public virtual void ClearDominantController()
        {
            if (!this.IsValidState())
            {
                return;
            }

            DominantController = default;
        }

        /// <summary>
        /// Clears <see cref="LeftController"/>.
        /// </summary>
        public virtual void ClearLeftController()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LeftController = default;
        }

        /// <summary>
        /// Clears <see cref="LeftControllerVelocityTracker"/>.
        /// </summary>
        public virtual void ClearLeftControllerVelocityTracker()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LeftControllerVelocityTracker = default;
        }

        /// <summary>
        /// Clears <see cref="LeftControllerHapticProcess"/>.
        /// </summary>
        public virtual void ClearLeftControllerHapticProcess()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LeftControllerHapticProcess = default;
        }

        /// <summary>
        /// Clears <see cref="LeftControllerHapticProfiles"/>.
        /// </summary>
        public virtual void ClearLeftControllerHapticProfiles()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LeftControllerHapticProfiles = default;
        }

        /// <summary>
        /// Clears <see cref="LeftControllerDeviceDetails"/>.
        /// </summary>
        public virtual void ClearLeftControllerDeviceDetails()
        {
            if (!this.IsValidState())
            {
                return;
            }

            LeftControllerDeviceDetails = default;
        }

        /// <summary>
        /// Clears <see cref="RightController"/>.
        /// </summary>
        public virtual void ClearRightController()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RightController = default;
        }

        /// <summary>
        /// Clears <see cref="RightControllerVelocityTracker"/>.
        /// </summary>
        public virtual void ClearRightControllerVelocityTracker()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RightControllerVelocityTracker = default;
        }

        /// <summary>
        /// Clears <see cref="RightControllerHapticProcess"/>.
        /// </summary>
        public virtual void ClearRightControllerHapticProcess()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RightControllerHapticProcess = default;
        }

        /// <summary>
        /// Clears <see cref="RightControllerHapticProfiles"/>.
        /// </summary>
        public virtual void ClearRightControllerHapticProfiles()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RightControllerHapticProfiles = default;
        }

        /// <summary>
        /// Clears <see cref="RightControllerDeviceDetails"/>.
        /// </summary>
        public virtual void ClearRightControllerDeviceDetails()
        {
            if (!this.IsValidState())
            {
                return;
            }

            RightControllerDeviceDetails = default;
        }
    }
}