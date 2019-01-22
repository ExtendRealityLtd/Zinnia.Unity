using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class FloatAdderTest
    {
        private GameObject containingObject;
        private FloatAdder subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatAdder>();
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
            subject.collection = new List<float>(new float[3]);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetElement(0, 1f);
            subject.SetElement(1, 2f);
            subject.CurrentIndex = 2;
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
            subject.collection = new List<float>(new float[3]);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetElement(0, 1f);
            subject.SetElement(1, 2f);
            float result = subject.Transform(2, 3f);

            Assert.AreEqual(6f, result);
            Assert.AreEqual(6f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformExceedingIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>(new float[2]);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            // adds 1 to index 0 -> adds 2 to index 1 -> attempts to add 3 to index 2 but is out of range so sets it at index 1
            // collection result is [1f, 3f]

            subject.SetElement(0, 1f);
            subject.SetElement(1, 2f);
            float result = subject.Transform(2, 3f);

            Assert.AreEqual(4f, result);
            Assert.AreEqual(4f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>(new float[2]);
            subject.gameObject.SetActive(false);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetElement(0, 1f);
            float result = subject.Transform(1, 3f);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<float>(new float[2]);
            subject.enabled = false;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.SetElement(0, 1f);
            float result = subject.Transform(1, 3f);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}
