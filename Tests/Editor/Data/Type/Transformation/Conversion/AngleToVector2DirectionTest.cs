using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class AngleToVector2DirectionTest
    {
        private GameObject containingObject;
        private AngleToVector2Direction subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("AngleToVector2DirectionTest");
            subject = containingObject.AddComponent<AngleToVector2Direction>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Transform()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 result = subject.Transform(5f);
            Vector2 expectedResult = new Vector2(1f, 0.087f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(5f);
            expectedResult = new Vector2(1f, 0.176f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(10f);
            expectedResult = new Vector2(1f, 0.4f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(-10f);
            expectedResult = new Vector2(1f, 0.176f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(35f);
            expectedResult = new Vector2(1f, 1f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(-45f);
            expectedResult = new Vector2(1f, 0f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.gameObject.SetActive(false);
            Vector2 result = subject.Transform(5f);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.enabled = false;
            Vector2 result = subject.Transform(5f);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}
