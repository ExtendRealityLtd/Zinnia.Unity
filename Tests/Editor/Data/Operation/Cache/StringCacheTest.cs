using Zinnia.Data.Operation.Cache;

namespace Test.Zinnia.Data.Operation.Cache
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class StringCacheTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private StringCache subject;
#pragma warning restore 0618

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
#pragma warning disable 0618
            subject = containingObject.AddComponent<StringCache>();
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

            subject.Data = "string";

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual("string", subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = "string";

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsTrue(unmodifiedListenerMock.Received);
            Assert.AreEqual("string", subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();

            subject.Data = "changed";

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual("changed", subject.Data);

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
            subject.Data = "string";

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
            subject.Data = "string";

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);
        }
    }
}