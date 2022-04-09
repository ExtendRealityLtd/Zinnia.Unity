namespace Zinnia.Utility
{
    using System.Collections.Generic;
#if UNITY_2019_3_OR_NEWER
    using UnityEngine;
#endif
    using UnityEngine.XR;

    /// <summary>
    /// Wrapper class for <see cref="XRDevice"/> properties that have become obsolete in later versions of Unity.
    /// </summary>
    public static class XRDeviceProperties
    {
        /// <summary>
        /// The number of devices found with the same node identifier.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The number of devices found.</returns>
        public static int DeviceCount(XRNode node = XRNode.Head)
        {
            int deviceCount = 0;
#if UNITY_2019_3_OR_NEWER
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(node, devices);
            deviceCount = devices.Count;
#else
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);
            foreach (XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == node)
                {
                    deviceCount++;
                }
            }
#endif
            return deviceCount;
        }

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
        /// Determines whether the device has positional tracking.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>Whether the device has positional tracking.</returns>
        public static bool HasPositionalTracking(XRNode node = XRNode.Head)
        {
            bool hasPositionalTracking = false;
#if UNITY_2019_3_OR_NEWER
            InputDevice positionalTrackingDevice = InputDevices.GetDeviceAtXRNode(node);
            if (positionalTrackingDevice != null)
            {
                positionalTrackingDevice.TryGetFeatureValue(CommonUsages.trackingState, out InputTrackingState trackingState);
                hasPositionalTracking = (trackingState & InputTrackingState.Position) != 0;
            }
#else
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);
            foreach (XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == node)
                {
                    hasPositionalTracking = nodeState.TryGetPosition(out _);
                    break;
                }
            }
#endif
            return hasPositionalTracking;
        }

        /// <summary>
        /// Determines whether the device has rotational tracking.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>Whether the device has rotational tracking.</returns>
        public static bool HasRotationalTracking(XRNode node = XRNode.Head)
        {
            bool hasRotationalTracking = false;
#if UNITY_2019_3_OR_NEWER
            InputDevice rotationalTrackingDevice = InputDevices.GetDeviceAtXRNode(node);
            if (rotationalTrackingDevice != null)
            {
                rotationalTrackingDevice.TryGetFeatureValue(CommonUsages.trackingState, out InputTrackingState trackingState);
                hasRotationalTracking = (trackingState & InputTrackingState.Rotation) != 0;
            }
#else
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);
            foreach (XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == node)
                {
                    hasRotationalTracking = nodeState.TryGetRotation(out _);
                    break;
                }
            }
#endif
            return hasRotationalTracking;
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
#else
            List<XRNodeState> nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);
            foreach (XRNodeState nodeState in nodeStates)
            {
                if (nodeState.nodeType == node)
                {
                    isTracked = nodeState.tracked;
                    break;
                }
            }
#endif
            return isTracked;
        }

        /// <summary>
        /// Determines whether the device is valid.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>Whether the device is valid.</returns>
        public static bool IsValid(XRNode node = XRNode.Head)
        {
            bool isValid = false;
#if UNITY_2019_3_OR_NEWER
            InputDevice validDevice = InputDevices.GetDeviceAtXRNode(node);
            isValid = validDevice != null && validDevice.isValid;
#endif
            return isValid;
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
            float batteryLevel = -1f;
#if UNITY_2019_3_OR_NEWER
            InputDevice batteryLevelDevice = InputDevices.GetDeviceAtXRNode(node);
            if (batteryLevelDevice != null)
            {
                batteryLevelDevice.TryGetFeatureValue(CommonUsages.batteryLevel, out batteryLevel);
            }
#endif
            return batteryLevel;
        }

#if UNITY_2019_3_OR_NEWER
        /// <summary>
        /// Returns the first device found at the given node.
        /// </summary>
        /// <param name="node">The node to check for.</param>
        /// <returns>The first found device.</returns>
        public static InputDevice DeviceInstance(XRNode node = XRNode.Head)
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(node, devices);

            if (devices.Count == 0)
            {
                return default;
            }

            if (devices.Count > 1)
            {
                Debug.Log("Multiple devices found at node: " + node);
            }

            return devices[0];
        }

        /// <summary>
        /// Determines whether the given device is the same as a default unset device.
        /// </summary>
        /// <param name="device">The device to check for.</param>
        /// <returns>Whether the device is the same as a default unset device.</returns>
        public static bool IsDeviceDefault(InputDevice device)
        {
            return device == default;
        }
#endif
    }
}