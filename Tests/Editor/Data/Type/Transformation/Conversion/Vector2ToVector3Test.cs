using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector2ToVector3Test
    {
        private GameObject containingObject;
        private Vector2ToVector3 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector2ToVector3Test");
            subject = containingObject.AddComponent<Vector2ToVector3>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformXToXAndYToY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToXAndYToY;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(1f, 2f, 0f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToXAndYToZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToXAndYToZ;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(1f, 0f, 2f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToX()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToYAndYToX;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 1f, 0f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToYAndYToZ;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(0f, 1f, 2f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToZAndYToX()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToZAndYToX;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 0f, 1f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToZAndYToY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToZAndYToY;
            subject.UnusedCoordinateValue = 5f;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(5f, 2f, 1f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToXAndYToY;
            subject.gameObject.SetActive(false);

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector2ToVector3.CoordinateMapType.XToXAndYToY;
            subject.enabled = false;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}