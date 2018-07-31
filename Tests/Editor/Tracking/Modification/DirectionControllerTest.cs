using VRTK.Core.Tracking.Modification;

namespace Test.VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class DirectionControllerTest
    {
        private GameObject containingObject;
        private DirectionController subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<DirectionController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.target = target;
            subject.lookAt = lookAt;
            subject.pivot = pivot;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(new Quaternion(0f, 0.6f, 0.8f, 0f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoLookAt()
        {
            GameObject target = new GameObject();
            GameObject pivot = new GameObject();

            subject.target = target;
            subject.pivot = pivot;

            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoPivot()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();

            subject.target = target;
            subject.lookAt = lookAt;

            lookAt.transform.position = Vector3.up * 2f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.target = target;
            subject.lookAt = lookAt;
            subject.pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.target = target;
            subject.lookAt = lookAt;
            subject.pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ResetOrientation()
        {
            UnityEventListenerMock orientationResetMock = new UnityEventListenerMock();
            subject.OrientationReset.AddListener(orientationResetMock.Listen);

            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.target = target;
            subject.lookAt = lookAt;
            subject.pivot = pivot;
            subject.resetOrientationSpeed = 0f;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(new Quaternion(0f, 0.6f, 0.8f, 0f).ToString(), target.transform.rotation.ToString());

            subject.ResetOrientation();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);
            Assert.IsTrue(orientationResetMock.Received);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void CancelResetOrientation()
        {
            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subject.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);
            subject.CancelResetOrientation();
            Assert.IsTrue(orientationResetCancelledMock.Received);
        }
    }
}