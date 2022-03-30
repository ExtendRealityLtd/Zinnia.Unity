using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;

    public class ArtificialVelocityApplierProcessTest
    {
        private GameObject containingObject;
        private ArtificialVelocityApplierProcess subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ArtificialVelocityApplierProcess>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
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

        [Test]
        public void SetVelocityX()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityX(1f);
            Assert.AreEqual(Vector3.right, subject.Velocity);
        }

        [Test]
        public void SetVelocityY()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityY(1f);
            Assert.AreEqual(Vector3.up, subject.Velocity);
        }

        [Test]
        public void SetVelocityZ()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityZ(1f);
            Assert.AreEqual(Vector3.forward, subject.Velocity);
        }

        [Test]
        public void IncrementVelocity()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.IncrementVelocity(Vector3.forward);
            Assert.AreEqual(Vector3.forward, subject.Velocity);
        }

        [Test]
        public void ClearVelocity()
        {
            subject.Velocity = Vector3.one;
            subject.ClearVelocity();
            Assert.AreEqual(Vector3.zero, subject.Velocity);
        }

        [Test]
        public void SetAngularVelocityX()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityX(1f);
            Assert.AreEqual(Vector3.right, subject.AngularVelocity);
        }

        [Test]
        public void SetAngularVelocityY()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityY(1f);
            Assert.AreEqual(Vector3.up, subject.AngularVelocity);
        }

        [Test]
        public void SetAngularVelocityZ()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityZ(1f);
            Assert.AreEqual(Vector3.forward, subject.AngularVelocity);
        }

        [Test]
        public void IncrementAngularVelocity()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.IncrementAngularVelocity(Vector3.forward);
            Assert.AreEqual(Vector3.forward, subject.AngularVelocity);
        }

        [Test]
        public void ClearAngularVelocity()
        {
            subject.AngularVelocity = Vector3.one;
            subject.ClearAngularVelocity();
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
        }

        [Test]
        public void Process()
        {
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.Apply();
            subject.Process();

            Assert.AreNotEqual(Vector3.zero, target.transform.position);
            Assert.AreNotEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.gameObject.SetActive(false);

            subject.Apply();
            subject.Process();

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.enabled = false;

            subject.Apply();
            subject.Process();

            Assert.AreEqual(Vector3.zero, target.transform.position);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
        }
    }
}