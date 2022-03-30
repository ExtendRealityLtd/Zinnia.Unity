using Zinnia.Tracking.CameraRig;

namespace Test.Zinnia.Tracking.CameraRig
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.XR;

    public class DominantControllerObserverTest
    {
        private GameObject containingObject;
        private DominantControllerObserver subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ContainingObject");
            subject = containingObject.AddComponent<DominantControllerObserver>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearLeftController()
        {
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftController);
            subject.LeftController = leftController;
            Assert.AreEqual(leftController, subject.LeftController);
            subject.ClearLeftController();
            Assert.IsNull(subject.LeftController);
        }

        [Test]
        public void ClearLeftControllerInactiveGameObject()
        {
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftController);
            subject.LeftController = leftController;
            Assert.AreEqual(leftController, subject.LeftController);
            subject.gameObject.SetActive(false);
            subject.ClearLeftController();
            Assert.AreEqual(leftController, subject.LeftController);
        }

        [Test]
        public void ClearLeftControllerInactiveComponent()
        {
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.LeftController);
            subject.LeftController = leftController;
            Assert.AreEqual(leftController, subject.LeftController);
            subject.enabled = false;
            subject.ClearLeftController();
            Assert.AreEqual(leftController, subject.LeftController);
        }

        [Test]
        public void ClearRightController()
        {
            DeviceDetailsRecordMock RightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightController);
            subject.RightController = RightController;
            Assert.AreEqual(RightController, subject.RightController);
            subject.ClearRightController();
            Assert.IsNull(subject.RightController);
        }

        [Test]
        public void ClearRightControllerInactiveGameObject()
        {
            DeviceDetailsRecordMock RightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightController);
            subject.RightController = RightController;
            Assert.AreEqual(RightController, subject.RightController);
            subject.gameObject.SetActive(false);
            subject.ClearRightController();
            Assert.AreEqual(RightController, subject.RightController);
        }

        [Test]
        public void ClearRightControllerInactiveComponent()
        {
            DeviceDetailsRecordMock RightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            Assert.IsNull(subject.RightController);
            subject.RightController = RightController;
            Assert.AreEqual(RightController, subject.RightController);
            subject.enabled = false;
            subject.ClearRightController();
            Assert.AreEqual(RightController, subject.RightController);
        }

        [Test]
        public void DominantControllerChanged()
        {
            UnityEventListenerMock isChangingMock = new UnityEventListenerMock();
            subject.IsChanging.AddListener(isChangingMock.Listen);

            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.connectedState = true;
            rightController.priority = 0;

            leftController.connectedState = true;
            leftController.priority = 1;

            Assert.IsFalse(isChangingMock.Received);

            subject.Process();

            Assert.IsTrue(isChangingMock.Received);

            isChangingMock.Reset();

            subject.Process();

            Assert.IsFalse(isChangingMock.Received);

            rightController.priority = 1;
            leftController.priority = 0;

            subject.Process();

            Assert.IsTrue(isChangingMock.Received);

            isChangingMock.Reset();

            subject.Process();

            Assert.IsFalse(isChangingMock.Received);

            rightController.connectedState = false;

            subject.Process();

            Assert.IsFalse(isChangingMock.Received);

            rightController.connectedState = true;
            leftController.connectedState = false;

            subject.Process();

            Assert.IsTrue(isChangingMock.Received);

            isChangingMock.Reset();

            subject.Process();

            Assert.IsFalse(isChangingMock.Received);

            rightController.connectedState = false;

            subject.Process();

            Assert.IsTrue(isChangingMock.Received);
        }

        [Test]
        public void DominantControllerNode()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.nodeType = XRNode.RightHand;
            rightController.connectedState = true;
            rightController.priority = 0;

            leftController.nodeType = XRNode.LeftHand;
            leftController.connectedState = true;
            leftController.priority = 1;

            Assert.AreEqual(XRNode.RightHand, subject.DominantController);

            rightController.priority = 1;
            leftController.priority = 0;

            Assert.AreEqual(XRNode.LeftHand, subject.DominantController);

            rightController.priority = 0;
            leftController.priority = 0;

            Assert.AreEqual(XRNode.RightHand, subject.DominantController);

            rightController.connectedState = false;
            leftController.connectedState = false;

            Assert.AreEqual(XRNode.Head, subject.DominantController);
        }

        [Test]
        public void DominantControllerDetailsBothSetAndConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.connectedState = true;
            rightController.priority = 0;

            leftController.connectedState = true;
            leftController.priority = 1;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);

            rightController.priority = 1;
            leftController.priority = 0;

            Assert.AreEqual(leftController, subject.DominantControllerDetails);

            rightController.priority = 0;
            leftController.priority = 0;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsBothSetOnlyRightConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.connectedState = true;
            rightController.priority = 0;

            leftController.connectedState = false;
            leftController.priority = 1;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);

            rightController.priority = 1;
            leftController.priority = 0;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);

            rightController.priority = 0;
            leftController.priority = 0;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsBothSetOnlyLeftConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.connectedState = false;
            rightController.priority = 0;

            leftController.connectedState = true;
            leftController.priority = 1;

            Assert.AreEqual(leftController, subject.DominantControllerDetails);

            rightController.priority = 1;
            leftController.priority = 0;

            Assert.AreEqual(leftController, subject.DominantControllerDetails);

            rightController.priority = 0;
            leftController.priority = 0;

            Assert.AreEqual(leftController, subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsBothSetNoneConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;
            subject.LeftController = leftController;

            rightController.connectedState = false;
            rightController.priority = 0;

            leftController.connectedState = false;
            leftController.priority = 1;

            Assert.IsNull(subject.DominantControllerDetails);

            rightController.priority = 1;
            leftController.priority = 0;

            Assert.IsNull(subject.DominantControllerDetails);

            rightController.priority = 0;
            leftController.priority = 0;

            Assert.IsNull(subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsOnlyRightSetAndConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;

            rightController.connectedState = true;
            rightController.priority = 0;

            Assert.AreEqual(rightController, subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsOnlyRightSetAndNotConnected()
        {
            DeviceDetailsRecordMock rightController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.RightController = rightController;

            rightController.connectedState = false;
            rightController.priority = 0;

            Assert.IsNull(subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsOnlyLeftSetAndConnected()
        {
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.LeftController = leftController;

            leftController.connectedState = true;
            leftController.priority = 0;

            Assert.AreEqual(leftController, subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsOnlyLeftSetAndNotConnected()
        {
            DeviceDetailsRecordMock leftController = containingObject.AddComponent<DeviceDetailsRecordMock>();

            subject.LeftController = leftController;

            leftController.connectedState = false;
            leftController.priority = 0;

            Assert.IsNull(subject.DominantControllerDetails);
        }

        [Test]
        public void DominantControllerDetailsNoneSetAndNotConnected()
        {
            Assert.IsNull(subject.DominantControllerDetails);
        }
    }
}