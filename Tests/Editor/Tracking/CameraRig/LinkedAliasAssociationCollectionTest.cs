using Zinnia.Data.Collection.List;
using Zinnia.Haptics.Collection;
using Zinnia.Tracking.CameraRig;

namespace Test.Zinnia.Tracking.CameraRig
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class LinkedAliasAssociationCollectionTest
    {
        private GameObject containingObject;
        private LinkedAliasAssociationCollection subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ContainingObject");
            subject = containingObject.AddComponent<LinkedAliasAssociationCollection>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearPlayArea()
        {
            Assert.IsNull(subject.PlayArea);
            subject.PlayArea = containingObject;
            Assert.AreEqual(containingObject, subject.PlayArea);
            subject.ClearPlayArea();
            Assert.IsNull(subject.PlayArea);
        }

        [Test]
        public void ClearPlayAreaInactiveGameObject()
        {
            Assert.IsNull(subject.PlayArea);
            subject.PlayArea = containingObject;
            Assert.AreEqual(containingObject, subject.PlayArea);
            subject.gameObject.SetActive(false);
            subject.ClearPlayArea();
            Assert.AreEqual(containingObject, subject.PlayArea);
        }

        [Test]
        public void ClearPlayAreaInactiveComponent()
        {
            Assert.IsNull(subject.PlayArea);
            subject.PlayArea = containingObject;
            Assert.AreEqual(containingObject, subject.PlayArea);
            subject.enabled = false;
            subject.ClearPlayArea();
            Assert.AreEqual(containingObject, subject.PlayArea);
        }

        [Test]
        public void ClearHeadset()
        {
            Assert.IsNull(subject.Headset);
            subject.Headset = containingObject;
            Assert.AreEqual(containingObject, subject.Headset);
            subject.ClearHeadset();
            Assert.IsNull(subject.Headset);
        }

        [Test]
        public void ClearHeadsetInactiveGameObject()
        {
            Assert.IsNull(subject.Headset);
            subject.Headset = containingObject;
            Assert.AreEqual(containingObject, subject.Headset);
            subject.gameObject.SetActive(false);
            subject.ClearHeadset();
            Assert.AreEqual(containingObject, subject.Headset);
        }

        [Test]
        public void ClearHeadsetInactiveComponent()
        {
            Assert.IsNull(subject.Headset);
            subject.Headset = containingObject;
            Assert.AreEqual(containingObject, subject.Headset);
            subject.enabled = false;
            subject.ClearHeadset();
            Assert.AreEqual(containingObject, subject.Headset);
        }

        [Test]
        public void ClearHeadsetCamera()
        {
            Camera testCamera = containingObject.AddComponent<Camera>();
            Assert.IsNull(subject.HeadsetCamera);
            subject.HeadsetCamera = testCamera;
            Assert.AreEqual(testCamera, subject.HeadsetCamera);
            subject.ClearHeadsetCamera();
            Assert.IsNull(subject.HeadsetCamera);
        }

        [Test]
        public void ClearHeadsetCameraInactiveGameObject()
        {
            Camera testCamera = containingObject.AddComponent<Camera>();
            Assert.IsNull(subject.HeadsetCamera);
            subject.HeadsetCamera = testCamera;
            Assert.AreEqual(testCamera, subject.HeadsetCamera);
            subject.gameObject.SetActive(false);
            subject.ClearHeadsetCamera();
            Assert.AreEqual(testCamera, subject.HeadsetCamera);
        }

        [Test]
        public void ClearHeadsetCameraInactiveComponent()
        {
            Camera testCamera = containingObject.AddComponent<Camera>();
            Assert.IsNull(subject.HeadsetCamera);
            subject.HeadsetCamera = testCamera;
            Assert.AreEqual(testCamera, subject.HeadsetCamera);
            subject.enabled = false;
            subject.ClearHeadsetCamera();
            Assert.AreEqual(testCamera, subject.HeadsetCamera);
        }

        [Test]
        public void ClearHeadsetVelocityTracker()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.HeadsetVelocityTracker);
            subject.HeadsetVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.HeadsetVelocityTracker);
            subject.ClearHeadsetVelocityTracker();
            Assert.IsNull(subject.HeadsetVelocityTracker);
        }

        [Test]
        public void ClearHeadsetVelocityTrackerInactiveGameObject()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.HeadsetVelocityTracker);
            subject.HeadsetVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.HeadsetVelocityTracker);
            subject.gameObject.SetActive(false);
            subject.ClearHeadsetVelocityTracker();
            Assert.AreEqual(tracker, subject.HeadsetVelocityTracker);
        }

        [Test]
        public void ClearHeadsetVelocityTrackerInactiveComponent()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.HeadsetVelocityTracker);
            subject.HeadsetVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.HeadsetVelocityTracker);
            subject.enabled = false;
            subject.ClearHeadsetVelocityTracker();
            Assert.AreEqual(tracker, subject.HeadsetVelocityTracker);
        }

        [Test]
        public void ClearSupplementHeadsetCameras()
        {
            CameraObservableList cameraList = containingObject.AddComponent<CameraObservableList>();
            Assert.IsNull(subject.SupplementHeadsetCameras);
            subject.SupplementHeadsetCameras = cameraList;
            Assert.AreEqual(cameraList, subject.SupplementHeadsetCameras);
            subject.ClearSupplementHeadsetCameras();
            Assert.IsNull(subject.SupplementHeadsetCameras);
        }

        [Test]
        public void ClearSupplementHeadsetCamerasInactiveGameObject()
        {
            CameraObservableList cameraList = containingObject.AddComponent<CameraObservableList>();
            Assert.IsNull(subject.SupplementHeadsetCameras);
            subject.SupplementHeadsetCameras = cameraList;
            Assert.AreEqual(cameraList, subject.SupplementHeadsetCameras);
            subject.gameObject.SetActive(false);
            subject.ClearSupplementHeadsetCameras();
            Assert.AreEqual(cameraList, subject.SupplementHeadsetCameras);
        }

        [Test]
        public void ClearSupplementHeadsetCamerasInactiveComponent()
        {
            CameraObservableList cameraList = containingObject.AddComponent<CameraObservableList>();
            Assert.IsNull(subject.SupplementHeadsetCameras);
            subject.SupplementHeadsetCameras = cameraList;
            Assert.AreEqual(cameraList, subject.SupplementHeadsetCameras);
            subject.enabled = false;
            subject.ClearSupplementHeadsetCameras();
            Assert.AreEqual(cameraList, subject.SupplementHeadsetCameras);
        }

        [Test]
        public void ClearHeadsetDeviceDetails()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.HeadsetDeviceDetails);
            subject.HeadsetDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.HeadsetDeviceDetails);
            subject.ClearHeadsetDeviceDetails();
            Assert.IsNull(subject.HeadsetDeviceDetails);
        }

        [Test]
        public void ClearHeadsetDeviceDetailsInactiveGameObject()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.HeadsetDeviceDetails);
            subject.HeadsetDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.HeadsetDeviceDetails);
            subject.gameObject.SetActive(false);
            subject.ClearHeadsetDeviceDetails();
            Assert.AreEqual(deviceMock, subject.HeadsetDeviceDetails);
        }

        [Test]
        public void ClearHeadsetDeviceDetailsInactiveComponent()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.HeadsetDeviceDetails);
            subject.HeadsetDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.HeadsetDeviceDetails);
            subject.enabled = false;
            subject.ClearHeadsetDeviceDetails();
            Assert.AreEqual(deviceMock, subject.HeadsetDeviceDetails);
        }

        [Test]
        public void ClearDominantController()
        {
            DominantControllerObserver observer = containingObject.AddComponent<DominantControllerObserver>();
            Assert.IsNull(subject.DominantController);
            subject.DominantController = observer;
            Assert.AreEqual(observer, subject.DominantController);
            subject.ClearDominantController();
            Assert.IsNull(subject.DominantController);
        }

        [Test]
        public void ClearDominantControllerInactiveGameObject()
        {
            DominantControllerObserver observer = containingObject.AddComponent<DominantControllerObserver>();
            Assert.IsNull(subject.DominantController);
            subject.DominantController = observer;
            Assert.AreEqual(observer, subject.DominantController);
            subject.gameObject.SetActive(false);
            subject.ClearDominantController();
            Assert.AreEqual(observer, subject.DominantController);
        }

        [Test]
        public void ClearDominantControllerInactiveComponent()
        {
            DominantControllerObserver observer = containingObject.AddComponent<DominantControllerObserver>();
            Assert.IsNull(subject.DominantController);
            subject.DominantController = observer;
            Assert.AreEqual(observer, subject.DominantController);
            subject.enabled = false;
            subject.ClearDominantController();
            Assert.AreEqual(observer, subject.DominantController);
        }

        [Test]
        public void ClearLeftController()
        {
            Assert.IsNull(subject.LeftController);
            subject.LeftController = containingObject;
            Assert.AreEqual(containingObject, subject.LeftController);
            subject.ClearLeftController();
            Assert.IsNull(subject.LeftController);
        }

        [Test]
        public void ClearLeftControllerInactiveGameObject()
        {
            Assert.IsNull(subject.LeftController);
            subject.LeftController = containingObject;
            Assert.AreEqual(containingObject, subject.LeftController);
            subject.gameObject.SetActive(false);
            subject.ClearLeftController();
            Assert.AreEqual(containingObject, subject.LeftController);
        }

        [Test]
        public void ClearLeftControllerInactiveComponent()
        {
            Assert.IsNull(subject.LeftController);
            subject.LeftController = containingObject;
            Assert.AreEqual(containingObject, subject.LeftController);
            subject.enabled = false;
            subject.ClearLeftController();
            Assert.AreEqual(containingObject, subject.LeftController);
        }

        [Test]
        public void ClearLeftControllerVelocityTracker()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.LeftControllerVelocityTracker);
            subject.LeftControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.LeftControllerVelocityTracker);
            subject.ClearLeftControllerVelocityTracker();
            Assert.IsNull(subject.LeftControllerVelocityTracker);
        }

        [Test]
        public void ClearLeftControllerVelocityTrackerInactiveGameObject()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.LeftControllerVelocityTracker);
            subject.LeftControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.LeftControllerVelocityTracker);
            subject.gameObject.SetActive(false);
            subject.ClearLeftControllerVelocityTracker();
            Assert.AreEqual(tracker, subject.LeftControllerVelocityTracker);
        }

        [Test]
        public void ClearLeftControllerVelocityTrackerInactiveComponent()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.LeftControllerVelocityTracker);
            subject.LeftControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.LeftControllerVelocityTracker);
            subject.enabled = false;
            subject.ClearLeftControllerVelocityTracker();
            Assert.AreEqual(tracker, subject.LeftControllerVelocityTracker);
        }

        [Test]
        public void ClearLeftControllerHapticProcess()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.LeftControllerHapticProcess);
            subject.LeftControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.LeftControllerHapticProcess);
            subject.ClearLeftControllerHapticProcess();
            Assert.IsNull(subject.LeftControllerHapticProcess);
        }

        [Test]
        public void ClearLeftControllerHapticProcessInactiveGameObject()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.LeftControllerHapticProcess);
            subject.LeftControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.LeftControllerHapticProcess);
            subject.gameObject.SetActive(false);
            subject.ClearLeftControllerHapticProcess();
            Assert.AreEqual(hapticProcess, subject.LeftControllerHapticProcess);
        }

        [Test]
        public void ClearLeftControllerHapticProcessInactiveComponent()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.LeftControllerHapticProcess);
            subject.LeftControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.LeftControllerHapticProcess);
            subject.enabled = false;
            subject.ClearLeftControllerHapticProcess();
            Assert.AreEqual(hapticProcess, subject.LeftControllerHapticProcess);
        }

        [Test]
        public void ClearLeftControllerHapticProfiles()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.LeftControllerHapticProfiles);
            subject.LeftControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.LeftControllerHapticProfiles);
            subject.ClearLeftControllerHapticProfiles();
            Assert.IsNull(subject.LeftControllerHapticProfiles);
        }

        [Test]
        public void ClearLeftControllerHapticProfilesInactiveGameObject()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.LeftControllerHapticProfiles);
            subject.LeftControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.LeftControllerHapticProfiles);
            subject.gameObject.SetActive(false);
            subject.ClearLeftControllerHapticProfiles();
            Assert.AreEqual(hapticProfiles, subject.LeftControllerHapticProfiles);
        }

        [Test]
        public void ClearLeftControllerHapticProfilesInactiveComponent()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.LeftControllerHapticProfiles);
            subject.LeftControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.LeftControllerHapticProfiles);
            subject.enabled = false;
            subject.ClearLeftControllerHapticProfiles();
            Assert.AreEqual(hapticProfiles, subject.LeftControllerHapticProfiles);
        }

        [Test]
        public void ClearLeftControllerDeviceDetails()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftControllerDeviceDetails);
            subject.LeftControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.LeftControllerDeviceDetails);
            subject.ClearLeftControllerDeviceDetails();
            Assert.IsNull(subject.LeftControllerDeviceDetails);
        }

        [Test]
        public void ClearLeftControllerDeviceDetailsInactiveGameObject()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftControllerDeviceDetails);
            subject.LeftControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.LeftControllerDeviceDetails);
            subject.gameObject.SetActive(false);
            subject.ClearLeftControllerDeviceDetails();
            Assert.AreEqual(deviceMock, subject.LeftControllerDeviceDetails);
        }

        [Test]
        public void ClearLeftControllerDeviceDetailsInactiveComponent()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftControllerDeviceDetails);
            subject.LeftControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.LeftControllerDeviceDetails);
            subject.enabled = false;
            subject.ClearLeftControllerDeviceDetails();
            Assert.AreEqual(deviceMock, subject.LeftControllerDeviceDetails);
        }

        [Test]
        public void ClearRightController()
        {
            Assert.IsNull(subject.RightController);
            subject.RightController = containingObject;
            Assert.AreEqual(containingObject, subject.RightController);
            subject.ClearRightController();
            Assert.IsNull(subject.RightController);
        }

        [Test]
        public void ClearRightControllerInactiveGameObject()
        {
            Assert.IsNull(subject.RightController);
            subject.RightController = containingObject;
            Assert.AreEqual(containingObject, subject.RightController);
            subject.gameObject.SetActive(false);
            subject.ClearRightController();
            Assert.AreEqual(containingObject, subject.RightController);
        }

        [Test]
        public void ClearRightControllerInactiveComponent()
        {
            Assert.IsNull(subject.RightController);
            subject.RightController = containingObject;
            Assert.AreEqual(containingObject, subject.RightController);
            subject.enabled = false;
            subject.ClearRightController();
            Assert.AreEqual(containingObject, subject.RightController);
        }

        [Test]
        public void ClearRightControllerVelocityTracker()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.RightControllerVelocityTracker);
            subject.RightControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.RightControllerVelocityTracker);
            subject.ClearRightControllerVelocityTracker();
            Assert.IsNull(subject.RightControllerVelocityTracker);
        }

        [Test]
        public void ClearRightControllerVelocityTrackerInactiveGameObject()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.RightControllerVelocityTracker);
            subject.RightControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.RightControllerVelocityTracker);
            subject.gameObject.SetActive(false);
            subject.ClearRightControllerVelocityTracker();
            Assert.AreEqual(tracker, subject.RightControllerVelocityTracker);
        }

        [Test]
        public void ClearRightControllerVelocityTrackerInactiveComponent()
        {
            VelocityTrackerMock tracker = containingObject.AddComponent<VelocityTrackerMock>();
            Assert.IsNull(subject.RightControllerVelocityTracker);
            subject.RightControllerVelocityTracker = tracker;
            Assert.AreEqual(tracker, subject.RightControllerVelocityTracker);
            subject.enabled = false;
            subject.ClearRightControllerVelocityTracker();
            Assert.AreEqual(tracker, subject.RightControllerVelocityTracker);
        }

        [Test]
        public void ClearRightControllerHapticProcess()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.RightControllerHapticProcess);
            subject.RightControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.RightControllerHapticProcess);
            subject.ClearRightControllerHapticProcess();
            Assert.IsNull(subject.RightControllerHapticProcess);
        }

        [Test]
        public void ClearRightControllerHapticProcessInactiveGameObject()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.RightControllerHapticProcess);
            subject.RightControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.RightControllerHapticProcess);
            subject.gameObject.SetActive(false);
            subject.ClearRightControllerHapticProcess();
            Assert.AreEqual(hapticProcess, subject.RightControllerHapticProcess);
        }

        [Test]
        public void ClearRightControllerHapticProcessInactiveComponent()
        {
            HapticProcessMock hapticProcess = containingObject.AddComponent<HapticProcessMock>();
            Assert.IsNull(subject.RightControllerHapticProcess);
            subject.RightControllerHapticProcess = hapticProcess;
            Assert.AreEqual(hapticProcess, subject.RightControllerHapticProcess);
            subject.enabled = false;
            subject.ClearRightControllerHapticProcess();
            Assert.AreEqual(hapticProcess, subject.RightControllerHapticProcess);
        }

        [Test]
        public void ClearRightControllerHapticProfiles()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.RightControllerHapticProfiles);
            subject.RightControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.RightControllerHapticProfiles);
            subject.ClearRightControllerHapticProfiles();
            Assert.IsNull(subject.RightControllerHapticProfiles);
        }

        [Test]
        public void ClearRightControllerHapticProfilesInactiveGameObject()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.RightControllerHapticProfiles);
            subject.RightControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.RightControllerHapticProfiles);
            subject.gameObject.SetActive(false);
            subject.ClearRightControllerHapticProfiles();
            Assert.AreEqual(hapticProfiles, subject.RightControllerHapticProfiles);
        }

        [Test]
        public void ClearRightControllerHapticProfilesInactiveComponent()
        {
            HapticProcessObservableList hapticProfiles = containingObject.AddComponent<HapticProcessObservableList>();
            Assert.IsNull(subject.RightControllerHapticProfiles);
            subject.RightControllerHapticProfiles = hapticProfiles;
            Assert.AreEqual(hapticProfiles, subject.RightControllerHapticProfiles);
            subject.enabled = false;
            subject.ClearRightControllerHapticProfiles();
            Assert.AreEqual(hapticProfiles, subject.RightControllerHapticProfiles);
        }

        [Test]
        public void ClearRightControllerDeviceDetails()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightControllerDeviceDetails);
            subject.RightControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.RightControllerDeviceDetails);
            subject.ClearRightControllerDeviceDetails();
            Assert.IsNull(subject.RightControllerDeviceDetails);
        }

        [Test]
        public void ClearRightControllerDeviceDetailsInactiveGameObject()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightControllerDeviceDetails);
            subject.RightControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.RightControllerDeviceDetails);
            subject.gameObject.SetActive(false);
            subject.ClearRightControllerDeviceDetails();
            Assert.AreEqual(deviceMock, subject.RightControllerDeviceDetails);
        }

        [Test]
        public void ClearRightControllerDeviceDetailsInactiveComponent()
        {
            DeviceDetailsRecordMock deviceMock = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightControllerDeviceDetails);
            subject.RightControllerDeviceDetails = deviceMock;
            Assert.AreEqual(deviceMock, subject.RightControllerDeviceDetails);
            subject.enabled = false;
            subject.ClearRightControllerDeviceDetails();
            Assert.AreEqual(deviceMock, subject.RightControllerDeviceDetails);
        }
    }
}