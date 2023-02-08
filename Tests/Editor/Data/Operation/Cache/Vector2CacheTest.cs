using Zinnia.Data.Operation.Cache;

namespace Test.Zinnia.Data.Operation.Cache
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector2CacheTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private Vector2Cache subject;
#pragma warning restore 0618

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector2CacheTest");
#pragma warning disable 0618
            subject = containingObject.AddComponent<Vector2Cache>();
#pragma warning restore 0618
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CacheChanged()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));

            subject.Data = Vector2.one;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.one).Using(comparer));

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsTrue(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.one).Using(comparer));

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = Vector2.one * 2f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.one * 2f).Using(comparer));

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.ClearCache();

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));
        }

        [Test]
        public void CacheChangedInactiveGameObject()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));

            subject.gameObject.SetActive(false);
            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));
        }

        [Test]
        public void CacheChangedInactiveComponent()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));

            subject.enabled = false;
            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.That(subject.Data, Is.EqualTo(Vector2.zero).Using(comparer));
        }
    }
}