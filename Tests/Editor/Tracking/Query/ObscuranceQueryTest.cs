using Zinnia.Cast;
using Zinnia.Tracking.Query;

namespace Test.Zinnia.Tracking.Query
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
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
            containingObject.AddComponent<Rigidbody>();
            containingObject.AddComponent<BoxCollider>();
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            containingObject.AddComponent<Rigidbody>();
            containingObject.AddComponent<BoxCollider>();
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
            containingObject.AddComponent<Rigidbody>();
            containingObject.AddComponent<BoxCollider>();
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearPhysicsCast()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.ClearPhysicsCast();
            Assert.IsNull(subject.PhysicsCast);
        }

        [Test]
        public void ClearPhysicsCastInactiveGameObject()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.gameObject.SetActive(false);
            subject.ClearPhysicsCast();
            Assert.AreEqual(cast, subject.PhysicsCast);
        }

        [Test]
        public void ClearPhysicsCastInactiveComponent()
        {
            Assert.IsNull(subject.PhysicsCast);
            PhysicsCast cast = containingObject.AddComponent<PhysicsCast>();
            subject.PhysicsCast = cast;
            Assert.AreEqual(cast, subject.PhysicsCast);
            subject.enabled = false;
            subject.ClearPhysicsCast();
            Assert.AreEqual(cast, subject.PhysicsCast);
        }
    }
}
