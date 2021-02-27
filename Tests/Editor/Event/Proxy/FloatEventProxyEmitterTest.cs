using Zinnia.Data.Type;
using Zinnia.Event.Proxy;
using Zinnia.Rule;

namespace Test.Zinnia.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatEventProxyEmitterTest
    {
        private GameObject containingObject;
        private FloatEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatEventProxyEmitter>();
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
            const float payload = 10f;

            Assert.AreEqual(0f, subject.Payload);
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
            const float payload = 10f;
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
            const float payload = 10f;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            subject.ClearPayload();
            Assert.AreEqual(0f, subject.Payload);
        }

        [Test]
        public void ReceiveWithRuleRestrictions()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            FloatInRangeRule rule = subject.gameObject.AddComponent<FloatInRangeRule>();
            rule.Range = new FloatRange(2f, 4f);

            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            Assert.AreEqual(0f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(3f);

            Assert.AreEqual(3f, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            Assert.IsFalse(emittedMock.Received);

            subject.Receive(1f);

            Assert.AreEqual(3f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);

            const float payload = 10f;

            Assert.AreEqual(0f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.AreEqual(0f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;

            const float payload = 10f;

            Assert.AreEqual(0f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.AreEqual(0f, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);
            const float payload = 10f;
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
            const float payload = 10f;
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}
