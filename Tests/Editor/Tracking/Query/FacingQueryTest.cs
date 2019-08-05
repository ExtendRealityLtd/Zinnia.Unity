using Zinnia.Tracking.Query;

namespace Test.Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
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
    }
}
