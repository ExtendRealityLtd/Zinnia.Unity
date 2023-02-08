using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector2MagnitudeSetterTest
    {
        private GameObject containingObject;
        private Vector2MagnitudeSetter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector2MagnitudeSetterTest");
            subject = containingObject.AddComponent<Vector2MagnitudeSetter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
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
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Magnitude = 2f;
            Vector2 input = new Vector2(1f, -2f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector2(0.8944272f, -1.788854f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}