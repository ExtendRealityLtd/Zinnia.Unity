using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class ArtificialVelocityApplierProcessTest
    {
        private GameObject containingObject;
        private ArtificialVelocityApplierProcess subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ArtificialVelocityApplierProcessTest");
            subject = containingObject.AddComponent<ArtificialVelocityApplierProcess>();
        }

        [TearDown]
        public void TearDown()
        {
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityX(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.right).Using(comparer));
        }

        [Test]
        public void SetVelocityY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityY(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.up).Using(comparer));
        }

        [Test]
        public void SetVelocityZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityZ(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void IncrementVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IncrementVelocity(Vector3.forward);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void ClearVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.Velocity = Vector3.one;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.ClearVelocity();
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityX()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityX(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.right).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityY(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.up).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityZ(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void IncrementAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IncrementAngularVelocity(Vector3.forward);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void ClearAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.AngularVelocity = Vector3.one;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.ClearAngularVelocity();
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void Process()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.000001f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.000001f);
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.Apply();
            subject.Process();

            Assert.That(target.transform.position, Is.Not.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.Not.EqualTo(Quaternion.identity).Using(quaternionComparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.gameObject.SetActive(false);

            subject.Apply();
            subject.Process();

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("Target");
            subject.Target = target;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            subject.Velocity = Vector3.forward;
            subject.AngularVelocity = Vector3.forward;

            subject.enabled = false;

            subject.Apply();
            subject.Process();

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(quaternionComparer));

            Object.DestroyImmediate(target);
        }
    }
}