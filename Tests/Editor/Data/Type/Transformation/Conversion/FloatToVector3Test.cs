using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class FloatToVector3Test
    {
        private GameObject containingObject;
        private FloatToVector3 subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("FloatToVector3");
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(2f, 3f, 4f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformWithSet()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            subject.CurrentX = 2f;
            subject.CurrentY = 3f;
            subject.CurrentZ = 4f;
            Vector3 result = subject.Transform();
            Vector3 expectedResult = new Vector3(2f, 3f, 4f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
            Vector3 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.enabled = false;

            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);

            float[] input = new float[] { 2f, 3f, 4f };
            Vector3 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}