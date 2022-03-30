using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class VelocityApplierTest
    {
        private GameObject containingObject;
        private VelocityApplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<VelocityApplier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Apply()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.Target = containingObject.AddComponent<Rigidbody>();
            subject.Apply();
            Assert.AreEqual(tracker.GetVelocity(), subject.Target.velocity);
            Assert.AreEqual(tracker.GetAngularVelocity(), subject.Target.angularVelocity);

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void ApplyNoSource()
        {
            subject.Target = containingObject.AddComponent<Rigidbody>();

            Vector3 originalVelocity = subject.Target.velocity;
            Vector3 originalAngularVelocity = subject.Target.angularVelocity;

            Assert.AreEqual(Vector3.zero, originalVelocity);
            Assert.AreEqual(Vector3.zero, originalAngularVelocity);

            subject.Apply();
            Assert.AreEqual(originalVelocity, subject.Target.velocity);
            Assert.AreEqual(originalAngularVelocity, subject.Target.angularVelocity);
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;

            Assert.AreEqual(tracker, subject.Source);

            subject.ClearSource();

            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;

            Assert.AreEqual(tracker, subject.Source);

            subject.gameObject.SetActive(false);
            subject.ClearSource();

            Assert.AreEqual(tracker, subject.Source);
        }

        [Test]
        public void ClearSourceInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;

            Assert.AreEqual(tracker, subject.Source);

            subject.enabled = false;
            subject.ClearSource();

            Assert.AreEqual(tracker, subject.Source);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            Rigidbody target = containingObject.AddComponent<Rigidbody>();
            subject.Target = target;

            Assert.AreEqual(target, subject.Target);

            subject.ClearTarget();

            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            Rigidbody target = containingObject.AddComponent<Rigidbody>();
            subject.Target = target;

            Assert.AreEqual(target, subject.Target);

            subject.gameObject.SetActive(false);
            subject.ClearTarget();

            Assert.AreEqual(target, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            Rigidbody target = containingObject.AddComponent<Rigidbody>();
            subject.Target = target;

            Assert.AreEqual(target, subject.Target);

            subject.enabled = false;
            subject.ClearTarget();

            Assert.AreEqual(target, subject.Target);
        }
    }
}