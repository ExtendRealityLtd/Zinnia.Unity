namespace Zinnia.Tracking.CameraRig
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
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
        [Serialized, Cleared]
        [field: Header("PlayArea Settings"), DocumentedByXml]
        public GameObject PlayArea { get; set; }
        #endregion

        #region Headset Settings
        /// <summary>
        /// The associated Headset.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Headset Settings"), DocumentedByXml]
        public GameObject Headset { get; set; }
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public Camera HeadsetCamera { get; set; }
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker HeadsetVelocityTracker { get; set; }
        /// <summary>
        /// A list of any additional cameras associated with the headset.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public CameraObservableList SupplementHeadsetCameras { get; set; }
        /// <summary>
        /// The details and status of the headset device.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public DeviceDetailsRecord HeadsetDeviceDetails { get; set; }
        /// <summary>
        /// The dominant controller observer.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public DominantControllerObserver DominantController { get; set; }
        #endregion

        #region Left Controller Settings
        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Left Controller Settings"), DocumentedByXml]
        public GameObject LeftController { get; set; }
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker LeftControllerVelocityTracker { get; set; }
        /// <summary>
        /// The main Left Controller Haptic Process profile.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcess LeftControllerHapticProcess { get; set; }
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of haptic profiles that can be used with the Left Controller.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcessObservableList LeftControllerHapticProfiles { get; set; }
        /// <summary>
        /// The details and status of the left controller device.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public DeviceDetailsRecord LeftControllerDeviceDetails { get; set; }
        #endregion

        #region Right Controller Settings
        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Right Controller Settings"), DocumentedByXml]
        public GameObject RightController { get; set; }
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker RightControllerVelocityTracker { get; set; }
        /// <summary>
        /// The main Right Controller Haptic Process profile.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcess RightControllerHapticProcess { get; set; }
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of supplement haptic settings that can be used with the Right Controller.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcessObservableList RightControllerHapticProfiles { get; set; }
        /// <summary>
        /// The details and status of the right controller device.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public DeviceDetailsRecord RightControllerDeviceDetails { get; set; }
        #endregion
    }
}