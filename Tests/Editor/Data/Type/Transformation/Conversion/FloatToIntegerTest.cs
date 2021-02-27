using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatToIntegerTest
    {
        private GameObject containingObject;
        private FloatToInteger subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatToInteger>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformRound()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(0, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.RoundBy = FloatToInteger.RoundingType.Round;

            int result = subject.Transform(1.3f);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(1.7f);

            Assert.AreEqual(2, result);
            Assert.AreEqual(2, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFloor()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(0, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.RoundBy = FloatToInteger.RoundingType.Floor;

            int result = subject.Transform(1.3f);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(1.7f);

            Assert.AreEqual(1, result);
            Assert.AreEqual(1, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformCeil()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(0, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.RoundBy = FloatToInteger.RoundingType.Ceil;

            int result = subject.Transform(1.3f);

            Assert.AreEqual(2, result);
            Assert.AreEqual(2, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            result = subject.Transform(1.7f);

            Assert.AreEqual(2, result);
            Assert.AreEqual(2, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            int result = subject.Transform(1.3f);

            Assert.AreEqual(0, result);
            Assert.AreEqual(0, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.enabled = false;

            int result = subject.Transform(1.3f);

            Assert.AreEqual(0, result);
            Assert.AreEqual(0, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}