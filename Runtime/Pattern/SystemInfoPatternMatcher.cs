namespace Zinnia.Pattern
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Matches the name of the current <see cref="SystemInfo.deviceModel"/>.
    /// </summary>
    public class SystemInfoPatternMatcher : PatternMatcher
    {
        /// <summary>
        /// The property source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// The current battery level.
            /// </summary>
            BatteryLevel,
            /// <summary>
            /// The current battery status.
            /// </summary>
            BatteryStatus,
            /// <summary>
            /// The current device model.
            /// </summary>
            DeviceModel,
            /// <summary>
            /// The current device name.
            /// </summary>
            DeviceName,
            /// <summary>
            /// The current device type.
            /// </summary>
            DeviceType,
            /// <summary>
            /// The current device unique identifier.
            /// </summary>
            DeviceUniqueIdentifier,
            /// <summary>
            /// The current id of graphics device.
            /// </summary>
            GraphicsDeviceID,
            /// <summary>
            /// The current name of graphics device.
            /// </summary>
            GraphicsDeviceName,
            /// <summary>
            /// The current type of graphics device.
            /// </summary>
            GraphicsDeviceType,
            /// <summary>
            /// The current vendor of graphics device.
            /// </summary>
            GraphicsDeviceVendor,
            /// <summary>
            /// The current vendor id of graphics device.
            /// </summary>
            GraphicsDeviceVendorID,
            /// <summary>
            /// The current version of graphics device.
            /// </summary>
            GraphicsDeviceVersion,
            /// <summary>
            /// The current amount of graphics memory.
            /// </summary>
            GraphicsMemorySize,
            /// <summary>
            /// Whether the graphics device multi-threaded.
            /// </summary>
            GraphicsMultiThreaded,
            /// <summary>
            /// The current shader level of the graphics device.
            /// </summary>
            GraphicsShaderLevel,
            /// <summary>
            /// The maximum cubemap texture size.
            /// </summary>
            MaxCubemapSize,
            /// <summary>
            /// The maximum texture size.
            /// </summary>
            MaxTextureSize,
            /// <summary>
            /// The current device operating system and version.
            /// </summary>
            OperatingSystem,
            /// <summary>
            /// The current device operating system family.
            /// </summary>
            OperatingSystemFamily,
            /// <summary>
            /// The current device processor count.
            /// </summary>
            ProcessorCount,
            /// <summary>
            /// The current device processor frequency.
            /// </summary>
            ProcessorFrequency,
            /// <summary>
            /// The current device processor type.
            /// </summary>
            ProcessorType,
            /// <summary>
            /// Whether the current device has an accelerometer.
            /// </summary>
            SupportsAccelerometer,
            /// <summary>
            /// Whether the current device has an audio playback device.
            /// </summary>
            SupportsAudio,
            /// <summary>
            /// Whether the current device has a gyroscope.
            /// </summary>
            SupportsGyroscope,
            /// <summary>
            /// Whether the current graphics device supports draw call instancing.
            /// </summary>
            SupportsInstancing,
            /// <summary>
            /// Whether the current device can report its location.
            /// </summary>
            SupportsLocationService,
            /// <summary>
            /// Whether the current device supports haptic feedback.
            /// </summary>
            SupportsVibration,
            /// <summary>
            /// The current amount of device memory.
            /// </summary>
            SystemMemorySize
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
                case Source.BatteryLevel:
                    return SystemInfo.batteryLevel.ToString();
                case Source.BatteryStatus:
                    return SystemInfo.batteryStatus.ToString();
                case Source.DeviceModel:
                    return SystemInfo.deviceModel;
                case Source.DeviceName:
                    return SystemInfo.deviceName;
                case Source.DeviceType:
                    return SystemInfo.deviceType.ToString();
                case Source.DeviceUniqueIdentifier:
                    return SystemInfo.deviceUniqueIdentifier;
                case Source.GraphicsDeviceID:
                    return SystemInfo.graphicsDeviceID.ToString();
                case Source.GraphicsDeviceName:
                    return SystemInfo.graphicsDeviceName;
                case Source.GraphicsDeviceType:
                    return SystemInfo.graphicsDeviceType.ToString();
                case Source.GraphicsDeviceVendor:
                    return SystemInfo.graphicsDeviceVendor;
                case Source.GraphicsDeviceVendorID:
                    return SystemInfo.graphicsDeviceVendorID.ToString();
                case Source.GraphicsDeviceVersion:
                    return SystemInfo.graphicsDeviceVersion;
                case Source.GraphicsMemorySize:
                    return SystemInfo.graphicsMemorySize.ToString();
                case Source.GraphicsMultiThreaded:
                    return SystemInfo.graphicsMultiThreaded.ToString();
                case Source.GraphicsShaderLevel:
                    return SystemInfo.graphicsShaderLevel.ToString();
                case Source.MaxCubemapSize:
                    return SystemInfo.maxCubemapSize.ToString();
                case Source.MaxTextureSize:
                    return SystemInfo.maxTextureSize.ToString();
                case Source.OperatingSystem:
                    return SystemInfo.operatingSystem;
                case Source.OperatingSystemFamily:
                    return SystemInfo.operatingSystemFamily.ToString();
                case Source.ProcessorCount:
                    return SystemInfo.processorCount.ToString();
                case Source.ProcessorFrequency:
                    return SystemInfo.processorFrequency.ToString();
                case Source.ProcessorType:
                    return SystemInfo.processorType;
                case Source.SupportsAccelerometer:
                    return SystemInfo.supportsAccelerometer.ToString();
                case Source.SupportsAudio:
                    return SystemInfo.supportsAudio.ToString();
                case Source.SupportsGyroscope:
                    return SystemInfo.supportsGyroscope.ToString();
                case Source.SupportsInstancing:
                    return SystemInfo.supportsInstancing.ToString();
                case Source.SupportsLocationService:
                    return SystemInfo.supportsLocationService.ToString();
                case Source.SupportsVibration:
                    return SystemInfo.supportsVibration.ToString();
                case Source.SystemMemorySize:
                    return SystemInfo.systemMemorySize.ToString();
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