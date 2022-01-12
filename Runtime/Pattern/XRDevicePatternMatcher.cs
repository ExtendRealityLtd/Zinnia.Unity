namespace Zinnia.Pattern
{
    using Malimbe.MemberChangeMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
#if UNITY_2020_2_OR_NEWER
    using System.Collections.Generic;
    using UnityEngine;
#endif
    using UnityEngine.XR;
    using Zinnia.Extension;

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
            /// The device presence state.
            /// </summary>
            IsPresent,
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

        /// <summary>
        /// The source property to match against.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Source PropertySource { get; set; }

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
                case Source.IsPresent:
                    bool isPresent = false;
#if UNITY_2020_2_OR_NEWER
                    List<XRDisplaySubsystem> xrDisplaySubsystems = new List<XRDisplaySubsystem>();
                    SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
                    foreach (var xrDisplay in xrDisplaySubsystems)
                    {
                        if (xrDisplay.running)
                        {
                            isPresent = true;
                        }
                    }
#else
                    isPresent = XRDevice.isPresent;
#endif
                    return isPresent.ToString();
                case Source.Model:
                    string modelName = "";
#if UNITY_2020_2_OR_NEWER
                    InputDevice modelDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                    modelName = modelDevice != null && modelDevice.name != null ? modelDevice.name : "";
#else
                    modelName = XRDevice.model;
#endif
                    return modelName;
                case Source.RefreshRate:
                    return XRDevice.refreshRate.ToString();
                case Source.UserPresence:
                    int userPresence = -1;
#if UNITY_2020_2_OR_NEWER
                    InputDevice userPresenceDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                    if (userPresenceDevice.isValid)
                    {
                        userPresenceDevice.TryGetFeatureValue(CommonUsages.userPresence, out bool userPresent);
                        userPresence = userPresent ? 1 : 0;
                    }
#else
                    userPresence = (int)XRDevice.userPresence;
#endif
                    return userPresence.ToString();
            }

            return null;
        }

        /// <summary>
        /// Called after <see cref="PropertySource"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(PropertySource))]
        protected virtual void OnAfterPropertySourceChange()
        {
            ProcessSourceString();
        }
    }
}