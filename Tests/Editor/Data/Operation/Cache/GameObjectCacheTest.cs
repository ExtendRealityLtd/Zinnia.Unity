using Zinnia.Data.Operation.Cache;

namespace Test.Zinnia.Data.Operation.Cache
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectCacheTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private GameObjectCache subject;
#pragma warning restore 0618

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
#pragma warning disable 0618
            subject = containingObject.AddComponent<GameObjectCache>();
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
            GameObject first = new GameObject();
            GameObject second = new GameObject();

            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.Data = first;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(first, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = first;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsTrue(unmodifiedListenerMock.Received);
            Assert.AreEqual(first, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = second;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(second, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.ClearCache();

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            Object.DestroyImmediate(first);
            Object.DestroyImmediate(second);
        }

        [Test]
        public void CacheChangedInactiveGameObject()
        {
            GameObject first = new GameObject();

            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.gameObject.SetActive(false);
            subject.Data = first;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            Object.DestroyImmediate(first);
        }

        [Test]
        public void CacheChangedInactiveComponent()
        {
            GameObject first = new GameObject();

            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            subject.enabled = false;
            subject.Data = first;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            Object.DestroyImmediate(first);
        }
    }
}