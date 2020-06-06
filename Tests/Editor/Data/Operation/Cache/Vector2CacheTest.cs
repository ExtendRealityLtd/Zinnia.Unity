using Zinnia.Data.Operation.Cache;

namespace Test.Zinnia.Data.Operation.Cache
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2CacheTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private Vector2Cache subject;
#pragma warning restore 0618

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
#pragma warning disable 0618
            subject = containingObject.AddComponent<Vector2Cache>();
#pragma warning restore 0618
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CacheChanged()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.Data = Vector2.one;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(Vector2.one, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsTrue(unmodifiedListenerMock.Received);
            Assert.AreEqual(Vector2.one, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = Vector2.one * 2f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(Vector2.one * 2f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.ClearCache();

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);
        }

        [Test]
        public void CacheChangedInactiveGameObject()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.gameObject.SetActive(false);
            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);
        }

        [Test]
        public void CacheChangedInactiveComponent()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.enabled = false;
            subject.Data = Vector2.one;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);
        }
    }
}