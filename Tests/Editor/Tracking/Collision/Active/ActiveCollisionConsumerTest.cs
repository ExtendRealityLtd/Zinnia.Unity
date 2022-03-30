using Zinnia.Data.Collection.List;
using Zinnia.Rule;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActiveCollisionConsumerTest
    {
        private GameObject containingObject;
        private ActiveCollisionConsumer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionConsumer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Consume()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.SourceContainer = publisherObject;

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            Assert.IsTrue(subject.Consume(publisher, null));

            Assert.IsTrue(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.AreEqual(publisher, subject.PublisherSource);

            Object.DestroyImmediate(publisherObject);
        }

        [UnityTest]
        public IEnumerator ConsumeExclusion()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.PublisherContainer = publisherObject;

            publisherObject.AddComponent<RuleStub>();
            NegationRule negationRule = containingObject.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = containingObject.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.PublisherValidity = new RuleContainer
            {
                Interface = negationRule
            };

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            Assert.IsFalse(subject.Consume(publisher, null));

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            Object.DestroyImmediate(publisherObject);
        }

        [Test]
        public void ConsumeInactiveGameObject()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.SourceContainer = publisherObject;

            subject.gameObject.SetActive(false);
            Assert.IsFalse(subject.Consume(publisher, null));

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            Object.DestroyImmediate(publisherObject);
        }

        [Test]
        public void ConsumeInactiveComponent()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            GameObject publisherObject = new GameObject();
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();
            publisher.SourceContainer = publisherObject;

            subject.enabled = false;
            Assert.IsFalse(subject.Consume(publisher, null));

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            Object.DestroyImmediate(publisherObject);
        }

        [Test]
        public void Clear()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.Clear();

            Assert.IsFalse(consumedMock.Received);
            Assert.IsTrue(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);
        }

        [Test]
        public void ClearInactiveGameObject()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.gameObject.SetActive(false);
            subject.Clear();

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);
        }

        [Test]
        public void ClearInactiveComponent()
        {
            UnityEventListenerMock consumedMock = new UnityEventListenerMock();
            UnityEventListenerMock clearedMock = new UnityEventListenerMock();

            subject.Consumed.AddListener(consumedMock.Listen);
            subject.Cleared.AddListener(clearedMock.Listen);

            subject.enabled = false;
            subject.Clear();

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);
        }

        [Test]
        public void ClearContainer()
        {
            Assert.IsNull(subject.Container);
            subject.Container = containingObject;
            Assert.AreEqual(containingObject, subject.Container);
            subject.ClearContainer();
            Assert.IsNull(subject.Container);
        }

        [Test]
        public void ClearContainerInactiveGameObject()
        {
            Assert.IsNull(subject.Container);
            subject.Container = containingObject;
            Assert.AreEqual(containingObject, subject.Container);
            subject.gameObject.SetActive(false);
            subject.ClearContainer();
            Assert.AreEqual(containingObject, subject.Container);
        }

        [Test]
        public void ClearContainerInactiveComponent()
        {
            Assert.IsNull(subject.Container);
            subject.Container = containingObject;
            Assert.AreEqual(containingObject, subject.Container);
            subject.enabled = false;
            subject.ClearContainer();
            Assert.AreEqual(containingObject, subject.Container);
        }

        [Test]
        public void ClearPublisherValidity()
        {
            Assert.IsNull(subject.PublisherValidity);
            RuleContainer rule = new RuleContainer();
            subject.PublisherValidity = rule;
            Assert.AreEqual(rule, subject.PublisherValidity);
            subject.ClearPublisherValidity();
            Assert.IsNull(subject.PublisherValidity);
        }

        [Test]
        public void ClearPublisherValidityInactiveGameObject()
        {
            Assert.IsNull(subject.PublisherValidity);
            RuleContainer rule = new RuleContainer();
            subject.PublisherValidity = rule;
            Assert.AreEqual(rule, subject.PublisherValidity);
            subject.gameObject.SetActive(false);
            subject.ClearPublisherValidity();
            Assert.AreEqual(rule, subject.PublisherValidity);
        }

        [Test]
        public void ClearPublisherValidityInactiveComponent()
        {
            Assert.IsNull(subject.PublisherValidity);
            RuleContainer rule = new RuleContainer();
            subject.PublisherValidity = rule;
            Assert.AreEqual(rule, subject.PublisherValidity);
            subject.enabled = false;
            subject.ClearPublisherValidity();
            Assert.AreEqual(rule, subject.PublisherValidity);
        }
    }
}