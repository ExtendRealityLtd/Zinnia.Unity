namespace Zinnia.Association
{
    using UnityEngine;
    using UnityEngine.XR;
    using System.Text.RegularExpressions;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the loaded XR device's name.
    /// </summary>
    public class LoadedXrDeviceAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public string XrDeviceNamePattern { get; set; }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            return Regex.IsMatch(XRSettings.loadedDeviceName, XrDeviceNamePattern);
        }
    }
}