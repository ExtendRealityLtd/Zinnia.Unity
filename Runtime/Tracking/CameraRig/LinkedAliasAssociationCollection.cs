namespace Zinnia.Tracking.CameraRig
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Haptics;
    using Zinnia.Tracking.Velocity;
    using Zinnia.Data.Collection.List;

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
        /// The associated Left Controller Haptic Process.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcess LeftControllerHapticProcess { get; set; }
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
        /// The associated Right Controller Haptic Process.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public HapticProcess RightControllerHapticProcess { get; set; }
        #endregion
    }
}