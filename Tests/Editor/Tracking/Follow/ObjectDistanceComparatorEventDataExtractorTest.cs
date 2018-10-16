using VRTK.Core.Tracking.Follow;

namespace Test.VRTK.Core.Tracking.Follow
{
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using UnityEngine;

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
            Vector3UnityEventListenerMock differenceExtractedMock = new Vector3UnityEventListenerMock();
            subject.DifferenceExtracted.AddListener(differenceExtractedMock.Listen);
            FloatUnityEventListenerMock distanceExtractedMock = new FloatUnityEventListenerMock();
            subject.DistanceExtracted.AddListener(distanceExtractedMock.Listen);

            ObjectDistanceComparator.EventData eventData = new ObjectDistanceComparator.EventData
            {
                difference = new Vector3(2f, 5f, 10f),
                distance = 42.123f
            };

            Assert.IsFalse(differenceExtractedMock.Received);
            Assert.IsFalse(distanceExtractedMock.Received);

            subject.Extract(eventData);

            Assert.AreEqual(eventData.difference, differenceExtractedMock.value);
            Assert.AreEqual(eventData.distance, distanceExtractedMock.value);
        }

        private class Vector3UnityEventListenerMock : UnityEventListenerMock
        {
            public Vector3 value;

            public void Listen(Vector3 value)
            {
                this.value = value;
            }
        }

        private class FloatUnityEventListenerMock : UnityEventListenerMock
        {
            public float value;

            public void Listen(float value)
            {
                this.value = value;
            }
        }
    }
}