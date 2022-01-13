namespace Zinnia.Utility
{
#if UNITY_2019_3_OR_NEWER
    using System.Collections.Generic;
    using UnityEngine;
#endif
    using UnityEngine.XR;

    /// <summary>
    /// Wrapper class for <see cref="XRDevice"/> properties that have become obsolete in later versions of Unity.
    /// </summary>
    public static class XRDeviceProperties
    {
        /// <summary>
        /// Determines whether the device is present.
        /// </summary>
        /// <returns>Whether the device is present.</returns>
        public static bool IsPresent()
        {
            bool isPresent = false;
#if UNITY_2019_3_OR_NEWER
            List<XRDisplaySubsystem> xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
            foreach (XRDisplaySubsystem xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    isPresent = true;
                }
            }
#else
            isPresent = XRDevice.isPresent;
#endif
            return isPresent;
        }

        /// <summary>
        /// The manufacturer name of the given node.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The manufacturer name.</returns>
        public static string Manufacturer(XRNode node = XRNode.Head)
        {
            string manufacturerName = "";
#if UNITY_2019_3_OR_NEWER
            InputDevice manufacturerDevice = InputDevices.GetDeviceAtXRNode(node);
            manufacturerName = manufacturerDevice != null && manufacturerDevice.manufacturer != null ? manufacturerDevice.manufacturer : "";
#endif
            return manufacturerName;
        }

        /// <summary>
        /// The model name of the given node.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The model name.</returns>
        public static string Model(XRNode node = XRNode.Head)
        {
            string modelName = "";
#if UNITY_2019_3_OR_NEWER
            InputDevice modelDevice = InputDevices.GetDeviceAtXRNode(node);
            modelName = modelDevice != null && modelDevice.name != null ? modelDevice.name : "";
#else
            modelName = XRDevice.model;
#endif
            return modelName;
        }

        /// <summary>
        /// Determines whether the device is being tracked.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>Whether the device is being tracked.</returns>
        public static bool IsTracked(XRNode node = XRNode.Head)
        {
            bool isTracked = false;
#if UNITY_2019_3_OR_NEWER
            InputDevice trackedDevice = InputDevices.GetDeviceAtXRNode(node);
            if (trackedDevice != null)
            {
                trackedDevice.TryGetFeatureValue(CommonUsages.isTracked, out isTracked);
            }
#endif
            return isTracked;
        }

        /// <summary>
        /// The user presence state for the given node.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The user presence state.</returns>
        public static string UserPresence(XRNode node = XRNode.Head)
        {
            string userPresence = "Unknown";
#if UNITY_2019_3_OR_NEWER
            InputDevice userPresenceDevice = InputDevices.GetDeviceAtXRNode(node);
            if (userPresenceDevice != null)
            {
                if (userPresenceDevice.TryGetFeatureValue(CommonUsages.userPresence, out bool userPresent))
                {
                    userPresence = userPresent ? "Present" : "NotPresent";
                }
                else
                {
                    userPresence = "Unsupported";
                }
            }
#else
            userPresence = XRDevice.userPresence.ToString();
#endif
            return userPresence;
        }

        /// <summary>
        /// The current battery level of the device at the given node.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The current battery level.</returns>
        public static float BatteryLevel(XRNode node = XRNode.Head)
        {
            float batteryLevel = -1;
#if UNITY_2019_3_OR_NEWER
            InputDevice batteryLevelDevice = InputDevices.GetDeviceAtXRNode(node);
            if (batteryLevelDevice != null)
            {
                batteryLevelDevice.TryGetFeatureValue(CommonUsages.batteryLevel, out batteryLevel);
            }
#endif
            return batteryLevel;
        }
    }
}