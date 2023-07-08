using Zinnia.Data.Type;
using Zinnia.Event.Proxy;
using Zinnia.Rule;

namespace Test.Zinnia.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class BooleanEventProxyEmitterTest
    {
        private GameObject containingObject;
        private BooleanEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("BooleanEventProxyEmitterTest");
            subject = containingObject.AddComponent<BooleanEventProxyEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Receive()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            bool payload = true;

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.IsTrue(subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void EmitPayload()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            bool payload = true;
            subject.Payload = payload;

            Assert.IsTrue(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.IsTrue(subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void ClearPayload()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            bool payload = true;
            subject.Payload = payload;

            Assert.IsTrue(subject.Payload);
            subject.ClearPayload();
            Assert.IsFalse(subject.Payload);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);

            bool payload = true;

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;

            bool payload = true;

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);
            bool payload = false;
            subject.Payload = payload;

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;
            bool payload = false;
            subject.Payload = payload;

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.IsFalse(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}
