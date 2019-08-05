using Zinnia.Tracking.Follow.Operation.Extraction;
using Zinnia.Tracking.Follow;

namespace Test.Zinnia.Tracking.Follow.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class ObjectDistanceComparatorEventDataExtractorTest
    {
        private ObjectDistanceComparatorEventDataExtractor subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<ObjectDistanceComparatorEventDataExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventValueListenerMock<Vector3> differenceExtractedMock = new UnityEventValueListenerMock<Vector3>();
            subject.DifferenceExtracted.AddListener(differenceExtractedMock.Listen);
            UnityEventValueListenerMock<float> distanceExtractedMock = new UnityEventValueListenerMock<float>();
            subject.DistanceExtracted.AddListener(distanceExtractedMock.Listen);

            ObjectDistanceComparator.EventData eventData = new ObjectDistanceComparator.EventData
            {
                CurrentDifference = new Vector3(2f, 5f, 10f),
                CurrentDistance = 42.123f
            };

            Assert.IsFalse(differenceExtractedMock.Received);
            Assert.IsFalse(distanceExtractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(eventData.CurrentDifference, differenceExtractedMock.Value);
            Assert.AreEqual(eventData.CurrentDistance, distanceExtractedMock.Value);
        }
    }
}