namespace Zinnia.Tracking.CameraRig
{
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
        [Header("PlayArea Settings")]
        [Tooltip("The associated PlayArea.")]
        [SerializeField]
        private GameObject _playArea;
        /// <summary>
        /// The associated PlayArea.
        /// </summary>
        public GameObject PlayArea
        {
            get
            {
                return _playArea;
            }
            set
            {
                _playArea = value;
            }
        }
        #endregion

        #region Headset Settings
        [Header("Headset Settings")]
        [Tooltip("The associated Headset.")]
        [SerializeField]
        private GameObject _headset;
        /// <summary>
        /// The associated Headset.
        /// </summary>
        public GameObject Headset
        {
            get
            {
                return _headset;
            }
            set
            {
                _headset = value;
            }
        }
        [Tooltip("The associated Headset Camera.")]
        [SerializeField]
        private Camera _headsetCamera;
        /// <summary>
        /// The associated Headset Camera.
        /// </summary>
        public Camera HeadsetCamera
        {
            get
            {
                return _headsetCamera;
            }
            set
            {
                _headsetCamera = value;
            }
        }
        [Tooltip("The associated Headset Velocity Tracker.")]
        [SerializeField]
        private VelocityTracker _headsetVelocityTracker;
        /// <summary>
        /// The associated Headset Velocity Tracker.
        /// </summary>
        public VelocityTracker HeadsetVelocityTracker
        {
            get
            {
                return _headsetVelocityTracker;
            }
            set
            {
                _headsetVelocityTracker = value;
            }
        }
        [Tooltip("A list of any additional cameras associated with the headset.")]
        [SerializeField]
        private CameraObservableList _supplementHeadsetCameras;
        /// <summary>
        /// A list of any additional cameras associated with the headset.
        /// </summary>
        public CameraObservableList SupplementHeadsetCameras
        {
            get
            {
                return _supplementHeadsetCameras;
            }
            set
            {
                _supplementHeadsetCameras = value;
            }
        }
        [Tooltip("The details and status of the headset device.")]
        [SerializeField]
        private DeviceDetailsRecord _headsetDeviceDetails;
        /// <summary>
        /// The details and status of the headset device.
        /// </summary>
        public DeviceDetailsRecord HeadsetDeviceDetails
        {
            get
            {
                return _headsetDeviceDetails;
            }
            set
            {
                _headsetDeviceDetails = value;
            }
        }
        [Tooltip("The dominant controller observer.")]
        [SerializeField]
        private DominantControllerObserver _dominantController;
        /// <summary>
        /// The dominant controller observer.
        /// </summary>
        public DominantControllerObserver DominantController
        {
            get
            {
                return _dominantController;
            }
            set
            {
                _dominantController = value;
            }
        }
        #endregion

        #region Left Controller Settings
        [Header("Left Controller Settings")]
        [Tooltip("The associated Left Controller.")]
        [SerializeField]
        private GameObject _leftController;
        /// <summary>
        /// The associated Left Controller.
        /// </summary>
        public GameObject LeftController
        {
            get
            {
                return _leftController;
            }
            set
            {
                _leftController = value;
            }
        }
        [Tooltip("The associated Left Controller Velocity Tracker.")]
        [SerializeField]
        private VelocityTracker _leftControllerVelocityTracker;
        /// <summary>
        /// The associated Left Controller Velocity Tracker.
        /// </summary>
        public VelocityTracker LeftControllerVelocityTracker
        {
            get
            {
                return _leftControllerVelocityTracker;
            }
            set
            {
                _leftControllerVelocityTracker = value;
            }
        }
        [Tooltip("The main Left Controller Haptic Process profile.")]
        [SerializeField]
        private HapticProcess _leftControllerHapticProcess;
        /// <summary>
        /// The main Left Controller Haptic Process profile.
        /// </summary>
        public HapticProcess LeftControllerHapticProcess
        {
            get
            {
                return _leftControllerHapticProcess;
            }
            set
            {
                _leftControllerHapticProcess = value;
            }
        }
        [Tooltip("A HapticProcess collection of haptic profiles that can be used with the Left Controller.")]
        [SerializeField]
        private HapticProcessObservableList _leftControllerHapticProfiles;
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of haptic profiles that can be used with the Left Controller.
        /// </summary>
        public HapticProcessObservableList LeftControllerHapticProfiles
        {
            get
            {
                return _leftControllerHapticProfiles;
            }
            set
            {
                _leftControllerHapticProfiles = value;
            }
        }
        [Tooltip("The details and status of the left controller device.")]
        [SerializeField]
        private DeviceDetailsRecord _leftControllerDeviceDetails;
        /// <summary>
        /// The details and status of the left controller device.
        /// </summary>
        public DeviceDetailsRecord LeftControllerDeviceDetails
        {
            get
            {
                return _leftControllerDeviceDetails;
            }
            set
            {
                _leftControllerDeviceDetails = value;
            }
        }
        #endregion

        #region Right Controller Settings
        [Header("Right Controller Settings")]
        [Tooltip("The associated Right Controller.")]
        [SerializeField]
        private GameObject _rightController;
        /// <summary>
        /// The associated Right Controller.
        /// </summary>
        public GameObject RightController
        {
            get
            {
                return _rightController;
            }
            set
            {
                _rightController = value;
            }
        }
        [Tooltip("The associated Right Controller Velocity Tracker.")]
        [SerializeField]
        private VelocityTracker _rightControllerVelocityTracker;
        /// <summary>
        /// The associated Right Controller Velocity Tracker.
        /// </summary>
        public VelocityTracker RightControllerVelocityTracker
        {
            get
            {
                return _rightControllerVelocityTracker;
            }
            set
            {
                _rightControllerVelocityTracker = value;
            }
        }
        [Tooltip("The main Right Controller Haptic Process profile.")]
        [SerializeField]
        private HapticProcess _rightControllerHapticProcess;
        /// <summary>
        /// The main Right Controller Haptic Process profile.
        /// </summary>
        public HapticProcess RightControllerHapticProcess
        {
            get
            {
                return _rightControllerHapticProcess;
            }
            set
            {
                _rightControllerHapticProcess = value;
            }
        }
        [Tooltip("A HapticProcess collection of supplement haptic settings that can be used with the Right Controller.")]
        [SerializeField]
        private HapticProcessObservableList _rightControllerHapticProfiles;
        /// <summary>
        /// A <see cref="HapticProcess"/> collection of supplement haptic settings that can be used with the Right Controller.
        /// </summary>
        public HapticProcessObservableList RightControllerHapticProfiles
        {
            get
            {
                return _rightControllerHapticProfiles;
            }
            set
            {
                _rightControllerHapticProfiles = value;
            }
        }
        [Tooltip("The details and status of the right controller device.")]
        [SerializeField]
        private DeviceDetailsRecord _rightControllerDeviceDetails;
        /// <summary>
        /// The details and status of the right controller device.
        /// </summary>
        public DeviceDetailsRecord RightControllerDeviceDetails
        {
            get
            {
                return _rightControllerDeviceDetails;
            }
            set
            {
                _rightControllerDeviceDetails = value;
            }
        }
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