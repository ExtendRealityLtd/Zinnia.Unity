namespace Zinnia.Tracking.CameraRig
{
    using UnityEngine;
    using UnityEngine.XR;
    using Zinnia.Utility;

    /// <summary>
    /// A base implementation of the <see cref="DeviceDetailsRecord"/> that utilises the <see cref="XRDeviceProperties"/> to be extended upon further.
    /// </summary>
    public abstract class BaseDeviceDetailsRecord : DeviceDetailsRecord
    {
        /// <inheritdoc/>
        public override bool IsConnected { get => XRDeviceProperties.IsTracked(XRNodeType); protected set => throw new System.NotImplementedException(); }
        /// <inheritdoc/>
        public override string Manufacturer { get => XRDeviceProperties.Manufacturer(XRNodeType); protected set => throw new System.NotImplementedException(); }
        /// <inheritdoc/>
        public override string Model { get => GetModelString(); protected set => throw new System.NotImplementedException(); }
        /// <inheritdoc/>
        public override SpatialTrackingType TrackingType { get => GetTrackingType(); protected set => throw new System.NotImplementedException(); }
        /// <inheritdoc/>
        public override float BatteryLevel { get => GetBatteryLevel(); protected set => throw new System.NotImplementedException(); }
        /// <inheritdoc/>
        public override BatteryStatus BatteryChargeStatus { get => XRNodeType == XRNode.Head ? SystemInfo.batteryStatus : BatteryStatus.Unknown; protected set => throw new System.NotImplementedException(); }

        /// <summary>
        /// The last known battery charge status.
        /// </summary>
        protected BatteryStatus lastKnownBatteryStatus;
        /// <summary>
        /// The last known is connected status.
        /// </summary>
        protected bool lastKnownIsConnected;
        /// <summary>
        /// The last known tracking type.
        /// </summary>
        protected SpatialTrackingType lastKnownTrackingType;

        /// <inheritdoc/>
        protected override bool HasBatteryChargeStatusChanged()
        {
            bool hasChanged = BatteryChargeStatus != lastKnownBatteryStatus;
            if (hasChanged)
            {
                BatteryChargeStatusChanged?.Invoke(BatteryChargeStatus);
            }
            lastKnownBatteryStatus = BatteryChargeStatus;
            return hasChanged;
        }

        /// <inheritdoc/>
        protected override bool HasIsConnectedChanged()
        {
            bool hasChanged = IsConnected != lastKnownIsConnected;
            if (hasChanged)
            {
                ConnectionStatusChanged?.Invoke(IsConnected);
            }
            lastKnownIsConnected = IsConnected;
            return hasChanged;
        }

        /// <inheritdoc/>
        protected override bool HasTrackingTypeChanged()
        {
            bool hasChanged = TrackingType != lastKnownTrackingType;
            if (hasChanged)
            {
                TrackingTypeChanged?.Invoke(TrackingType);
            }
            lastKnownTrackingType = TrackingType;
            return hasChanged;
        }

        /// <summary>
        /// Gets the device model from the appropriate Unity library.
        /// </summary>
        /// <returns>The connected node device model.</returns>
        protected virtual string GetModelString()
        {
            if (XRNodeType == XRNode.Head && !SystemInfo.deviceModel.ToLower().Contains("system product name"))
            {
                return SystemInfo.deviceModel;
            }

            return XRDeviceProperties.Model(XRNodeType);
        }

        /// <summary>
        /// Gets the spatial tracking type.
        /// </summary>
        /// <returns>The tracking type for the node.</returns>
        protected virtual SpatialTrackingType GetTrackingType()
        {
            if (XRDeviceProperties.HasPositionalTracking(XRNodeType) && XRDeviceProperties.HasRotationalTracking(XRNodeType))
            {
                return SpatialTrackingType.RotationAndPosition;
            }
            else if (XRDeviceProperties.HasRotationalTracking(XRNodeType))
            {
                return SpatialTrackingType.RotationOnly;
            }

            return SpatialTrackingType.None;
        }

        /// <summary>
        /// Gets the battery level of the device.
        /// </summary>
        /// <returns>The device battery level.</returns>
        protected virtual float GetBatteryLevel()
        {
            if (XRNodeType == XRNode.Head)
            {
                return SystemInfo.batteryLevel;
            }

            return XRDeviceProperties.BatteryLevel(XRNodeType);
        }
    }
}