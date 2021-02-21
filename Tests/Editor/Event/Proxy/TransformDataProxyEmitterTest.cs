using Zinnia.Data.Collection.List;
using Zinnia.Data.Type;
using Zinnia.Event.Proxy;
using Zinnia.Rule;

namespace Test.Zinnia.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformDataProxyEmitterTest
    {
        private GameObject containingObject;
        private TransformDataProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformDataProxyEmitter>();
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
            TransformData payload = new TransformData();

            Assert.IsNull(subject.Payload);
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
            TransformData payload = new TransformData();
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
            TransformData payload = new TransformData();
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            subject.ClearPayload();
            Assert.IsNull(subject.Payload);
        }

        [Test]
        public void ReceiveWithRuleRestrictions()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            GameObject digestValid = new GameObject();
            GameObject digestInvalid = new GameObject();
            TransformData validData = new TransformData(digestValid.transform);
            TransformData invalidData = new TransformData(digestInvalid.transform);

            ListContainsRule rule = subject.gameObject.AddComponent<ListContainsRule>();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            rule.Objects = objects;

            objects.Add(digestValid);
            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(validData);

            Assert.AreEqual(validData, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            Assert.IsFalse(emittedMock.Received);

            subject.Receive(invalidData);

            Assert.AreEqual(validData, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(digestValid);
            Object.DestroyImmediate(digestInvalid);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);
            TransformData payload = new TransformData();

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.enabled = false;
            TransformData payload = new TransformData();

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(payload);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void EmitPayloadInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            subject.gameObject.SetActive(false);
            TransformData payload = new TransformData();
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
            TransformData payload = new TransformData();
            subject.Payload = payload;

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.EmitPayload();

            Assert.AreEqual(payload, subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}
