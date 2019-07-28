using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3MagnitudeSetterTest
    {
        private Vector3MagnitudeSetter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<Vector3MagnitudeSetter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void SetVectorMagnitude()
        {
            subject.Magnitude = 2f;

            Assert.AreEqual(2f, subject.Magnitude);

            Vector3 expected = new Vector3(1f, 2f, 3f);
            subject.SetMagnitude(expected);

            Assert.AreEqual(expected.magnitude, subject.Magnitude);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Magnitude = 2f;
            Vector3 input = new Vector3(1f, -2f, 5f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(0.3651484f, -0.7302967f, 1.825742f);

            Assert.IsTrue(expectedResult == result);
            Assert.IsTrue(expectedResult == subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}