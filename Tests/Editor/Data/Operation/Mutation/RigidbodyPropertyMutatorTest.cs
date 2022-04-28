using Zinnia.Data.Operation.Mutation;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class RigidbodyPropertyMutatorTest
    {
        private GameObject containingObject;
        private RigidbodyPropertyMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
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
            GameObject rigidBodyContainer = new GameObject();
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
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
            Assert.AreEqual(false, subjectRigidbody.useGravity);
            Assert.AreEqual(true, subjectRigidbody.isKinematic);
            Assert.AreEqual(Vector3.up, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.up, subjectRigidbody.angularVelocity);
            Assert.AreEqual(8f, subjectRigidbody.maxAngularVelocity);

            subject.ClearVelocity();
            subject.ClearAngularVelocity();

            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);

            Object.DestroyImmediate(rigidBodyContainer);
        }

        [Test]
        public void MutateGameObjectRigidbodyInactiveGameObject()
        {
            GameObject rigidBodyContainer = new GameObject();
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
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
            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
            Assert.AreEqual(7f, subjectRigidbody.maxAngularVelocity);

            Object.DestroyImmediate(rigidBodyContainer);
        }

        [Test]
        public void MutateGameObjectRigidbodyInactiveComponent()
        {
            GameObject rigidBodyContainer = new GameObject();
            Rigidbody subjectRigidbody = rigidBodyContainer.AddComponent<Rigidbody>();

            Assert.IsNull(subject.Target);
            Assert.AreEqual(1f, subjectRigidbody.mass);
            Assert.AreEqual(0f, subjectRigidbody.drag);
            Assert.AreEqual(0.05f, subjectRigidbody.angularDrag);
            Assert.AreEqual(true, subjectRigidbody.useGravity);
            Assert.AreEqual(false, subjectRigidbody.isKinematic);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
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
            Assert.AreEqual(Vector3.zero, subjectRigidbody.velocity);
            Assert.AreEqual(Vector3.zero, subjectRigidbody.angularVelocity);
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
            subject.Velocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityX(1f);
            Assert.AreEqual(Vector3.right, subject.Velocity);
        }

        [Test]
        public void SetVelocityY()
        {
            subject.Velocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityY(1f);
            Assert.AreEqual(Vector3.up, subject.Velocity);
        }

        [Test]
        public void SetVelocityZ()
        {
            subject.Velocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.SetVelocityZ(1f);
            Assert.AreEqual(Vector3.forward, subject.Velocity);
        }

        [Test]
        public void ClearVelocity()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.Velocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.Velocity);
            subject.ClearVelocity();
            Assert.AreEqual(Vector3.zero, subject.Velocity);
        }

        [Test]
        public void ClearVelocityInactiveGameObject()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.Velocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.Velocity);
            subject.gameObject.SetActive(false);
            subject.ClearVelocity();
            Assert.AreEqual(Vector3.one, subject.Velocity);
        }

        [Test]
        public void ClearVelocityInactiveComponent()
        {
            Assert.AreEqual(Vector3.zero, subject.Velocity);
            subject.Velocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.Velocity);
            subject.enabled = false;
            subject.ClearVelocity();
            Assert.AreEqual(Vector3.one, subject.Velocity);
        }

        [Test]
        public void SetAngularVelocityX()
        {
            subject.AngularVelocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityX(1f);
            Assert.AreEqual(Vector3.right, subject.AngularVelocity);
        }

        [Test]
        public void SetAngularVelocityY()
        {
            subject.AngularVelocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityY(1f);
            Assert.AreEqual(Vector3.up, subject.AngularVelocity);
        }

        [Test]
        public void SetAngularVelocityZ()
        {
            subject.AngularVelocity = Vector3.zero;
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.SetAngularVelocityZ(1f);
            Assert.AreEqual(Vector3.forward, subject.AngularVelocity);
        }

        [Test]
        public void ClearAngularVelocity()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.AngularVelocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.AngularVelocity);
            subject.ClearAngularVelocity();
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
        }

        [Test]
        public void ClearAngularVelocityInactiveGameObject()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.AngularVelocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.AngularVelocity);
            subject.gameObject.SetActive(false);
            subject.ClearAngularVelocity();
            Assert.AreEqual(Vector3.one, subject.AngularVelocity);
        }

        [Test]
        public void ClearAngularVelocityInactiveComponent()
        {
            Assert.AreEqual(Vector3.zero, subject.AngularVelocity);
            subject.AngularVelocity = Vector3.one;
            Assert.AreEqual(Vector3.one, subject.AngularVelocity);
            subject.enabled = false;
            subject.ClearAngularVelocity();
            Assert.AreEqual(Vector3.one, subject.AngularVelocity);
        }
    }
}