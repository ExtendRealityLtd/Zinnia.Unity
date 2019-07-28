using Zinnia.Tracking.Collision.Active.Operation.Extraction;
using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class NotifierTargetExtractorTest
    {
        private GameObject containingObject;
        private NotifierTargetExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<NotifierTargetExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Collider collider = new GameObject().AddComponent<BoxCollider>();
            CollisionNotifier.EventData eventData = new CollisionNotifier.EventData
            {
                ColliderData = collider
            };

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(collider.gameObject, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(collider.gameObject);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Collider collider = new GameObject().AddComponent<BoxCollider>();
            CollisionNotifier.EventData eventData = new CollisionNotifier.EventData
            {
                ColliderData = collider
            };

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Extract(eventData);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(collider.gameObject);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Collider collider = new GameObject().AddComponent<BoxCollider>();
            CollisionNotifier.EventData eventData = new CollisionNotifier.EventData
            {
                ColliderData = collider
            };

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;
            subject.Extract(eventData);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            Object.DestroyImmediate(collider.gameObject);
        }

        [Test]
        public void ExtractInvalidNotifier()
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