using VRTK.Core.Event;
using VRTK.Core.Tracking.Collision.Active;

namespace Test.VRTK.Core.Event
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class ActiveCollisionsContainerEventProxyEmitterTest
    {
        private GameObject containingObject;
        private ActiveCollisionsContainerEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionsContainerEventProxyEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Receive()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            ActiveCollisionsContainer.EventData digest = new ActiveCollisionsContainer.EventData();

            Assert.IsFalse(emittedMock.Received);
            subject.Receive(digest);
            Assert.AreEqual(digest, subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            ActiveCollisionsContainer.EventData digest = new ActiveCollisionsContainer.EventData();

            subject.gameObject.SetActive(false);

            Assert.IsFalse(emittedMock.Received);
            subject.Receive(digest);
            Assert.AreEqual(digest, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            ActiveCollisionsContainer.EventData digest = new ActiveCollisionsContainer.EventData();

            subject.enabled = false;

            Assert.IsFalse(emittedMock.Received);
            subject.Receive(digest);
            Assert.AreEqual(digest, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}