using Zinnia.Tracking.Collision.Active;
using Zinnia.Tracking.Collision.Active.Event.Proxy;

namespace Test.Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActiveCollisionPublisherEventProxyEmitterTest
    {
        private GameObject containingObject;
        private ActiveCollisionPublisherEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionPublisherEventProxyEmitter>();
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
            ActiveCollisionPublisher.PayloadData digest = new ActiveCollisionPublisher.PayloadData();

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
            ActiveCollisionPublisher.PayloadData digest = new ActiveCollisionPublisher.PayloadData();

            subject.gameObject.SetActive(false);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            ActiveCollisionPublisher.PayloadData digest = new ActiveCollisionPublisher.PayloadData();

            subject.enabled = false;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}