using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class AngleToVector2DirectionTest
    {
        private GameObject containingObject;
        private AngleToVector2Direction subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
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
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 result = subject.Transform(5f);
            Vector2 expectedResult = new Vector2(1f, 0.1f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(5f);
            expectedResult = new Vector2(1f, 0.2f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(10f);
            expectedResult = new Vector2(1f, 0.4f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(-10f);
            expectedResult = new Vector2(1f, 0.2f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(35f);
            expectedResult = new Vector2(1f, 1f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(-45f);
            expectedResult = new Vector2(1f, 0f);

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.gameObject.SetActive(false);
            Vector2 result = subject.Transform(5f);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.enabled = false;
            Vector2 result = subject.Transform(5f);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}
