using VRTK.Core.Tracking.Collision.Active;
using VRTK.Core.Rule;

namespace Test.VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Stub;

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
            publisher.sourceContainer = publisherObject;

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            subject.Consume(publisher, null);

            Assert.IsTrue(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.AreEqual(publisher, subject.PublisherSource);

            Object.DestroyImmediate(publisherObject);
        }

        [Test]
        public void ConsumeExclusion()
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
            anyComponentTypeRule.componentTypes.Add(typeof(RuleStub));
            negationRule.rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.publisherValidity = new RuleContainer
            {
                Interface = negationRule
            };

            Assert.IsFalse(consumedMock.Received);
            Assert.IsFalse(clearedMock.Received);

            Assert.IsNull(subject.PublisherSource);

            subject.Consume(publisher, null);

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
            publisher.sourceContainer = publisherObject;

            subject.gameObject.SetActive(false);
            subject.Consume(publisher, null);

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
            publisher.sourceContainer = publisherObject;

            subject.enabled = false;
            subject.Consume(publisher, null);

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
    }
}