namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig
{
    using UnityEngine;
    using UnityEngine.XR;
    using VRTK.Core.Extension;
    using VRTK.Core.Prefabs.CameraRig.TrackedAlias;
    using VRTK.Core.Mutation.TransformProperty;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// Provides configuration for the Simulated CameraRig.
    /// </summary>
    public class SimulatorConfiguration : MonoBehaviour
    {
        #region Simulator Settings
        /// <summary>
        /// The optional Tracked Alias prefab, must be provided if one is used in the scene.
        /// </summary>
        [Header("Simulator Settings"), Tooltip("The optional Tracked Alias prefab, must be provided if one is used in the scene.")]
        public TrackedAliasFacade trackedAlias;
        /// <summary>
        /// Determines whether to disable the XRSettings.
        /// </summary>
        [Tooltip("Determines whether to disable the XRSettings."), SerializeField]
        protected bool disableXRSettings = true;
        /// <summary>
        /// The frame rate to simulate with fixedDeltaTime.
        /// </summary>
        [Tooltip("The frame rate to simulate with fixedDeltaTime."), SerializeField]
        protected float simulatedFrameRate = 90f;
        #endregion

        #region Internal Settings
        /// <summary>
        /// The linked PositionProperty.
        /// </summary>
        [Header("Internal Settings"), Tooltip("The linked PositionProperty."), InternalSetting]
        public PositionProperty playareaPosition;
        /// <summary>
        /// The linked TransformPropertyResetter.
        /// </summary>
        [Tooltip("The linked TransformPropertyResetter."), InternalSetting]
        public TransformPropertyResetter playareaResetter;
        #endregion

        /// <summary>
        /// Determines whether to disable the XRSettings.
        /// </summary>
        public bool DisableXRSettings
        {
            get
            {
                return disableXRSettings;
            }
            set
            {
                disableXRSettings = value;
                UpdateXRSettings(disableXRSettings);
            }
        }

        /// <summary>
        /// The frame rate to simulate with fixedDeltaTime.
        /// </summary>
        public float SimulatedFrameRate
        {
            get
            {
                return simulatedFrameRate;
            }
            set
            {
                simulatedFrameRate = value;
                UpdateSimulatedFrameRate(simulatedFrameRate);
            }
        }

        /// <summary>
        /// The original configuration of XRSettings.
        /// </summary>
        protected bool originalXRSettings;
        /// <summary>
        /// The original configuration of FixedDeltaTime.
        /// </summary>
        protected float originalFixedDeltaTime;

        protected virtual void OnEnable()
        {
            originalXRSettings = XRSettings.enabled;
            originalFixedDeltaTime = Time.fixedDeltaTime;

            if (trackedAlias != null)
            {
                if (playareaPosition != null)
                {
                    playareaPosition.target = trackedAlias.internalSetup?.playArea?.sourceComponent?.gameObject;
                }

                if (playareaResetter != null)
                {
                    playareaResetter.source = trackedAlias.internalSetup?.playArea?.sourceComponent?.TryGetTransform();
                }
            }
        }

        protected virtual void OnDisable()
        {
            UpdateXRSettings(false);
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }

        protected virtual void OnValidate()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            UpdateXRSettings(disableXRSettings);
            UpdateSimulatedFrameRate(simulatedFrameRate);
        }

        /// <summary>
        /// Updates the XRSettings.
        /// </summary>
        /// <param name="state">The new value for the setting.</param>
        protected virtual void UpdateXRSettings(bool state)
        {
            if (state)
            {
                originalXRSettings = XRSettings.enabled;
                XRSettings.enabled = false;
            }
            else
            {
                XRSettings.enabled = originalXRSettings;
            }
        }

        /// <summary>
        /// Updates the simulated frame rate.
        /// </summary>
        /// <param name="rate">The new frame rate.</param>
        protected virtual void UpdateSimulatedFrameRate(float rate)
        {
            Time.fixedDeltaTime = Time.timeScale / rate;
        }
    }
}