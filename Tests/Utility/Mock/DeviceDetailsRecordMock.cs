using UnityEngine;
using UnityEngine.XR;
using Zinnia.Tracking.CameraRig;

namespace Test.Zinnia.Utility.Mock
{
    [AddComponentMenu("")]
    public class DeviceDetailsRecordMock : DeviceDetailsRecord
    {
        public XRNode nodeType;
        public bool connectedState;
        public int priority;

        public override XRNode XRNodeType
        {
            get
            {
                return nodeType;
            }
            protected set
            {
                nodeType = value;
            }
        }

        public override bool IsConnected
        {
            get
            {
                return connectedState;
            }
            protected set
            {
                connectedState = value;
            }
        }

        public override int Priority
        {
            get
            {
                return priority;
            }
            protected set
            {
                priority = value;
            }
        }

        public override string Manufacturer { get; protected set; }
        public override string Model { get; protected set; }
        public override SpatialTrackingType TrackingType { get; protected set; }
        public override float BatteryLevel { get; protected set; }
        public override BatteryStatus BatteryChargeStatus { get; protected set; }

        protected override bool HasBatteryChargeStatusChanged()
        {
            throw new System.NotImplementedException();
        }

        protected override bool HasIsConnectedChanged()
        {
            throw new System.NotImplementedException();
        }

        protected override bool HasTrackingTypeChanged()
        {
            throw new System.NotImplementedException();
        }
    }
}