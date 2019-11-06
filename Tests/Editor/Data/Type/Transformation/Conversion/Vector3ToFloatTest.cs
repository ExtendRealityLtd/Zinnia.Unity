using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3ToFloatTest
    {
        private GameObject containingObject;
        private Vector3ToFloat subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector3ToFloat>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformExtractX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector3ToFloat.ExtractionCoordinate.ExtractX;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 6f);
            float result = subject.Transform(input);
            float expectedResult = 2f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExtractY()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector3ToFloat.ExtractionCoordinate.ExtractY;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 6f);
            float result = subject.Transform(input);
            float expectedResult = 3f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExtractZ()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector3ToFloat.ExtractionCoordinate.ExtractZ;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 6f);
            float result = subject.Transform(input);
            float expectedResult = 6f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExtractMagnitude()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector3ToFloat.ExtractionCoordinate.ExtractMagnitude;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 6f);
            float result = subject.Transform(input);
            float expectedResult = 7f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExtractSqrMagnitude()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector3ToFloat.ExtractionCoordinate.ExtractSqrMagnitude;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 6f);
            float result = subject.Transform(input);
            float expectedResult = 49f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}