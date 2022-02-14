using Zinnia.Data.Collection.List;
using Zinnia.Data.Type.Transformation.Aggregation;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3AdderTest
    {
        private GameObject containingObject;
        private Vector3Adder subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector3Adder>();
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector3.one * 3f, 0);
            subject.Collection.SetAt(Vector3.one * 2f, 1);
            subject.Collection.CurrentIndex = 2;
            Vector3 result = subject.Transform(Vector3.one);

            Assert.AreEqual(Vector3.one * 6f, result);
            Assert.AreEqual(Vector3.one * 6f, subject.Result);
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector3.one * 3f, 0);
            subject.Collection.SetAt(Vector3.one * 2f, 1);
            Vector3 result = subject.Transform(Vector3.one, 2);

            Assert.AreEqual(Vector3.one * 6f, result);
            Assert.AreEqual(Vector3.one * 6f, subject.Result);
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector3 result = subject.Transform();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            // adds (3f,3f,3f) to index 0 -> adds (2f,2f,2f) to index 1 -> attempts to add (1f,1f,1f) to index 2 but is out of range so sets it at index 1
            // collection result is [(3f,3f,3f), (1f,1f,1f)]

            subject.Collection.SetAt(Vector3.one * 3f, 0);
            subject.Collection.SetAt(Vector3.one * 2f, 1);
            Vector3 result = subject.Transform(Vector3.one, 2);

            Assert.AreEqual(Vector3.one * 4f, result);
            Assert.AreEqual(Vector3.one * 4f, subject.Result);
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector3.one * 3f, 0);
            Vector3 result = subject.Transform(Vector3.one, 1);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
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
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            subject.Collection.SetAt(Vector3.one * 3f, 0);
            Vector3 result = subject.Transform(Vector3.one, 1);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }
    }
}