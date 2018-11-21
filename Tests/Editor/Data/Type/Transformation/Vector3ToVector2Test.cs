using VRTK.Core.Data.Type.Transformation;

namespace Test.VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class Vector3ToVector2Test
    {
        private GameObject containingObject;
        private Vector3ToVector2 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
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
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToXAndYToYExcludeZ);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(1f, 2f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToXExcludeZ()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToYAndYToXExcludeZ);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(2f, 1f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformYToYAndZToXExcludeX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.YToYAndZToXExcludeX);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(3f, 2f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformYToXAndZToYExcluedX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.YToXAndZToYExcluedX);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(2f, 3f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToXAndZToYExcludeY()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToXAndZToYExcludeY);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(1f, 3f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndZToXExcludeY()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToYAndZToXExcludeY);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector3(3f, 1f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToYAndZToXExcludeY);
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector3ToVector2.CoordinateMap.XToYAndZToXExcludeY);
            subject.enabled = false;

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(1f, 2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}