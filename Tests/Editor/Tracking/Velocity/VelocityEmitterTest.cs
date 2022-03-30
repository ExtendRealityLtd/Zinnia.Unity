using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class VelocityEmitterTest
    {
        private GameObject containingObject;
        private VelocityEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<VelocityEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void EmitAll()
        {
            UnityEventListenerMock velocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock speedEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularVelocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularSpeedEmittedMock = new UnityEventListenerMock();

            subject.VelocityEmitted.AddListener(velocityEmittedMock.Listen);
            subject.SpeedEmitted.AddListener(speedEmittedMock.Listen);
            subject.AngularVelocityEmitted.AddListener(angularVelocityEmittedMock.Listen);
            subject.AngularSpeedEmitted.AddListener(angularSpeedEmittedMock.Listen);

            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;

            subject.EmitAll();

            Assert.IsTrue(velocityEmittedMock.Received);
            Assert.IsTrue(speedEmittedMock.Received);
            Assert.IsTrue(angularVelocityEmittedMock.Received);
            Assert.IsTrue(angularSpeedEmittedMock.Received);

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void EmitAllInactiveGameObject()
        {
            UnityEventListenerMock velocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock speedEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularVelocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularSpeedEmittedMock = new UnityEventListenerMock();

            subject.VelocityEmitted.AddListener(velocityEmittedMock.Listen);
            subject.SpeedEmitted.AddListener(speedEmittedMock.Listen);
            subject.AngularVelocityEmitted.AddListener(angularVelocityEmittedMock.Listen);
            subject.AngularSpeedEmitted.AddListener(angularSpeedEmittedMock.Listen);

            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.gameObject.SetActive(false);

            subject.EmitAll();

            Assert.IsFalse(velocityEmittedMock.Received);
            Assert.IsFalse(speedEmittedMock.Received);
            Assert.IsFalse(angularVelocityEmittedMock.Received);
            Assert.IsFalse(angularSpeedEmittedMock.Received);

            Object.DestroyImmediate(tracker.gameObject);
        }

        [Test]
        public void EmitAllInactiveComponent()
        {
            UnityEventListenerMock velocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock speedEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularVelocityEmittedMock = new UnityEventListenerMock();
            UnityEventListenerMock angularSpeedEmittedMock = new UnityEventListenerMock();

            subject.VelocityEmitted.AddListener(velocityEmittedMock.Listen);
            subject.SpeedEmitted.AddListener(speedEmittedMock.Listen);
            subject.AngularVelocityEmitted.AddListener(angularVelocityEmittedMock.Listen);
            subject.AngularSpeedEmitted.AddListener(angularSpeedEmittedMock.Listen);

            VelocityTrackerMock tracker = VelocityTrackerMock.Generate(true, Vector3.one, Vector3.one);
            subject.Source = tracker;
            subject.enabled = false;

            subject.EmitAll();

            Assert.IsFalse(velocityEmittedMock.Received);
            Assert.IsFalse(speedEmittedMock.Received);
            Assert.IsFalse(angularVelocityEmittedMock.Received);
            Assert.IsFalse(angularSpeedEmittedMock.Received);

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
    }
}