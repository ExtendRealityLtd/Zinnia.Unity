using Zinnia.Data.Collection.List;
using Zinnia.Data.Type.Transformation.Aggregation;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatMedianFinderTest
    {
        private GameObject containingObject;
        private FloatMedianFinder subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatMedianFinder>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformOdd()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(2f);
            subject.Collection.Add(2f);
            subject.Collection.Add(3f);
            subject.Collection.Add(5f);
            subject.Collection.Add(5f);
            subject.Collection.Add(7f);
            subject.Collection.Add(8f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(5f, result);
            Assert.AreEqual(5f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformOddUnordered()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(3f);
            subject.Collection.Add(2f);
            subject.Collection.Add(5f);
            subject.Collection.Add(2f);
            subject.Collection.Add(8f);
            subject.Collection.Add(7f);
            subject.Collection.Add(5f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(5f, result);
            Assert.AreEqual(5f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformEven()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(2f);
            subject.Collection.Add(2f);
            subject.Collection.Add(3f);
            subject.Collection.Add(4f);
            subject.Collection.Add(5f);
            subject.Collection.Add(7f);
            subject.Collection.Add(8f);
            subject.Collection.Add(9f);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(4.5f, result);
            Assert.AreEqual(4.5f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformEmptyCollection()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsTrue(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(2f);
            subject.Collection.Add(2f);
            subject.Collection.Add(3f);
            subject.Collection.Add(5f);
            subject.Collection.Add(5f);
            subject.Collection.Add(7f);
            subject.Collection.Add(8f);

            subject.gameObject.SetActive(false);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            FloatObservableList collection = containingObject.AddComponent<FloatObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(2f);
            subject.Collection.Add(2f);
            subject.Collection.Add(3f);
            subject.Collection.Add(5f);
            subject.Collection.Add(5f);
            subject.Collection.Add(7f);
            subject.Collection.Add(8f);

            subject.enabled = false;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            float result = subject.Transform();

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }
    }
}
