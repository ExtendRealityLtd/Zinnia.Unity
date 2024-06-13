﻿namespace Zinnia.Association
{
    using System;
    using System.Text.RegularExpressions;
    using UnityEngine;
#if UNITY_XR || UNITY_VR
    using UnityEngine.XR;
#endif

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the loaded XR device's name.
    /// </summary>
    [Obsolete("Use `PlatformDeviceAssociation` instead.")]
    public class LoadedXrDeviceAssociation : GameObjectsAssociation
    {
        [Tooltip("A regular expression to match the name of the XR device that needs to be loaded.")]
        [SerializeField]
        private string xrDeviceNamePattern;
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        public string XrDeviceNamePattern
        {
            get
            {
                return xrDeviceNamePattern;
            }
            set
            {
                xrDeviceNamePattern = value;
            }
        }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
#if UNITY_VR
            return Regex.IsMatch(XRSettings.loadedDeviceName, XrDeviceNamePattern);
#else
            return false;
#endif
        }
    }
}