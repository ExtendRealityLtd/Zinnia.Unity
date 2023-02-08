using Zinnia.Tracking.Follow;
using Zinnia.Tracking.Follow.Operation.Extraction;

namespace Test.Zinnia.Tracking.Follow.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class ObjectDistanceComparatorEventDataExtractorTest
    {
        private GameObject containingObject;
        private ObjectDistanceComparatorEventDataExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ObjectDistanceComparatorEventDataExtractorTest");
            subject = containingObject.AddComponent<ObjectDistanceComparatorEventDataExtractor>();
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

            Assert.That(eventData.CurrentDifference, Is.EqualTo(differenceExtractedMock.Value).Using(comparer));
            Assert.That(eventData.CurrentDistance, Is.EqualTo(distanceExtractedMock.Value).Using(comparer));
        }
    }
}