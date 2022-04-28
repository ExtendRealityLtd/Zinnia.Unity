namespace Zinnia.Pattern
{
    using UnityEngine;
    using UnityEngine.XR;
    using Zinnia.Extension;
    using Zinnia.Utility;

    /// <summary>
    /// Matches the name of the selected <see cref="XRDevice"/> property.
    /// </summary>
    /// <remarks>
    /// `InputDevices.GetDeviceAtXRNode(XRNode.Head)` is used in Unity 2020.2 and above instead of <see cref="XRDevice.model"/>.
    /// </remarks>
    public class XRDevicePatternMatcher : PatternMatcher
    {
        /// <summary>
        /// The property source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// The device battery level.
            /// </summary>
            BatteryLevel,
            /// <summary>
            /// The number of devices found at a given node.
            /// </summary>
            DeviceCount,
            /// <summary>
            /// The device positional tracking state.
            /// </summary>
            HasPositionalTracking,
            /// <summary>
            /// The device rotational tracking state.
            /// </summary>
            HasRotationalTracking,
            /// <summary>
            /// The device presence state.
            /// </summary>
            IsPresent,
            /// <summary>
            /// The device tracked state.
            /// </summary>
            IsTracked,
            /// <summary>
            /// The device validity.
            /// </summary>
            IsValid,
            /// <summary>
            /// The device manufacturer.
            /// </summary>
            Manufacturer,
            /// <summary>
            /// The device model.
            /// </summary>
            Model,
            /// <summary>
            /// The device refresh rate.
            /// </summary>
            RefreshRate,
            /// <summary>
            /// The user presence state.
            /// </summary>
            UserPresence
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
#if UNITY_2019_3_OR_NEWER
        [Tooltip("The source node to consider as the device to check.")]
        [SerializeField]
        private XRNode deviceSource = XRNode.Head;
        /// <summary>
        /// The source node to consider as the device to check.
        /// </summary>
        public XRNode DeviceSource
        {
            get
            {
                return deviceSource;
            }
            set
            {
                deviceSource = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterDeviceSourceChange();
                }
            }
        }
#else
        protected XRNode DeviceSource { get; set; } = XRNode.Head;
#endif

        /// <summary>
        /// Sets the <see cref="PropertySource"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Source"/>.</param>
        public virtual void SetPropertySource(int index)
        {
            PropertySource = EnumExtensions.GetByIndex<Source>(index);
        }

        /// <summary>
        /// Sets the <see cref="DeviceSource"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="XRNode"/>.</param>
        public virtual void SetDeviceSource(int index)
        {
#if UNITY_2019_3_OR_NEWER
            DeviceSource = EnumExtensions.GetByIndex<XRNode>(index);
#else
            DeviceSource = XRNode.Head;
#endif
        }

        /// <inheritdoc/>
        protected override string DefineSourceString()
        {
            switch (PropertySource)
            {
                case Source.BatteryLevel:
                    return XRDeviceProperties.BatteryLevel(DeviceSource).ToString();
                case Source.DeviceCount:
                    return XRDeviceProperties.DeviceCount(DeviceSource).ToString();
                case Source.HasPositionalTracking:
                    return XRDeviceProperties.HasPositionalTracking().ToString();
                case Source.HasRotationalTracking:
                    return XRDeviceProperties.HasRotationalTracking().ToString();
                case Source.IsPresent:
                    return XRDeviceProperties.IsPresent().ToString();
                case Source.IsTracked:
                    return XRDeviceProperties.IsTracked(DeviceSource).ToString();
                case Source.IsValid:
                    return XRDeviceProperties.IsValid(DeviceSource).ToString();
                case Source.Manufacturer:
                    return XRDeviceProperties.Manufacturer(DeviceSource);
                case Source.Model:
                    return XRDeviceProperties.Model(DeviceSource);
                case Source.RefreshRate:
                    return XRDevice.refreshRate.ToString();
                case Source.UserPresence:
                    return XRDeviceProperties.UserPresence(DeviceSource);
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

        /// <summary>
        /// Called after <see cref="DeviceSource"/> has been changed.
        /// </summary>
        protected virtual void OnAfterDeviceSourceChange()
        {
            ProcessSourceString();
        }
    }
}