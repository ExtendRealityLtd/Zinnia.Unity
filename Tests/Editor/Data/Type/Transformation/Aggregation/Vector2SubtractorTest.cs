using Zinnia.Data.Collection.List;
using Zinnia.Data.Type.Transformation.Aggregation;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector2SubtractorTest
    {
        private GameObject containingObject;
        private Vector2Subtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector2Subtractor>();
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
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector2.one * 3f, 0);
            subject.Collection.SetAt(Vector2.one, 1);
            subject.Collection.CurrentIndex = 2;
            Vector2 result = subject.Transform(Vector2.one);

            Assert.AreEqual(Vector2.one, result);
            Assert.AreEqual(Vector2.one, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformWithIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector2.one * 3f, 0);
            subject.Collection.SetAt(Vector2.one, 1);
            Vector2 result = subject.Transform(Vector2.one, 2);

            Assert.AreEqual(Vector2.one, result);
            Assert.AreEqual(Vector2.one, subject.Result);
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
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector2 result = subject.Transform();

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsTrue(failedListenerMock.Received);
        }

        [Test]
        public void TransformExceedingIndex()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            // adds (3f,3f) to index 0 -> adds (1f,1f) to index 1 -> attempts to add (2f,2f) to index 2 but is out of range so sets it at index 1
            // collection result is [(3f,3f), (2f,2f)]

            subject.Collection.SetAt(Vector2.one * 3f, 0);
            subject.Collection.SetAt(Vector2.one, 1);
            Vector2 result = subject.Transform(Vector2.one * 2f, 2);

            Assert.AreEqual(Vector2.one, result);
            Assert.AreEqual(Vector2.one, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector2.one * 3f, 0);
            Vector2 result = subject.Transform(Vector2.one, 1);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
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
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            subject.enabled = false;

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector2.one * 3f, 0);
            Vector2 result = subject.Transform(Vector2.one, 1);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }
    }
}