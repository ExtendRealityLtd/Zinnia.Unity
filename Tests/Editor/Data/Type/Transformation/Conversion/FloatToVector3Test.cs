using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatToVector3Test
    {
        private GameObject containingObject;
        private FloatToVector3 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatToVector3>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Transform()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 3f, 4f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformWithSet()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.CurrentX = 2f;
            subject.CurrentY = 3f;
            subject.CurrentZ = 4f;
            Vector3 result = subject.Transform();
            Vector3 expectedResult = new Vector3(2f, 3f, 4f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
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
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
            Vector3 result = subject.Transform(input);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}