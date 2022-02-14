using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2MagnitudeSetterTest
    {
        private Vector2MagnitudeSetter subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<Vector2MagnitudeSetter>();
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

            Vector2 expected = new Vector2(1f, 2f);
            subject.SetMagnitude(expected);

            Assert.AreEqual(expected.magnitude, subject.Magnitude);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Magnitude = 2f;
            Vector2 input = new Vector2(1f, -2f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector2(0.8944272f, -1.788854f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}