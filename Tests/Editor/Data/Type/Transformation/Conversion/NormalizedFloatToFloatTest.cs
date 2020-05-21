using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class NormalizedFloatToFloatTest
    {
        private GameObject containingObject;
        private NormalizedFloatToFloat subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<NormalizedFloatToFloat>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformWithinBounds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetRange(new Vector2(0f, 10f));

            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float expected = 5f;
            float result = subject.Transform(0.5f);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformUnderBounds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetRange(new Vector2(1f, 10f));

            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float expected = 1f;
            float result = subject.Transform(0f);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformOverBounds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetRange(new Vector2(0f, 10f));

            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float expected = 10f;
            float result = subject.Transform(1f);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetRange(new Vector2(0f, 10f));
            subject.gameObject.SetActive(false);

            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float result = subject.Transform(0.5f);

            Assert.AreEqual(default, result);
            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetRange(new Vector2(0f, 10f));
            subject.enabled = false;

            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            float result = subject.Transform(0.5f);

            Assert.AreEqual(default, result);
            Assert.AreEqual(default, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}