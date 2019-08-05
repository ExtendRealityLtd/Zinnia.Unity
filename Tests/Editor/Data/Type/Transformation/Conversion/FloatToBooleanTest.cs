using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatToBooleanTest
    {
        private GameObject containingObject;
        private FloatToBoolean subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatToBoolean>();
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
            subject.SetPositiveBounds(new Vector2(0.3f, 0.8f));

            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            bool result = subject.Transform(0.5f);

            Assert.IsTrue(result);
            Assert.IsTrue(subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformUnderBounds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetPositiveBounds(new Vector2(0.3f, 0.8f));

            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            bool result = subject.Transform(0.2f);

            Assert.IsFalse(result);
            Assert.IsFalse(subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformOverBounds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetPositiveBounds(new Vector2(0.3f, 0.8f));

            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            bool result = subject.Transform(1f);

            Assert.IsFalse(result);
            Assert.IsFalse(subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetPositiveBounds(new Vector2(0.3f, 0.8f));
            subject.gameObject.SetActive(false);

            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            bool result = subject.Transform(0.5f);

            Assert.IsFalse(result);
            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.SetPositiveBounds(new Vector2(0.3f, 0.8f));
            subject.enabled = false;

            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            bool result = subject.Transform(0.5f);

            Assert.IsFalse(result);
            Assert.IsFalse(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}