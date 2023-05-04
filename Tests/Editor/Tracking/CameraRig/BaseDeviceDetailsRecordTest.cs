using Zinnia.Tracking.CameraRig;

namespace Test.Zinnia.Tracking.CameraRig
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.XR;

    public class BaseDeviceDetailsRecordTest
    {
        private GameObject containingObject;
        private MockBaseDeviceDetailsRecord subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("BaseDeviceDetailsRecordTest");
            subject = containingObject.AddComponent<MockBaseDeviceDetailsRecord>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void PassThroughCameraEnabled()
        {
            UnityEventListenerMock passthruEnabledMock = new UnityEventListenerMock();
            UnityEventListenerMock passthruDisabledMock = new UnityEventListenerMock();
            subject.PassThroughCameraWasEnabled.AddListener(passthruEnabledMock.Listen);
            subject.PassThroughCameraWasDisabled.AddListener(passthruDisabledMock.Listen);

            subject.hasPassThroughCamera = false;
            subject.PassThroughCameraEnabled = true;
            Assert.IsFalse(subject.PassThroughCameraEnabled);
            Assert.IsFalse(passthruEnabledMock.Received);
            Assert.IsFalse(passthruDisabledMock.Received);

            passthruEnabledMock.Reset();
            passthruDisabledMock.Reset();

            subject.hasPassThroughCamera = true;
            subject.PassThroughCameraEnabled = true;

            Assert.IsTrue(subject.PassThroughCameraEnabled);
            Assert.IsTrue(passthruEnabledMock.Received);
            Assert.IsFalse(passthruDisabledMock.Received);

            passthruEnabledMock.Reset();
            passthruDisabledMock.Reset();

            subject.PassThroughCameraEnabled = false;
            Assert.IsFalse(subject.PassThroughCameraEnabled);
            Assert.IsFalse(passthruEnabledMock.Received);
            Assert.IsTrue(passthruDisabledMock.Received);
        }

        [Test]
        public void TrackingBegun()
        {
            UnityEventListenerMock trackingBegunMock = new UnityEventListenerMock();
            subject.TrackingBegun.AddListener(trackingBegunMock.Listen);

            subject.isConnected = false;
            subject.Process();
            Assert.IsFalse(trackingBegunMock.Received);
            Assert.IsFalse(subject.TrackingHasBegun);

            trackingBegunMock.Reset();
            subject.isConnected = true;
            subject.Process();

            Assert.IsTrue(trackingBegunMock.Received);
            Assert.IsTrue(subject.TrackingHasBegun);

            trackingBegunMock.Reset();
            subject.isConnected = false;
            subject.Process();
            Assert.IsFalse(trackingBegunMock.Received);
            Assert.IsTrue(subject.TrackingHasBegun);
        }

        [Test]
        public void ConnectionStatusChanged()
        {
            UnityEventListenerMock connectionStatusChanged = new UnityEventListenerMock();
            subject.ConnectionStatusChanged.AddListener(connectionStatusChanged.Listen);

            subject.isConnected = false;
            subject.Process();
            Assert.IsTrue(connectionStatusChanged.Received);
            Assert.IsFalse(subject.IsConnected);

            connectionStatusChanged.Reset();
            subject.Process();
            Assert.IsFalse(connectionStatusChanged.Received);
            Assert.IsFalse(subject.IsConnected);

            connectionStatusChanged.Reset();
            subject.isConnected = true;
            subject.Process();
            Assert.IsTrue(connectionStatusChanged.Received);
            Assert.IsTrue(subject.IsConnected);

            connectionStatusChanged.Reset();
            subject.Process();
            Assert.IsFalse(connectionStatusChanged.Received);
            Assert.IsTrue(subject.IsConnected);

            connectionStatusChanged.Reset();
            subject.isConnected = false;
            subject.Process();
            Assert.IsTrue(connectionStatusChanged.Received);
            Assert.IsFalse(subject.IsConnected);
        }

        [Test]
        public void BatteryChargeStatusChanged()
        {
            UnityEventListenerMock batteryChargeStatusChanged = new UnityEventListenerMock();
            subject.BatteryChargeStatusChanged.AddListener(batteryChargeStatusChanged.Listen);

            subject.batteryChargeStatus = BatteryStatus.Unknown;
            subject.Process();
            Assert.IsFalse(batteryChargeStatusChanged.Received);
            Assert.AreEqual(BatteryStatus.Unknown, subject.BatteryChargeStatus);

            batteryChargeStatusChanged.Reset();
            subject.Process();
            Assert.IsFalse(batteryChargeStatusChanged.Received);
            Assert.AreEqual(BatteryStatus.Unknown, subject.BatteryChargeStatus);

            batteryChargeStatusChanged.Reset();
            subject.batteryChargeStatus = BatteryStatus.Discharging;
            subject.Process();
            Assert.IsTrue(batteryChargeStatusChanged.Received);
            Assert.AreEqual(BatteryStatus.Discharging, subject.BatteryChargeStatus);

            batteryChargeStatusChanged.Reset();
            subject.Process();
            Assert.IsFalse(batteryChargeStatusChanged.Received);
            Assert.AreEqual(BatteryStatus.Discharging, subject.BatteryChargeStatus);

            batteryChargeStatusChanged.Reset();
            subject.batteryChargeStatus = BatteryStatus.Charging;
            subject.Process();
            Assert.IsTrue(batteryChargeStatusChanged.Received);
            Assert.AreEqual(BatteryStatus.Charging, subject.BatteryChargeStatus);
        }

        [Test]
        public void TrackingTypeChanged()
        {
            UnityEventListenerMock trackingTypeChanged = new UnityEventListenerMock();
            subject.TrackingTypeChanged.AddListener(trackingTypeChanged.Listen);

            subject.trackingType = DeviceDetailsRecord.SpatialTrackingType.Unknown;
            subject.Process();
            Assert.IsFalse(trackingTypeChanged.Received);
            Assert.AreEqual(DeviceDetailsRecord.SpatialTrackingType.Unknown, subject.TrackingType);

            trackingTypeChanged.Reset();
            subject.Process();
            Assert.IsFalse(trackingTypeChanged.Received);
            Assert.AreEqual(DeviceDetailsRecord.SpatialTrackingType.Unknown, subject.TrackingType);

            trackingTypeChanged.Reset();
            subject.trackingType = DeviceDetailsRecord.SpatialTrackingType.RotationAndPosition;
            subject.Process();
            Assert.IsTrue(trackingTypeChanged.Received);
            Assert.AreEqual(DeviceDetailsRecord.SpatialTrackingType.RotationAndPosition, subject.TrackingType);

            trackingTypeChanged.Reset();
            subject.Process();
            Assert.IsFalse(trackingTypeChanged.Received);
            Assert.AreEqual(DeviceDetailsRecord.SpatialTrackingType.RotationAndPosition, subject.TrackingType);

            trackingTypeChanged.Reset();
            subject.trackingType = DeviceDetailsRecord.SpatialTrackingType.RotationOnly;
            subject.Process();
            Assert.IsTrue(trackingTypeChanged.Received);
            Assert.AreEqual(DeviceDetailsRecord.SpatialTrackingType.RotationOnly, subject.TrackingType);
        }
    }

    public class MockBaseDeviceDetailsRecord : BaseDeviceDetailsRecord
    {
        public XRNode xrNodeType;
        public int priority;
        public bool hasPassThroughCamera;
        public bool isConnected;
        public string manufacturer;
        public string model;
        public SpatialTrackingType trackingType;
        public float batteryLevel;
        public BatteryStatus batteryChargeStatus;

        public override XRNode XRNodeType { get => xrNodeType; protected set => throw new System.NotImplementedException(); }
        public override int Priority { get => priority; protected set => throw new System.NotImplementedException(); }
        public override bool HasPassThroughCamera { get => hasPassThroughCamera; protected set => throw new System.NotImplementedException(); }
        public override bool IsConnected { get => isConnected; protected set => throw new System.NotImplementedException(); }
        public override string Manufacturer { get => manufacturer; protected set => throw new System.NotImplementedException(); }
        public override string Model { get => model; protected set => throw new System.NotImplementedException(); }
        public override SpatialTrackingType TrackingType { get => trackingType; protected set => throw new System.NotImplementedException(); }
        public override float BatteryLevel { get => batteryLevel; protected set => throw new System.NotImplementedException(); }
        public override BatteryStatus BatteryChargeStatus { get => batteryChargeStatus; protected set => throw new System.NotImplementedException(); }
    }
}