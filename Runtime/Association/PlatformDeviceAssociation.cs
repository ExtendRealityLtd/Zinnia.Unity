﻿namespace Zinnia.Association
{
    using System;
    using System.Text.RegularExpressions;
    using UnityEngine;
#if UNITY_XR || UNITY_VR
    using UnityEngine.XR;
#endif

    /// <summary>
    /// Holds <see cref="GameObject"/>s to (de)activate based on the current platform and loaded XR device type.
    /// </summary>
    [Obsolete("Use `RuleAssociation` instead.")]
    public class PlatformDeviceAssociation : GameObjectsAssociation
    {
        [Tooltip("A regular expression to match the name of the current RuntimePlatform.")]
        [SerializeField]
        private string platformPattern;
        /// <summary>
        /// A regular expression to match the name of the current <see cref="RuntimePlatform"/>.
        /// </summary>
        public string PlatformPattern
        {
            get
            {
                return platformPattern;
            }
            set
            {
                platformPattern = value;
            }
        }
        [Tooltip("A regular expression to match the name of the XR device that needs to be loaded.")]
        [SerializeField]
        private string xrSdkPattern;
        /// <summary>
        /// A regular expression to match the name of the XR device that needs to be loaded.
        /// </summary>
        public string XrSdkPattern
        {
            get
            {
                return xrSdkPattern;
            }
            set
            {
                xrSdkPattern = value;
            }
        }

        [Tooltip("A regular expression to match the name of the XR model that is being used.")]
        [SerializeField]
        private string xrModelPattern;
        /// <summary>
        /// A regular expression to match the name of the XR model that is being used.
        /// </summary>
        public string XrModelPattern
        {
            get
            {
                return xrModelPattern;
            }
            set
            {
                xrModelPattern = value;
            }
        }

        /// <inheritdoc/>
        public override bool ShouldBeActive()
        {
            string modelName = "";
#if UNITY_2020_2_OR_NEWER
#if UNITY_XR
            InputDevice currentDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            modelName = currentDevice != null && currentDevice.name != null ? currentDevice.name : "";
#endif
#else
#if UNITY_VR
            modelName = XRDevice.model;
#endif
#endif
            return Regex.IsMatch(Application.platform.ToString(), PlatformPattern) &&
#if UNITY_VR
                Regex.IsMatch(XRSettings.loadedDeviceName, XrSdkPattern) &&
#endif
                Regex.IsMatch(modelName, XrModelPattern);
        }
    }
}