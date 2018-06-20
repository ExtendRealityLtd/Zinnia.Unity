namespace VRTK.Core.Prefabs.UnityXRCameraRig
{
    using UnityEngine;
    using UnityEngine.XR;
    using VRTK.Core.Extension;

    /// <summary>
    /// Provides configuration for the Unity Engine in XR.
    /// </summary>
    public class UnityXRConfiguration : MonoBehaviour
    {
        /// <summary>
        /// Represents the type of physical space available for XR.
        /// </summary>
        public TrackingSpaceType TrackingSpaceType
        {
            get
            {
                return trackingSpaceType;
            }
            set
            {
                trackingSpaceType = value;
                UpdateTrackingSpaceType();
            }
        }

        /// <summary>
        /// Automatically set the Unity Physics Fixed Timestep value based on the headset render frequency.
        /// </summary>
        public bool LockPhysicsUpdateRateToRenderFrequency
        {
            get
            {
                return lockPhysicsUpdateRateToRenderFrequency;
            }
            set
            {
                lockPhysicsUpdateRateToRenderFrequency = value;
                UpdateFixedDeltaTime();
            }
        }

        [SerializeField]
        [Tooltip("Represents the type of physical space available for XR.")]
        private TrackingSpaceType trackingSpaceType = TrackingSpaceType.RoomScale;

        [SerializeField]
        [Tooltip("Automatically set the Unity Physics Fixed Timestep value based on the headset render frequency.")]
        private bool lockPhysicsUpdateRateToRenderFrequency = true;

        protected virtual void OnEnable()
        {
            UpdateTrackingSpaceType();
        }

        protected virtual void OnValidate()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            UpdateTrackingSpaceType();
            UpdateFixedDeltaTime();
        }

        protected virtual void Update()
        {
            UpdateFixedDeltaTime();
        }

        /// <summary>
        /// Updates the tracking space type.
        /// </summary>
        protected virtual void UpdateTrackingSpaceType()
        {
            XRDevice.SetTrackingSpaceType(TrackingSpaceType);
        }

        /// <summary>
        /// Updates the fixed delta time to the appropriate value.
        /// </summary>
        protected virtual void UpdateFixedDeltaTime()
        {
            if (LockPhysicsUpdateRateToRenderFrequency && Time.timeScale > 0.0f)
            {
                Time.fixedDeltaTime = Time.timeScale / XRDevice.refreshRate;
            }
        }
    }
}