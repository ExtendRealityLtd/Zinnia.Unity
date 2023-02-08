using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class FloatToVector2Test
    {
        private GameObject containingObject;
        private FloatToVector2 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("FloatToVector2Test");
            subject = containingObject.AddComponent<FloatToVector2>();
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
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f };
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector2(2f, 3f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));

            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformWithSet()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.CurrentX = 2f;
            subject.CurrentY = 3f;
            Vector2 result = subject.Transform();
            Vector2 expectedResult = new Vector2(2f, 3f);

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
            subject.gameObject.SetActive(false);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f };
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
            subject.enabled = false;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f };
            Vector2 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}