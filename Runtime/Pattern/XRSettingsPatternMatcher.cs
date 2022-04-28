namespace Zinnia.Pattern
{
    using UnityEngine;
    using UnityEngine.XR;
    using Zinnia.Extension;

    /// <summary>
    /// Matches the name of the selected <see cref="XRSettings"/> property.
    /// </summary>
    public class XRSettingsPatternMatcher : PatternMatcher
    {
        /// <summary>
        /// The property source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// The device eye texture demension.
            /// </summary>
            DeviceEyeTextureDimension,
            /// <summary>
            /// The eye texture height.
            /// </summary>
            EyeTextureHeight,
            /// <summary>
            /// The eye texture resolution scale.
            /// </summary>
            EyeTextureResolutionScale,
            /// <summary>
            /// The eye texture resolution width.
            /// </summary>
            EyeTextureWidth,
            /// <summary>
            /// The device active state.
            /// </summary>
            IsDeviceActive,
            /// <summary>
            /// The loaded device name.
            /// </summary>
            LoadedDeviceName,
            /// <summary>
            /// The stereo rendering mode.
            /// </summary>
            StereoRenderingMode
        }

        [Tooltip("The source property to match against.")]
        [SerializeField]
        private Source propertySource;
        /// <summary>
        /// The source property to match against.
        /// </summary>
        public Source PropertySource
        {
            get
            {
                return propertySource;
            }
            set
            {
                propertySource = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterPropertySourceChange();
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="PropertySource"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Source"/>.</param>
        public virtual void SetPropertySource(int index)
        {
            PropertySource = EnumExtensions.GetByIndex<Source>(index);
        }

        /// <inheritdoc/>
        protected override string DefineSourceString()
        {
            switch (PropertySource)
            {
                case Source.DeviceEyeTextureDimension:
                    return XRSettings.deviceEyeTextureDimension.ToString();
                case Source.EyeTextureHeight:
                    return XRSettings.eyeTextureHeight.ToString();
                case Source.EyeTextureResolutionScale:
                    return XRSettings.eyeTextureResolutionScale.ToString();
                case Source.EyeTextureWidth:
                    return XRSettings.eyeTextureWidth.ToString();
                case Source.IsDeviceActive:
                    return XRSettings.isDeviceActive.ToString();
                case Source.LoadedDeviceName:
                    return XRSettings.loadedDeviceName;
                case Source.StereoRenderingMode:
                    return XRSettings.stereoRenderingMode.ToString();
            }

            return null;
        }

        /// <summary>
        /// Called after <see cref="PropertySource"/> has been changed.
        /// </summary>
        protected virtual void OnAfterPropertySourceChange()
        {
            ProcessSourceString();
        }
    }
}