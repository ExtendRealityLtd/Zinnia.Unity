using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2ComponentExtractorTest
    {
        private GameObject containingObject;
        private Vector2ComponentExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector2ComponentExtractor>();
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
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;

            Assert.IsFalse(extractedListenerMock.Received);

            float result = subject.Extract().Value;

            Assert.AreEqual(1f, result);
            Assert.IsTrue(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;

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
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedListenerMock.Received);

            float? result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;

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
            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.X;
            subject.enabled = false;

            Assert.IsFalse(extractedListenerMock.Received);

            float? result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);

            extractedListenerMock.Reset();
            Assert.IsFalse(extractedListenerMock.Received);

            subject.ComponentToExtract = Vector2ComponentExtractor.Vector2Component.Y;

            result = subject.Extract();

            Assert.IsFalse(result.HasValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}