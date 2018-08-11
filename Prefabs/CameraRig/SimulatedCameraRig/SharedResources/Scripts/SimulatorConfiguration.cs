namespace VRTK.Core.Prefabs.CameraRig.SimulatedCameraRig
{
    using UnityEngine;
    using UnityEngine.XR;
    using VRTK.Core.Extension;
    using VRTK.Core.Prefabs.CameraRig.TrackedAlias;
    using VRTK.Core.Mutation.TransformProperty;

    /// <summary>
    /// Provides configuration for the Simulated CameraRig.
    /// </summary>
    public class SimulatorConfiguration : MonoBehaviour
    {
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

        [Header("Simulator Settings")]

        [SerializeField]
        [Tooltip("Determines whether to disable the XRSettings.")]
        protected bool disableXRSettings = true;
        [SerializeField]
        [Tooltip("The frame rate to simulate with fixedDeltaTime.")]
        protected float simulatedFrameRate = 90f;
        /// <summary>
        /// The optional Tracked Alias prefab, must be provided if one is used in the scene.
        /// </summary>
        [Tooltip("The optional Tracked Alias prefab, must be provided if one is used in the scene.")]
        public TrackedAliasFacade trackedAlias;

        [Header("Internal Settings")]

        /// <summary>
        /// **DO NOT CHANGE** - The linked PositionProperty.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked PositionProperty.")]
        public PositionProperty playareaPosition;
        /// <summary>
        /// **DO NOT CHANGE** - The linked TransformPropertyResetter.
        /// </summary>
        [Tooltip("**DO NOT CHANGE** - The linked TransformPropertyResetter.")]
        public TransformPropertyResetter playareaResetter;

        protected bool originalXRSettings;
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

        protected virtual void UpdateSimulatedFrameRate(float rate)
        {
            Time.fixedDeltaTime = Time.timeScale / rate;
        }
    }
}