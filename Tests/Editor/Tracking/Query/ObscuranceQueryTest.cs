using Zinnia.Tracking.Query;

namespace Test.Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class ObscuranceQueryTest
    {
        private GameObject containingObject;
        private ObscuranceQuery subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ObscuranceQuery>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator ObscuredThenUnobscured()
        {
            UnityEventListenerMock targetObscuredMock = new UnityEventListenerMock();
            UnityEventListenerMock targetUnobscuredMock = new UnityEventListenerMock();
            subject.TargetObscured.AddListener(targetObscuredMock.Listen);
            subject.TargetUnobscured.AddListener(targetUnobscuredMock.Listen);

            GameObject objectA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject objectB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectA.transform.position = Vector3.left * 2f;
            objectB.transform.position = Vector3.right * 2f;

            subject.Source = objectA;
            subject.Target = objectB;

            Assert.IsFalse(targetObscuredMock.Received);
            Assert.IsFalse(targetUnobscuredMock.Received);

            targetObscuredMock.Reset();
            targetUnobscuredMock.Reset();

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetObscuredMock.Received);
            Assert.IsTrue(targetUnobscuredMock.Received);

            targetObscuredMock.Reset();
            targetUnobscuredMock.Reset();

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetObscuredMock.Received);
            Assert.IsFalse(targetUnobscuredMock.Received);

            targetObscuredMock.Reset();
            targetUnobscuredMock.Reset();

            GameObject obscurer = GameObject.CreatePrimitive(PrimitiveType.Cube);

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsTrue(targetObscuredMock.Received);
            Assert.IsFalse(targetUnobscuredMock.Received);

            targetObscuredMock.Reset();
            targetUnobscuredMock.Reset();

            Object.Destroy(obscurer);

            yield return new WaitForEndOfFrame();

            subject.Process();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(targetObscuredMock.Received);
            Assert.IsTrue(targetUnobscuredMock.Received);

            Object.Destroy(objectA);
            Object.Destroy(objectB);
        }

        [Test]
        public void TargetWithoutCollider_ThrowsMissingColliderException()
        {
            GameObject gameObject = new GameObject();
            Assert.Throws<ObscuranceQuery.MissingColliderException>(() => subject.Target = gameObject);
            Object.DestroyImmediate(gameObject);
        }
    }
}
