using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
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
    }
}