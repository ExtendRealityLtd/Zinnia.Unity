using Zinnia.Tracking.Modification.Operation.Extraction;
using Zinnia.Data.Type;
using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformPropertyApplierEventDataExtractorTest
    {
        private GameObject containingObject;
        private TransformPropertyApplierEventDataExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformPropertyApplierEventDataExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock sourceExtractedMock = new UnityEventListenerMock();
            UnityEventListenerMock targetExtractedMock = new UnityEventListenerMock();
            subject.SourceExtracted.AddListener(sourceExtractedMock.Listen);
            subject.TargetExtracted.AddListener(targetExtractedMock.Listen);

            TransformData sourceData = new TransformData();
            TransformData targetData = new TransformData();
            TransformPropertyApplier.EventData eventData = new TransformPropertyApplier.EventData();
            eventData.Set(sourceData, targetData);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);

            subject.Extract(eventData);

            Assert.IsTrue(sourceExtractedMock.Received);
            Assert.IsTrue(targetExtractedMock.Received);
            Assert.AreEqual(sourceData, subject.SourceResult);
            Assert.AreEqual(targetData, subject.TargetResult);
        }

        [Test]
        public void ExtractNull()
        {
            UnityEventListenerMock sourceExtractedMock = new UnityEventListenerMock();
            UnityEventListenerMock targetExtractedMock = new UnityEventListenerMock();
            subject.SourceExtracted.AddListener(sourceExtractedMock.Listen);
            subject.TargetExtracted.AddListener(targetExtractedMock.Listen);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);

            subject.Extract(null);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock sourceExtractedMock = new UnityEventListenerMock();
            UnityEventListenerMock targetExtractedMock = new UnityEventListenerMock();
            subject.SourceExtracted.AddListener(sourceExtractedMock.Listen);
            subject.TargetExtracted.AddListener(targetExtractedMock.Listen);

            TransformData sourceData = new TransformData();
            TransformData targetData = new TransformData();
            TransformPropertyApplier.EventData eventData = new TransformPropertyApplier.EventData();
            eventData.Set(sourceData, targetData);

            subject.gameObject.SetActive(false);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);

            subject.Extract(eventData);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock sourceExtractedMock = new UnityEventListenerMock();
            UnityEventListenerMock targetExtractedMock = new UnityEventListenerMock();
            subject.SourceExtracted.AddListener(sourceExtractedMock.Listen);
            subject.TargetExtracted.AddListener(targetExtractedMock.Listen);

            TransformData sourceData = new TransformData();
            TransformData targetData = new TransformData();
            TransformPropertyApplier.EventData eventData = new TransformPropertyApplier.EventData();
            eventData.Set(sourceData, targetData);

            subject.enabled = false;

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);

            subject.Extract(eventData);

            Assert.IsFalse(sourceExtractedMock.Received);
            Assert.IsFalse(targetExtractedMock.Received);
            Assert.IsNull(subject.SourceResult);
            Assert.IsNull(subject.TargetResult);
        }
    }
}