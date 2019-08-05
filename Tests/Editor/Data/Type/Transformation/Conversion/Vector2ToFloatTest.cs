using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2ToFloatTest : MonoBehaviour
    {
        private GameObject containingObject;
        private Vector2ToFloat subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector2ToFloat>();
        }

        [TearDown]
        public void TearDown()
        {
            DestroyImmediate(subject);
            DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformExtractX()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector2ToFloat.ExtractionCoordinate.ExtractX;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector3(1f, 2f);
            float result = subject.Transform(input);
            float expectedResult = 1f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExtractY()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.CoordinateToExtract = Vector2ToFloat.ExtractionCoordinate.ExtractY;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector3(1f, 2f);
            float result = subject.Transform(input);
            float expectedResult = 2f;

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}