using Zinnia.Tracking.Query;

namespace Test.Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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

        [UnityTest]
        public IEnumerator ObscuredThenUnobscuredCompoundColliders()
        {
            UnityEventListenerMock targetObscuredMock = new UnityEventListenerMock();
            UnityEventListenerMock targetUnobscuredMock = new UnityEventListenerMock();
            subject.TargetObscured.AddListener(targetObscuredMock.Listen);
            subject.TargetUnobscured.AddListener(targetUnobscuredMock.Listen);

            GameObject objectA = GameObject.CreatePrimitive(PrimitiveType.Cube);

            GameObject objectB = new GameObject();
            objectB.AddComponent<Rigidbody>();
            GameObject objectC = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objectC.transform.SetParent(objectB.transform);

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
            Object.Destroy(objectC);
        }

        [Test]
        public void TargetWithoutCollider_ThrowsMissingColliderException()
        {
            GameObject target = new GameObject();
            NUnit.Framework.Assert.Throws<ObscuranceQuery.MissingColliderException>(() => subject.Target = target);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void TargetWithRigidbodyWithoutCompoundCollider_ThrowsMissingColliderException()
        {
            GameObject target = new GameObject();
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.transform.SetParent(target.transform);

            NUnit.Framework.Assert.Throws<ObscuranceQuery.MissingColliderException>(() => subject.Target = target);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void TargetWithoutRigidbodyWithCompoundCollider_ThrowsMissingColliderException()
        {
            GameObject target = new GameObject();
            target.AddComponent<Rigidbody>();
            GameObject child = new GameObject();
            child.transform.SetParent(target.transform);

            NUnit.Framework.Assert.Throws<ObscuranceQuery.MissingColliderException>(() => subject.Target = target);
            Object.DestroyImmediate(target);
        }
    }
}
