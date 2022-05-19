using Zinnia.Cast;
using Zinnia.Cast.Operation.Extraction;

namespace Test.Zinnia.Cast.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Helper;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class PointsCastEventDataRaycastHitExtractorTest
    {
        private GameObject containingObject;
        private PointsCastEventDataRaycastHitExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointsCastEventDataRaycastHitExtractor>();

        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            PointsCast.EventData subjectData = new PointsCast.EventData();
            subjectData.HitData = hitData;
            subject.Source = subjectData;
            subject.ExtractWhen = PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>.ExtractionState.AlwaysExtract;

            Assert.IsFalse(extractedMock.Received);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(hitData, subject.Result);
        }

        [Test]
        public void ExtractIsValid()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            PointsCast.EventData subjectData = new PointsCast.EventData();
            subjectData.HitData = hitData;
            subject.Source = subjectData;
            subject.ExtractWhen = PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>.ExtractionState.OnlyWhenValid;

            Assert.IsFalse(extractedMock.Received);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreNotEqual(hitData, subject.Result);
        }

        [Test]
        public void ExtractIsNotValid()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            PointsCast.EventData subjectData = new PointsCast.EventData();
            subjectData.HitData = hitData;
            subject.Source = subjectData;
            subject.ExtractWhen = PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>.ExtractionState.OnlyWhenNotValid;

            Assert.IsFalse(extractedMock.Received);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(hitData, subject.Result);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            PointsCast.EventData subjectData = new PointsCast.EventData();
            subjectData.HitData = hitData;
            subject.Source = subjectData;
            subject.ExtractWhen = PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>.ExtractionState.AlwaysExtract;

            Assert.IsFalse(extractedMock.Received);

            subject.gameObject.SetActive(false);
            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreNotEqual(hitData, subject.Result);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            PointsCast.EventData subjectData = new PointsCast.EventData();
            subjectData.HitData = hitData;
            subject.Source = subjectData;
            subject.ExtractWhen = PointsCastEventDataExtractor<RaycastHit, PointsCastEventDataRaycastHitExtractor.UnityEvent, RaycastHit>.ExtractionState.AlwaysExtract;

            Assert.IsFalse(extractedMock.Received);

            subject.enabled = false;
            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreNotEqual(hitData, subject.Result);
        }
    }
}