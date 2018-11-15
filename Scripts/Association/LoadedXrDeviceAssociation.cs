namespace VRTK.Core.Association
{
    using UnityEngine;
    using UnityEngine.XR;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the loaded XR device's name.
    /// </summary>
    public class LoadedXrDeviceAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        [Tooltip("A regular expression to match the name of the XR device that needs to be loaded.")]
        public string xrDeviceNamePattern;

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            return Regex.IsMatch(XRSettings.loadedDeviceName, xrDeviceNamePattern);
        }
    }
}