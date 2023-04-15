using Zinnia.Tracking.Collision.Active;
using Zinnia.Tracking.Collision.Active.Operation.Extraction;

namespace Test.Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using global::Zinnia.Tracking.Collision;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class ActiveCollisionConsumerEventCollisionExtractorTest
    {
        private GameObject containingObject;
        private ActiveCollisionConsumerEventCollisionExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("PublisherContainerExtractorTest");
            subject = containingObject.AddComponent<ActiveCollisionConsumerEventCollisionExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            ActiveCollisionConsumer.EventData input = new ActiveCollisionConsumer.EventData();
            CollisionNotifier.EventData expectedOutput = new CollisionNotifier.EventData();

            input.CurrentCollision = expectedOutput;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(input);

            Assert.AreEqual(expectedOutput, subject.Result);
            Assert.IsTrue(extractedMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            ActiveCollisionConsumer.EventData input = new ActiveCollisionConsumer.EventData();
            CollisionNotifier.EventData expectedOutput = new CollisionNotifier.EventData();

            input.CurrentCollision = expectedOutput;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);

            subject.Extract(input);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            ActiveCollisionConsumer.EventData input = new ActiveCollisionConsumer.EventData();
            CollisionNotifier.EventData expectedOutput = new CollisionNotifier.EventData();

            input.CurrentCollision = expectedOutput;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;

            subject.Extract(input);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);
        }

        [Test]
        public void ExtractInvalidConsumer()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(null);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);
        }
    }

}