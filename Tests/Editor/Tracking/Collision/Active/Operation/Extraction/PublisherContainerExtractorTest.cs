using Zinnia.Tracking.Collision.Active.Operation.Extraction;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class PublisherContainerExtractorTest
    {
        private GameObject containingObject;
        private PublisherContainerExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PublisherContainerExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ExtractFromPublisher()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.SourceContainer = publisherSource;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(publisher);

            Assert.AreEqual(publisherSource, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractFromConsumerEvent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.SourceContainer = publisherSource;

            ActiveCollisionConsumer.EventData eventData = new ActiveCollisionConsumer.EventData();
            eventData.Set(publisher, null);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(publisherSource, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.SourceContainer = publisherSource;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Extract(publisher);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject publisherSource = new GameObject();
            GameObject publisherChild = new GameObject();
            publisherChild.transform.SetParent(publisherSource.transform);
            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            publisher.SourceContainer = publisherSource;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;
            subject.Extract(publisher);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(publisherSource);
            Object.DestroyImmediate(publisherChild);
        }

        [Test]
        public void ExtractInvalidPublisher()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            ActiveCollisionPublisher.PayloadData publisher = new ActiveCollisionPublisher.PayloadData();

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(publisher);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);
        }
    }
}