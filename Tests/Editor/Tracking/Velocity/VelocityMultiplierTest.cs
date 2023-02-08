using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class VelocityMultiplierTest
    {
        private GameObject containingObject;
        private VelocityMultiplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("VelocityMultiplierTest");
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.VelocityMultiplierFactor = Vector3.one * 2f;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.one * 2f).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void GetAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.one * 2f).Using(comparer));

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void InactiveSource()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            subject.Source.enabled = false;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void InactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.AngularVelocityMultiplierFactor = Vector3.one * 2f;
            subject.enabled = false;
            Assert.That(subject.GetVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.GetAngularVelocity(), Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(tracker.gameObject);
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
    }
}