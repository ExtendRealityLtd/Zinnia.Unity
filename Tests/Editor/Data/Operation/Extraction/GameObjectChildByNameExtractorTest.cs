using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    using Object = UnityEngine.Object;

    public class GameObjectChildByNameExtractorTest
    {
        private GameObject containingObject;
        private GameObjectChildByNameExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectChildByNameExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            GameObject source = new GameObject("parent");
            GameObject child = new GameObject("child");
            child.transform.SetParent(source.transform);

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            UnityEventListenerMock failedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.Failed.AddListener(failedMock.Listen);

            subject.ChildNamePath = "child";

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);

            GameObject result = subject.Extract(source);

            Assert.IsTrue(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);
            Assert.AreEqual(subject.Result, child);
            Assert.AreEqual(result, child);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void ExtractInvalidResultNotFound()
        {
            GameObject source = new GameObject("parent");
            GameObject child = new GameObject("child");
            child.transform.SetParent(source.transform);

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            UnityEventListenerMock failedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.Failed.AddListener(failedMock.Listen);

            subject.ChildNamePath = "another";

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);

            GameObject result = subject.Extract(source);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsTrue(failedMock.Received);
            Assert.IsNull(subject.Result);
            Assert.IsNull(result);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void ExtractInvalidResultNotExists()
        {
            GameObject source = new GameObject("parent");

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            UnityEventListenerMock failedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.Failed.AddListener(failedMock.Listen);

            subject.ChildNamePath = "child";

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);

            GameObject result = subject.Extract(source);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsTrue(failedMock.Received);
            Assert.IsNull(subject.Result);
            Assert.IsNull(result);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            GameObject source = new GameObject("parent");
            GameObject child = new GameObject("child");
            child.transform.SetParent(source.transform);

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            UnityEventListenerMock failedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.Failed.AddListener(failedMock.Listen);

            subject.ChildNamePath = "child";
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);

            GameObject result = subject.Extract(source);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);
            Assert.IsNull(subject.Result);
            Assert.IsNull(result);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            GameObject source = new GameObject("parent");
            GameObject child = new GameObject("child");
            child.transform.SetParent(source.transform);

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            UnityEventListenerMock failedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            subject.Failed.AddListener(failedMock.Listen);

            subject.ChildNamePath = "child";
            subject.enabled = false;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);

            GameObject result = subject.Extract(source);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(failedMock.Received);
            Assert.IsNull(subject.Result);
            Assert.IsNull(result);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(child);
        }
    }
}