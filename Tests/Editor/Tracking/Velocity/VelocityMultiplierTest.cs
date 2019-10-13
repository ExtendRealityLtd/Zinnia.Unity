using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class VelocityMultiplierTest
    {
        private GameObject containingObject;
        private VelocityMultiplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<VelocityMultiplier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetVelocity()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.VelocityMultiplierFactor = Vector3.one * 2f;
            Assert.AreEqual(Vector3.one * 2f, subject.GetVelocity());
            Assert.AreEqual(Vector3.one, subject.GetAngularVelocity());

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void GetAngularVelocity()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            Assert.AreEqual(Vector3.one, subject.GetVelocity());
            Assert.AreEqual(Vector3.one * 2f, subject.GetAngularVelocity());

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void InactiveSource()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            subject.Source.enabled = false;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void InactiveComponent()
        {
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            subject.enabled = false;
            Assert.AreEqual(Vector3.zero, subject.GetVelocity());
            Assert.AreEqual(Vector3.zero, subject.GetAngularVelocity());

            Object.DestroyImmediate(tracker.gameObject);
        }
    }
}