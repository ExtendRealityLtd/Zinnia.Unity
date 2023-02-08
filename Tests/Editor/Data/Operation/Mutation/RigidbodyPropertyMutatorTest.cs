using Zinnia.Data.Operation.Mutation;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class RigidbodyPropertyMutatorTest
    {
        private GameObject containingObject;
        private RigidbodyPropertyMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("RigidbodyPropertyMutatorTest");
            subject = containingObject.AddComponent<RigidbodyPropertyMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void MutateGameObjectRigidbody()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject rigidBodyContainer = new GameObject("RigidbodyPropertyMutatorTest");
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.IsTrue(subjectRigidbody.useGravity);
            Assert.IsFalse(subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            subject.SetTarget(rigidBodyContainer);
            subject.Mass = 2f;
            subject.Drag = 1f;
            subject.AngularDrag = 1f;
            subject.UseGravity = false;
            subject.IsKinematic = true;
            subject.Velocity = Vector3.up;
            subject.AngularVelocity = Vector3.up;
            subject.MaxAngularVelocity = 8f;

            Assert.AreEqual(subjectRigidbody, subject.Target);
            Assert.AreEqual(2f, subjectRigidbody.mass);
            Assert.AreEqual(1f, subjectRigidbody.drag);
            Assert.AreEqual(1f, subjectRigidbody.angularDrag);
            Assert.IsFalse(subjectRigidbody.useGravity);
            Assert.IsTrue(subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(8f, subjectRigidbody.maxAngularVelocity);

            subject.IsKinematic = false;
            subject.Velocity = Vector3.up;
            subject.AngularVelocity = Vector3.up;

            Assert.IsFalse(subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.up).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.up).Using(comparer));

            subject.ClearVelocity();
            subject.ClearAngularVelocity();

            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(rigidBodyContainer);
        }

        [Test]
        public void MutateGameObjectRigidbodyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject rigidBodyContainer = new GameObject("RigidbodyPropertyMutatorTest");
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            subject.gameObject.SetActive(false);

            subject.SetTarget(rigidBodyContainer);
            subject.Mass = 2f;
            subject.Drag = 1f;
            subject.AngularDrag = 1f;
            subject.UseGravity = false;
            subject.IsKinematic = true;
            subject.Velocity = Vector3.up;
            subject.AngularVelocity = Vector3.up;
            subject.MaxAngularVelocity = 8f;

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            Object.DestroyImmediate(rigidBodyContainer);
        }

        [Test]
        public void MutateGameObjectRigidbodyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject rigidBodyContainer = new GameObject("RigidbodyPropertyMutatorTest");
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            subject.enabled = false;

            subject.SetTarget(rigidBodyContainer);
            subject.Mass = 2f;
            subject.Drag = 1f;
            subject.AngularDrag = 1f;
            subject.UseGravity = false;
            subject.IsKinematic = true;
            subject.Velocity = Vector3.up;
            subject.AngularVelocity = Vector3.up;
            subject.MaxAngularVelocity = 8f;

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.That(subjectRigidbody.velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subjectRigidbody.angularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            Object.DestroyImmediate(rigidBodyContainer);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            Rigidbody rb = containingObject.AddComponent<Rigidbody>();
            subject.Target = rb;
            Assert.AreEqual(rb, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            Rigidbody rb = containingObject.AddComponent<Rigidbody>();
            subject.Target = rb;
            Assert.AreEqual(rb, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(rb, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            Rigidbody rb = containingObject.AddComponent<Rigidbody>();
            subject.Target = rb;
            Assert.AreEqual(rb, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(rb, subject.Target);
        }

        [Test]
        public void SetVelocityX()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.Velocity = Vector3.zero;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityX(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.right).Using(comparer));
        }

        [Test]
        public void SetVelocityY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.Velocity = Vector3.zero;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityY(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.up).Using(comparer));
        }

        [Test]
        public void SetVelocityZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.Velocity = Vector3.zero;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetVelocityZ(1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void ClearVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.Velocity = Vector3.one;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.ClearVelocity();
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void ClearVelocityInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.Velocity = Vector3.one;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.gameObject.SetActive(false);
            subject.ClearVelocity();
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void ClearVelocityInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.Velocity = Vector3.one;
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.enabled = false;
            subject.ClearVelocity();
            Assert.That(subject.Velocity, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityX()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.zero;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityX(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.right).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.zero;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityY(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.up).Using(comparer));
        }

        [Test]
        public void SetAngularVelocityZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.zero;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.SetAngularVelocityZ(1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void ClearAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.one;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.ClearAngularVelocity();
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void ClearAngularVelocityInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.one;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.gameObject.SetActive(false);
            subject.ClearAngularVelocity();
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void ClearAngularVelocityInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            subject.IsKinematic = false;
            subject.AngularVelocity = Vector3.one;
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
            subject.enabled = false;
            subject.ClearAngularVelocity();
            Assert.That(subject.AngularVelocity, Is.EqualTo(Vector3.one).Using(comparer));
        }
    }
}