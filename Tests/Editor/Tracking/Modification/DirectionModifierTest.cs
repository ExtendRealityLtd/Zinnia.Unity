using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class DirectionModifierTest
    {
        private GameObject containingObject;
        private DirectionModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<DirectionModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessWithoutLookAtZRotation()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.PreventLookAtZRotation = false;

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
        public void ProcessWithLookAtZRotation()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.PreventLookAtZRotation = true;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(new Quaternion(-0.6f, 0f, 0f, 0.8f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoLookAt()
        {
            GameObject target = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.Pivot = pivot;

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

            subject.Target = target;
            subject.LookAt = lookAt;

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

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
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

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
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

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.ResetOrientationSpeed = 0f;
            subject.PreventLookAtZRotation = false;

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