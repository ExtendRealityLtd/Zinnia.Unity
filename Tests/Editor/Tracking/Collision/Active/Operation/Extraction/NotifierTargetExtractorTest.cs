using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active.Operation.Extraction;

namespace Test.Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class NotifierTargetExtractorTest
    {
        private GameObject containingObject;
        private NotifierTargetExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("NotifierTargetExtractorTest");
            subject = containingObject.AddComponent<NotifierTargetExtractor>();
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

            Collider collider = new GameObject("NotifierTargetExtractorTest").AddComponent<BoxCollider>();
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
        public void ExtractCompoundParent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.ExtractCompoundParent = true;
            GameObject parent = new GameObject("NotifierTargetExtractorTest_Parent");
            parent.AddComponent<Rigidbody>();
            GameObject child = new GameObject("NotifierTargetExtractorTest_Child");
            Collider collider = child.AddComponent<BoxCollider>();
            child.transform.SetParent(parent.transform);
            CollisionNotifier.EventData eventData = new CollisionNotifier.EventData
            {
                ColliderData = collider
            };

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(parent, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractIgnoreCompoundParent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.ExtractCompoundParent = false;
            GameObject parent = new GameObject("NotifierTargetExtractorTest_Parent");
            parent.AddComponent<Rigidbody>();
            GameObject child = new GameObject("NotifierTargetExtractorTest_Child");
            Collider collider = child.AddComponent<BoxCollider>();
            child.transform.SetParent(parent.transform);
            CollisionNotifier.EventData eventData = new CollisionNotifier.EventData
            {
                ColliderData = collider
            };

            Assert.IsNull(subject.Result);
            Assert.IsFalse(extractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(child, subject.Result);
            Assert.IsTrue(extractedMock.Received);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Collider collider = new GameObject("NotifierTargetExtractorTest").AddComponent<BoxCollider>();
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

            Collider collider = new GameObject("NotifierTargetExtractorTest").AddComponent<BoxCollider>();
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