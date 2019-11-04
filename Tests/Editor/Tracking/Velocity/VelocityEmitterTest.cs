﻿using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
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
    }
}