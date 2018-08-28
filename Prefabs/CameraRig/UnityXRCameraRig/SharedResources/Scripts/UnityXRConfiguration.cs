namespace VRTK.Core.Prefabs.CameraRig.UnityXRCameraRig
{
    using UnityEngine;
    using UnityEngine.XR;

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

        /// <summary>
        /// Represents the type of physical space available for XR.
        /// </summary>
        [Tooltip("Represents the type of physical space available for XR."), SerializeField]
        protected TrackingSpaceType trackingSpaceType = TrackingSpaceType.RoomScale;

        /// <summary>
        /// Automatically set the Unity Physics Fixed Timestep value based on the headset render frequency.
        /// </summary>
        [Tooltip("Automatically set the Unity Physics Fixed Timestep value based on the headset render frequency."), SerializeField]
        protected bool lockPhysicsUpdateRateToRenderFrequency = true;

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