using VRTK.Core.Data.Type.Transformation;

namespace Test.VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class FloatCollectionAggregatorTest
    {
        private GameObject containingObject;
        private FloatCollectionAggregator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatCollectionAggregator>();
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
            subject.collection = new List<float>() { 0f, 0f, 0f };

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetIndex(0);
            subject.Transform(1f);
            subject.SetIndex(1);
            subject.Transform(2f);
            subject.SetIndex(2);
            float result = subject.Transform(3f);

            Assert.AreEqual(6f, result);
            Assert.AreEqual(6f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformWithIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>() { 0f, 0f, 0f };

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Transform(1f, 0);
            subject.Transform(2f, 1);
            float result = subject.Transform(3f, 2);

            Assert.AreEqual(6f, result);
            Assert.AreEqual(6f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExceedingIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>() { 0f, 0f };

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            // adds 1 to index 0 -> adds 2 to index 1 -> attempts to add 3 to index 2 but is out of range so sets it at index 1
            // collection result is [1f, 3f]

            subject.SetIndex(0);
            subject.Transform(1f);
            subject.SetIndex(1);
            subject.Transform(2f);
            subject.SetIndex(2);
            float result = subject.Transform(3f);

            Assert.AreEqual(4f, result);
            Assert.AreEqual(4f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>() { 0f, 0f };
            subject.gameObject.SetActive(false);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetIndex(0);
            subject.Transform(1f);
            subject.SetIndex(1);
            float result = subject.Transform(2f);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>() { 0f, 0f };
            subject.enabled = false;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetIndex(0);
            subject.Transform(1f);
            subject.SetIndex(1);
            float result = subject.Transform(2f);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}
