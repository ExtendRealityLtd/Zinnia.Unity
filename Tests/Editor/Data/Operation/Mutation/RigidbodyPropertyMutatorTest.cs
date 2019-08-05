using Zinnia.Data.Operation.Mutation;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using NUnit.Framework;
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
    }
}