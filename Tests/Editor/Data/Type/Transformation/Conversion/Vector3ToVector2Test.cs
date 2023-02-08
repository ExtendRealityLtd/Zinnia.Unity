using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector3ToVector2Test
    {
        private GameObject containingObject;
        private Vector3ToVector2 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector3ToVector2Test");
            subject = containingObject.AddComponent<Vector3ToVector2>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformXToXAndYToYExcludeZ()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToXAndYToYExcludeZ;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(1f, 2f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToXExcludeZ()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToYAndYToXExcludeZ;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(2f, 1f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformYToYAndZToXExcludeX()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.YToYAndZToXExcludeX;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(3f, 2f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformYToXAndZToYExcludeX()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.YToXAndZToYExcludeX;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(2f, 3f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToXAndZToYExcludeY()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToXAndZToYExcludeY;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(1f, 3f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndZToXExcludeY()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToYAndZToXExcludeY;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(3f, 1f);

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
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToYAndZToXExcludeY;
            subject.gameObject.SetActive(false);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);

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
            subject.CoordinateMap = Vector3ToVector2.CoordinateMapType.XToYAndZToXExcludeY;
            subject.enabled = false;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}