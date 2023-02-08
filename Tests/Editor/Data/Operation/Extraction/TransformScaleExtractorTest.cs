using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformScaleExtractorTest
    {
        private GameObject containingObject;
        private TransformScaleExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformScaleExtractorTest");
            subject = containingObject.AddComponent<TransformScaleExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;

            Vector3? result = subject.Extract();

            Assert.That(result, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.That(result, Is.EqualTo(Vector3.one * 2f).Using(comparer));
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one * 2f).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void Process()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;

            subject.Process();

#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            subject.Process();

#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one * 2f).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;
            subject.gameObject.SetActive(false);

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = true;
            subject.enabled = false;

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.localScale = Vector3.one * 2f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}