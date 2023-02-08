using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector3MagnitudeSetterTest
    {
        private GameObject containingObject;
        private Vector3MagnitudeSetter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector3MagnitudeSetterTest");
            subject = containingObject.AddComponent<Vector3MagnitudeSetter>();
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

            Vector3 expected = new Vector3(1f, 2f, 3f);
            subject.SetMagnitude(expected);

            Assert.AreEqual(expected.magnitude, subject.Magnitude);
        }

        [Test]
        public void Process()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Magnitude = 2f;
            Vector3 input = new Vector3(1f, -2f, 5f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(0.3651484f, -0.7302967f, 1.825742f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}