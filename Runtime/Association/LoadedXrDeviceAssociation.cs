namespace Zinnia.Association
{
    using System;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.XR;

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the loaded XR device's name.
    /// </summary>
    [Obsolete("Use `PlatformDeviceAssociation` instead.")]
    public class LoadedXrDeviceAssociation : GameObjectsAssociation
    {
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        [Tooltip("A regular expression to match the name of the XR device that needs to be loaded.")]
        [SerializeField]
        private string _xrDeviceNamePattern;
        public string XrDeviceNamePattern
        {
            get
            {
                return _xrDeviceNamePattern;
            }
            set
            {
                _xrDeviceNamePattern = value;
            }
        }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            return Regex.IsMatch(XRSettings.loadedDeviceName, XrDeviceNamePattern);
        }
    }
}