namespace Zinnia.Association
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.XR;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the current platform and loaded XR device type.
    /// </summary>
    public class PlatformDeviceAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// A regular expression to match the name of the current <see cref="RuntimePlatform"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string PlatformPattern { get; set; }
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string XrSdkPattern { get; set; }

        /// <summary>
        /// A regular expression to match the name of the XR model that is being used.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string XrModelPattern { get; set; }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            string modelName = "";
#if UNITY_2020_2_OR_NEWER
            InputDevice currentDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            modelName = currentDevice != null && currentDevice.name != null ? currentDevice.name : "";
#else
            modelName = XRDevice.model;
#endif
            return Regex.IsMatch(Application.platform.ToString(), PlatformPattern) &&
                Regex.IsMatch(XRSettings.loadedDeviceName, XrSdkPattern) &&
                Regex.IsMatch(modelName, XrModelPattern);

        }
    }
}