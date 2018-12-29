using VRTK.Core.Data.Type.Transformation;

namespace Test.VRTK.Core.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using UnityEngine;

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

        [TestCase(0f), TestCase(1f), TestCase(-2f), TestCase(123f), TestCase(float.MaxValue)]
        public void SetFloatMagnitude(float expected)
        {
            subject.magnitude = 2f;

            Assert.AreEqual(2f, subject.magnitude);

            subject.SetMagnitude(expected);

            Assert.AreEqual(expected, subject.magnitude);
        }

        [Test]
        public void SetVectorMagnitude()
        {
            subject.magnitude = 2f;

            Assert.AreEqual(2f, subject.magnitude);

            Vector3 expected = new Vector3(1f, 2f, 3f);
            subject.SetMagnitude(expected);

            Assert.AreEqual(expected.magnitude, subject.magnitude);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.magnitude = 2f;
            Vector3 input = new Vector3(1f, -2f, 5f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(0.3651484f, -0.7302967f, 1.825742f);

            Assert.IsTrue(expectedResult == result);
            Assert.IsTrue(expectedResult == subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}