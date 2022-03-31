namespace Zinnia.Tracking.CameraRig
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.XR;
    using Zinnia.Process;

    /// <summary>
    /// Provides the basis for describing a CameraRig Alias device details and status.
    /// </summary>
    public abstract class DeviceDetailsRecord : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="bool"/>.
        /// </summary>
        [Serializable]
        public class BoolUnityEvent : UnityEvent<bool> { }
        /// <summary>
        /// Defines the event with the <see cref="SpatialTrackingType"/>.
        /// </summary>
        [Serializable]
        public class SpatialTrackingTypeUnityEvent : UnityEvent<SpatialTrackingType> { }
        /// <summary>
        /// Defines the event with the <see cref="BatteryStatus"/>.
        /// </summary>
        [Serializable]
        public class BatteryStatusUnityEvent : UnityEvent<BatteryStatus> { }

        /// <summary>
        /// The types of spatial tracking available.
        /// </summary>
        public enum SpatialTrackingType
        {
            /// <summary>
            /// An unknown tracking type.
            /// </summary>
            Unknown,
            /// <summary>
            /// 0 degrees of freedom, does not track rotation or position of a static device.
            /// </summary>
            None,
            /// <summary>
            /// 3 degrees of freedom, only rotational tracking of yaw, pitch and roll rotations.
            /// </summary>
            RotationOnly,
            /// <summary>
            /// 6 degrees of freedom, including rotational tracking as well as positional tracking of horizontal, vertical and depth movement.
            /// </summary>
            RotationAndPosition
        }

        /// <summary>
        /// The type of XR node the device is representing.
        /// </summary>
        public abstract XRNode XRNodeType { get; protected set; }
        /// <summary>
        /// Whether the device is currently connected.
        /// </summary>
        public abstract bool IsConnected { get; protected set; }
        /// <summary>
        /// The priority this device has over any similar devices.
        /// </summary>
        public abstract int Priority { get; protected set; }
        /// <summary>
        /// The manufacturer name of the device.
        /// </summary>
        public abstract string Manufacturer { get; protected set; }
        /// <summary>
        /// The model name of the device.
        /// </summary>
        public abstract string Model { get; protected set; }
        /// <summary>
        /// The spatial tracking type that the device is tracking with.
        /// </summary>
        public abstract SpatialTrackingType TrackingType { get; protected set; }
        /// <summary>
        /// The current level of charge in the battery in percentage.
        /// </summary>
        public abstract float BatteryLevel { get; protected set; }
        /// <summary>
        /// The current battery charge state.
        /// </summary>
        public abstract BatteryStatus BatteryChargeStatus { get; protected set; }

        /// <summary>
        /// Whether tracking for this device has begun.
        /// </summary>
        public bool TrackingHasBegun { get; protected set; }

        /// <summary>
        /// Emitted when the device begins tracking.
        /// </summary>
        public UnityEvent TrackingBegun = new UnityEvent();
        /// <summary>
        /// Emitted whenever the connection status changes.
        /// </summary>
        public BoolUnityEvent ConnectionStatusChanged = new BoolUnityEvent();
        /// <summary>
        /// Emitted whenever the tracking type changes.
        /// </summary>
        public SpatialTrackingTypeUnityEvent TrackingTypeChanged = new SpatialTrackingTypeUnityEvent();
        /// <summary>
        /// Emitted whenever the tracking type changes.
        /// </summary>
        public BatteryStatusUnityEvent BatteryChargeStatusChanged = new BatteryStatusUnityEvent();

        /// <summary>
        /// Checks to see if the statues have changed.
        /// </summary>
        public virtual void Process()
        {
            HasTrackingBegun();
            HasIsConnectedChanged();
            HasTrackingTypeChanged();
            HasBatteryChargeStatusChanged();
        }

        /// <summary>
        /// Checks to see if the <see cref="BatteryChargeStatus"/> has changed.
        /// </summary>
        /// <returns>Whether the status has changed or not.</returns>
        protected abstract bool HasBatteryChargeStatusChanged();
        /// <summary>
        /// Checks to see if the <see cref="IsConnected"/> has changed.
        /// </summary>
        /// <returns>Whether the status has changed or not.</returns>
        protected abstract bool HasIsConnectedChanged();
        /// <summary>
        /// Checks to see if the <see cref="TrackingType"/> has changed.
        /// </summary>
        /// <returns>Whether the status has changed or not.</returns>
        protected abstract bool HasTrackingTypeChanged();

        protected virtual void OnEnable()
        {
            TrackingHasBegun = false;
        }

        /// <summary>
        /// Determines whether tracking for this device has begun.
        /// </summary>
        /// <returns>Whether tracking for this device has begun.</returns>
        protected virtual bool HasTrackingBegun()
        {
            if (!TrackingHasBegun && IsConnected)
            {
                TrackingBegun?.Invoke();
                TrackingHasBegun = true;
                return true;
            }

            return false;
        }
    }
}