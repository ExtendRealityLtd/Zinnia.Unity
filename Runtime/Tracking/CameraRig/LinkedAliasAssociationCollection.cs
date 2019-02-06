namespace Zinnia.Tracking.CameraRig
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
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
        [Header("PlayArea Settings"), DocumentedByXml]
        public GameObject playArea;
        #endregion

        #region Headset Settings
        /// <summary>
        /// The associated Headset.
        /// </summary>
        [Header("Headset Settings"), DocumentedByXml]
        public GameObject headset;
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        [DocumentedByXml]
        public Camera headsetCamera;
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        [DocumentedByXml]
        public VelocityTracker headsetVelocityTracker;
        #endregion

        #region Left Controller Settings
        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        [Header("Left Controller Settings"), DocumentedByXml]
        public GameObject leftController;
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        [DocumentedByXml]
        public VelocityTracker leftControllerVelocityTracker;
        #endregion

        #region Right Controller Settings
        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        [Header("Right Controller Settings"), DocumentedByXml]
        public GameObject rightController;
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        [DocumentedByXml]
        public VelocityTracker rightControllerVelocityTracker;
        #endregion
    }
}