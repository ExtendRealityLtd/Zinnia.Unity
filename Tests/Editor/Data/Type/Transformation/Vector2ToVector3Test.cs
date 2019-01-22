using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class Vector2ToVector3Test
    {
        private GameObject containingObject;
        private Vector2ToVector3 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
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
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToXAndYToY);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(1f, 2f, 0f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToXAndYToZ()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToXAndYToZ);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(1f, 0f, 2f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToYAndYToX);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 1f, 0f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToYAndYToZ()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToYAndYToZ);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(0f, 1f, 2f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToZAndYToX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToZAndYToX);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 0f, 1f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformXToZAndYToY()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToZAndYToY);
            subject.SetUnusedCoordinateValue(5f);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(5f, 2f, 1f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToXAndYToY);
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetCoordinateMap(Vector2ToVector3.CoordinateMap.XToXAndYToY);
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(1f, 2f);
            Vector3 result = subject.Transform(input);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}