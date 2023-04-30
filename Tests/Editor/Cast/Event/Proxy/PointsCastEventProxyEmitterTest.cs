using Zinnia.Cast;
using Zinnia.Cast.Event.Proxy;
using Zinnia.Data.Collection.List;
using Zinnia.Rule;

namespace Test.Zinnia.Cast.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Helper;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class PointsCastEventProxyEmitterTest
    {
        private GameObject containingObject;
        private PointsCastEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("PointsCastEventProxyEmitterTest");
            subject = containingObject.AddComponent<PointsCastEventProxyEmitter>();
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

            PointsCast.EventData data = new PointsCast.EventData();
            data.HitData = RaycastHitHelper.GetRaycastHit();
            data.IsValid = true;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(data);

            Assert.AreEqual(data, subject.Payload);
            Assert.IsTrue(emittedMock.Received);
        }

        [Test]
        public void ReceiveWithRuleRestrictions()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            GameObject validBlockerParent = new GameObject("PointsCastEventProxyEmitterTest_validParent");
            validBlockerParent.AddComponent<Rigidbody>().isKinematic = true;
            GameObject validBlockerChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
            validBlockerChild.name = "PointsCastEventProxyEmitterTest_validChild";
            validBlockerChild.transform.SetParent(validBlockerParent.transform);
            validBlockerParent.transform.position = Vector3.left + Vector3.forward;

            GameObject invalidBlockerParent = new GameObject("PointsCastEventProxyEmitterTest_invalidParent");
            invalidBlockerParent.AddComponent<Rigidbody>().isKinematic = true;
            GameObject invalidBlockerChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
            invalidBlockerChild.name = "PointsCastEventProxyEmitterTest_invalidChild";
            invalidBlockerChild.transform.SetParent(invalidBlockerParent.transform);
            invalidBlockerParent.transform.position = Vector3.right + Vector3.forward;

            ListContainsRule rule = subject.gameObject.AddComponent<ListContainsRule>();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            rule.Objects = objects;

            objects.Add(validBlockerChild);
            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            subject.RuleSource = PointsCastEventProxyEmitter.RuleSourceType.Collider;

            PointsCast.EventData data = new PointsCast.EventData();
            data.HitData = RaycastHitHelper.GetRaycastHit(validBlockerParent, false, Vector3.left, Vector3.forward);
            data.IsValid = true;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(data);

            Assert.AreEqual(data, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            subject.Payload = null;
            emittedMock.Reset();

            subject.RuleSource = PointsCastEventProxyEmitter.RuleSourceType.Rigidbody;

            subject.Receive(data);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Payload = null;
            emittedMock.Reset();

            objects.Add(validBlockerParent);

            subject.Receive(data);

            Assert.AreEqual(data, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            subject.Payload = null;
            emittedMock.Reset();

            subject.RuleSource = PointsCastEventProxyEmitter.RuleSourceType.Collider;

            data.HitData = RaycastHitHelper.GetRaycastHit(invalidBlockerParent, false, Vector3.right, Vector3.forward);
            data.IsValid = true;

            subject.Receive(data);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Payload = null;
            emittedMock.Reset();

            subject.RuleSource = PointsCastEventProxyEmitter.RuleSourceType.Rigidbody;

            subject.Receive(data);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(validBlockerParent);
            Object.DestroyImmediate(invalidBlockerParent);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            PointsCast.EventData data = new PointsCast.EventData();
            data.HitData = RaycastHitHelper.GetRaycastHit();
            data.IsValid = true;

            subject.gameObject.SetActive(false);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(data);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            PointsCast.EventData data = new PointsCast.EventData();
            data.HitData = RaycastHitHelper.GetRaycastHit();
            data.IsValid = true;

            subject.enabled = false;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(data);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);
        }
    }
}