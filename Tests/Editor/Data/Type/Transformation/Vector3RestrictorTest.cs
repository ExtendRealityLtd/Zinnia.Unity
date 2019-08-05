using Zinnia.Data.Type;
using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3RestrictorTest
    {
        private GameObject containingObject;
        private Vector3Restrictor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector3Restrictor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClampAllCoordinates()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.XBounds = new FloatRange(-2f, 2f);
            subject.YBounds = new FloatRange(-3f, 3f);
            subject.ZBounds = new FloatRange(-2f, 2f);

            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(5f, -5f, 5f);
            Vector3 expected = new Vector3(2f, -3f, 2f);

            Vector3 result = subject.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            subject.YBounds = new FloatRange(1f, 3f);

            expected = new Vector3(2f, 1f, 2f);

            result = subject.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);

            transformedListenerMock.Reset();

            subject.YBounds = FloatRange.MinMax;

            expected = new Vector3(2f, -5f, 2f);

            result = subject.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.XBounds = new FloatRange(-2f, 2f);
            subject.YBounds = new FloatRange(-3f, 3f);
            subject.ZBounds = new FloatRange(-2f, 2f);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(5f, -5f, 5f);
            Vector3 expected = Vector3.zero;

            Vector3 result = subject.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.XBounds = new FloatRange(-2f, 2f);
            subject.YBounds = new FloatRange(-3f, 3f);
            subject.ZBounds = new FloatRange(-2f, 2f);
            subject.enabled = false;

            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(5f, -5f, 5f);
            Vector3 expected = Vector3.zero;

            Vector3 result = subject.Transform(input);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}