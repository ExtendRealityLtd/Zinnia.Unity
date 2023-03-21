namespace Zinnia.Visual
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Attempts to mutate the valid camera background settings and caches the previous settings for later restoration.
    /// </summary>
    public class CameraBackgroundMutator : MonoBehaviour
    {
        public struct CameraSettings
        {
            public CameraClearFlags clearFlags;
            public Color backgroundColor;
        }

        [Tooltip("The Camera collection to mutate the background of.")]
        [SerializeField]
        private UnityObjectObservableList targetCameras;
        /// <summary>
        /// The <see cref="Camera"/> collection to mutate the background of.
        /// </summary>
        public UnityObjectObservableList TargetCameras
        {
            get
            {
                return targetCameras;
            }
            set
            {
                targetCameras = value;
            }
        }
        [Tooltip("The camera clear flags to set the valid cameras to.")]
        [SerializeField]
        private CameraClearFlags targetClearFlags;
        /// <summary>
        /// The camera clear flags to set the valid cameras to.
        /// </summary>
        public CameraClearFlags TargetClearFlags
        {
            get
            {
                return targetClearFlags;
            }
            set
            {
                targetClearFlags = value;
            }
        }
        [Tooltip("The Color to mutate the background to.")]
        [SerializeField]
        private Color targetBackgroundColor = Color.black;
        /// <summary>
        /// The <see cref="Color"/> to mutate the background to.
        /// </summary>
        public Color TargetBackgroundColor
        {
            get
            {
                return targetBackgroundColor;
            }
            set
            {
                targetBackgroundColor = value;
            }
        }

        /// <summary>
        /// The existing cached <see cref="Camera"/> settings before it was changed.
        /// </summary>
        protected Dictionary<Camera, CameraSettings> cachedCameraSettings = new Dictionary<Camera, CameraSettings>();

        /// <summary>
        /// Mutates the <see cref="TargetCameras"/> collection with the target settings.
        /// </summary>
        public virtual void Mutate()
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (Object currentCameraObject in TargetCameras.NonSubscribableElements)
            {
                Camera currentCamera = (Camera)currentCameraObject;
                if (currentCamera == null)
                {
                    continue;
                }

                CameraSettings currentSettings = new CameraSettings()
                {
                    clearFlags = currentCamera.clearFlags,
                    backgroundColor = currentCamera.backgroundColor
                };

                cachedCameraSettings[currentCamera] = currentSettings;

                if (TargetClearFlags != 0)
                {
                    currentCamera.clearFlags = TargetClearFlags;
                }
                currentCamera.backgroundColor = TargetBackgroundColor;
            }
        }

        /// <summary>
        /// Restores the <see cref="TargetCameras"/> settings to the cached settings before the mutate occurred.
        /// </summary>
        public virtual void Restore()
        {
            if (!this.IsValidState())
            {
                return;
            }

            foreach (KeyValuePair<Camera, CameraSettings> currentCamera in cachedCameraSettings)
            {
                currentCamera.Key.clearFlags = currentCamera.Value.clearFlags;
                currentCamera.Key.backgroundColor = currentCamera.Value.backgroundColor;
            }

            cachedCameraSettings.Clear();
        }
    }
}