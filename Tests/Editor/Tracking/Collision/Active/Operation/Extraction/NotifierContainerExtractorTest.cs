using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active.Operation.Extraction;

namespace Test.Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class NotifierContainerExtractorTest
    {
        private GameObject containingObject;
        private NotifierContainerExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("NotifierContainerExtractorTest");
            subject = containingObject.AddComponent<NotifierContainerExtractor>();
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

            GameObject forwardSource = new GameObject("NotifierContainerExtractorTest");
            CollisionNotifier.EventData notifier = new CollisionNotifier.EventData();
            notifier.ForwardSource = forwardSource.GetComponent<Component>();

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(notifier);

            Assert.AreEqual(forwardSource, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(forwardSource);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject forwardSource = new GameObject("NotifierContainerExtractorTest");
            CollisionNotifier.EventData notifier = new CollisionNotifier.EventData();
            notifier.ForwardSource = forwardSource.GetComponent<Component>();

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Extract(notifier);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(forwardSource);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject forwardSource = new GameObject("NotifierContainerExtractorTest");
            CollisionNotifier.EventData notifier = new CollisionNotifier.EventData();
            notifier.ForwardSource = forwardSource.GetComponent<Component>();

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;
            subject.Extract(notifier);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(forwardSource);
        }

        [Test]
        public void ExtractInvalidNotifier()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            GameObject forwardSource = new GameObject("NotifierContainerExtractorTest");
            CollisionNotifier.EventData notifier = null;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(notifier);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(forwardSource);
        }
    }
}