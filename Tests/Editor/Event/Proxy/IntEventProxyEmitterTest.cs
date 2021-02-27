using Zinnia.Data.Type;
using Zinnia.Event.Proxy;
using Zinnia.Rule;

namespace Test.Zinnia.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class IntEventProxyEmitterTest
    {
        private GameObject containingObject;
        private IntEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<IntEventProxyEmitter>();
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
            const int payload = 10;

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void EmitPayload()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            const int payload = 10;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void ClearPayload()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            const int payload = 10;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            subject.ClearPayload();
            Assert.AreEqual(0, subject.Payload);
        }

        [Test]
        public void ReceiveWithRuleRestrictions()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            IntInRangeRule rule = subject.gameObject.AddComponent<IntInRangeRule>();
            rule.Range = new IntRange(2, 4);

            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(3);

            Assert.AreEqual(3, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            Assert.IsFalse(emittedMock.Received);

            subject.Receive(1);

            Assert.AreEqual(3, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);

            const int payload = 10;

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;

            const int payload = 10;

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.AreEqual(0, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);
            const int payload = 10;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;
            const int payload = 10;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}
