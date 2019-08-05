using Zinnia.Tracking.Collision.Event.Proxy;
using Zinnia.Rule;
using Zinnia.Tracking.Collision;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Tracking.Collision.Event.Proxy
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class CollisionNotifierEventProxyEmitterTest
    {
        private GameObject containingObject;
        private CollisionNotifierEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CollisionNotifierEventProxyEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Receive()
        {
            GameObject forwardSource = new GameObject();
            GameObject collisionSource = new GameObject();
            Collider collider = collisionSource.AddComponent<BoxCollider>();

            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            CollisionNotifier.EventData digest = new CollisionNotifier.EventData();
            digest.Set(forwardSource.GetComponent<Component>(), true, null, collider);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.AreEqual(digest, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            Object.DestroyImmediate(forwardSource);
            Object.DestroyImmediate(collisionSource);
        }

        [Test]
        public void ReceiveWithRuleRestrictionsOnForwardSource()
        {
            GameObject forwardSourceValid = new GameObject();
            GameObject forwardSourceInvalid = new GameObject();
            GameObject collisionSource = new GameObject();
            Collider collider = collisionSource.AddComponent<BoxCollider>();

            ListContainsRule rule = subject.gameObject.AddComponent<ListContainsRule>();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            rule.Objects = objects;

            objects.Add(forwardSourceValid);
            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            subject.RuleSource = CollisionNotifierEventProxyEmitter.RuleSourceType.ForwardSource;

            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            CollisionNotifier.EventData validDigest = new CollisionNotifier.EventData();
            validDigest.Set(forwardSourceValid.GetComponent<Component>(), true, null, collider);
            CollisionNotifier.EventData invalidDigest = new CollisionNotifier.EventData();
            invalidDigest.Set(forwardSourceInvalid.GetComponent<Component>(), true, null, collider);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(validDigest);

            Assert.AreEqual(validDigest, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            subject.Receive(invalidDigest);

            Assert.AreEqual(validDigest, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(forwardSourceValid);
            Object.DestroyImmediate(forwardSourceInvalid);
            Object.DestroyImmediate(collisionSource);
        }

        [Test]
        public void ReceiveWithRuleRestrictionsOnCollisionSource()
        {
            GameObject forwardSource = new GameObject();
            GameObject collisionSourceValid = new GameObject();
            Collider colliderValid = collisionSourceValid.AddComponent<BoxCollider>();
            GameObject collisionSourceInvalid = new GameObject();
            Collider colliderInvalid = collisionSourceInvalid.AddComponent<BoxCollider>();

            ListContainsRule rule = subject.gameObject.AddComponent<ListContainsRule>();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            rule.Objects = objects;

            objects.Add(collisionSourceValid);
            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            subject.RuleSource = CollisionNotifierEventProxyEmitter.RuleSourceType.CollidingSource;

            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            CollisionNotifier.EventData validDigest = new CollisionNotifier.EventData();
            validDigest.Set(forwardSource.GetComponent<Component>(), true, null, colliderValid);
            CollisionNotifier.EventData invalidDigest = new CollisionNotifier.EventData();
            invalidDigest.Set(forwardSource.GetComponent<Component>(), true, null, colliderInvalid);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(validDigest);

            Assert.AreEqual(validDigest, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            subject.Receive(invalidDigest);

            Assert.AreEqual(validDigest, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(forwardSource);
            Object.DestroyImmediate(collisionSourceValid);
            Object.DestroyImmediate(collisionSourceInvalid);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            GameObject forwardSource = new GameObject();
            GameObject collisionSource = new GameObject();
            Collider collider = collisionSource.AddComponent<BoxCollider>();

            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            CollisionNotifier.EventData digest = new CollisionNotifier.EventData();
            digest.Set(forwardSource.GetComponent<Component>(), true, null, collider);

            subject.gameObject.SetActive(false);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(forwardSource);
            Object.DestroyImmediate(collisionSource);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            GameObject forwardSource = new GameObject();
            GameObject collisionSource = new GameObject();
            Collider collider = collisionSource.AddComponent<BoxCollider>();

            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);

            CollisionNotifier.EventData digest = new CollisionNotifier.EventData();
            digest.Set(forwardSource.GetComponent<Component>(), true, null, collider);

            subject.enabled = false;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(digest);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(forwardSource);
            Object.DestroyImmediate(collisionSource);
        }
    }
}
