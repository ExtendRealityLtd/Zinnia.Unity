using Zinnia.Tracking.Collision.Active.Event.Proxy;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active.Event.Proxy
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActiveCollisionConsumerEventProxyEmitterTest
    {
        private GameObject containingObject;
        private ActiveCollisionConsumerEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionConsumerEventProxyEmitter>();
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
            ActiveCollisionConsumer.EventData digest = new ActiveCollisionConsumer.EventData();

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
            ActiveCollisionConsumer.EventData digest = new ActiveCollisionConsumer.EventData();

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
            ActiveCollisionConsumer.EventData digest = new ActiveCollisionConsumer.EventData();

            subject.enabled = false;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}