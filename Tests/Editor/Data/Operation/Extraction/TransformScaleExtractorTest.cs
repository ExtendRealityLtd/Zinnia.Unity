using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformScaleExtractorTest
    {
        private GameObject containingObject;
        private TransformScaleExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformScaleExtractor>();
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
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.one, result);
            Assert.AreEqual(Vector3.one, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.one * 2f, result);
            Assert.AreEqual(Vector3.one * 2f, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;

            subject.Process();

            Assert.AreEqual(Vector3.one, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            subject.Process();

            Assert.AreEqual(Vector3.one * 2f, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;
            subject.gameObject.SetActive(false);

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;
            subject.enabled = false;

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}