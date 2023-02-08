using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class VelocityApplierTest
    {
        private GameObject containingObject;
        private VelocityApplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("VelocityApplierTest");
            subject = containingObject.AddComponent<VelocityApplier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Apply()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.Target = containingObject.AddComponent<Rigidbody>();
            subject.Apply();
            Assert.That(subject.Target.velocity, Is.EqualTo(tracker.GetVelocity()).Using(comparer));
            Assert.That(subject.Target.angularVelocity, Is.EqualTo(tracker.GetAngularVelocity()).Using(comparer));

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void ApplyNoSource()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.Target = containingObject.AddComponent<Rigidbody>();

            Vector3 originalVelocity = subject.Target.velocity;
            Vector3 originalAngularVelocity = subject.Target.angularVelocity;

            Assert.That(originalVelocity, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(originalAngularVelocity, Is.EqualTo(Vector3.zero).Using(comparer));

            subject.Apply();

            Assert.That(subject.Target.velocity, Is.EqualTo(originalVelocity).Using(comparer));
            Assert.That(subject.Target.angularVelocity, Is.EqualTo(originalAngularVelocity).Using(comparer));
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
            Object.DestroyImmediate(tracker.gameObject);
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
            Object.DestroyImmediate(tracker.gameObject);
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
            Object.DestroyImmediate(tracker.gameObject);
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