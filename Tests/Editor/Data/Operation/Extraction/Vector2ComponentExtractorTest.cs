using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2ComponentExtractorTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private Vector2ComponentExtractor subject;
#pragma warning restore 0618

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
#pragma warning disable 0618
            subject = containingObject.AddComponent<Vector2ComponentExtractor>();
#pragma warning restore 0618
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = new Vector2(1f, 2f);
#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;
#pragma warning restore 0618

            Assert.IsFalse(extractedListenerMock.Received);

            float result = subject.Extract().Value;

            Assert.AreEqual(1f, result);
            Assert.IsTrue(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;
#pragma warning restore 0618

            result = subject.Extract().Value;

            Assert.AreEqual(2f, result);
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = new Vector2(1f, 2f);
#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;
#pragma warning restore 0618
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedListenerMock.Received);

            float? result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;
#pragma warning restore 0618

            result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = new Vector2(1f, 2f);
#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;
#pragma warning restore 0618
            subject.enabled = false;

            Assert.IsFalse(extractedListenerMock.Received);

            float? result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

#pragma warning disable 0618
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;
#pragma warning restore 0618

            result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}