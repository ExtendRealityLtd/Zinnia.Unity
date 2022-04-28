using Zinnia.Tracking.Query;

namespace Test.Zinnia.Tracking.Query
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class FacingQueryTest
    {
        private GameObject containingObject;
        private FacingQuery subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FacingQuery>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator FacingThenNotFacing()
        {
            UnityEventListenerMock targetFacedMock = new UnityEventListenerMock();
            UnityEventListenerMock targetNotFacedMock = new UnityEventListenerMock();
            subject.TargetFaced.AddListener(targetFacedMock.Listen);
            subject.TargetNotFaced.AddListener(targetNotFacedMock.Listen);

            GameObject objectA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject objectB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectA.transform.position = Vector3.left * 2f;
            objectB.transform.position = Vector3.right * 2f;

            subject.Source = objectA;
            subject.Target = objectB;

            Assert.IsFalse(targetFacedMock.Received);
            Assert.IsFalse(targetNotFacedMock.Received);

            targetFacedMock.Reset();
            targetNotFacedMock.Reset();

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetFacedMock.Received);
            Assert.IsTrue(targetNotFacedMock.Received);

            targetFacedMock.Reset();
            targetNotFacedMock.Reset();

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetFacedMock.Received);
            Assert.IsFalse(targetNotFacedMock.Received);

            targetFacedMock.Reset();
            targetNotFacedMock.Reset();

            objectA.transform.eulerAngles = Vector3.up * 90f;

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsTrue(targetFacedMock.Received);
            Assert.IsFalse(targetNotFacedMock.Received);

            targetFacedMock.Reset();
            targetNotFacedMock.Reset();

            objectA.transform.eulerAngles = Vector3.up * 180f;

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetFacedMock.Received);
            Assert.IsTrue(targetNotFacedMock.Received);

            Object.Destroy(objectA);
            Object.Destroy(objectB);
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearSourceInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            subject.Source = containingObject;
            Assert.AreEqual(containingObject, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(containingObject, subject.Source);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }
    }
}
