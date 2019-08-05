using Zinnia.Data.Type.Transformation.Aggregation;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class Vector3MultiplierTest
    {
        private GameObject containingObject;
        private Vector3Multiplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector3Multiplier>();
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
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            subject.Collection.SetAt(new Vector3(3f, 4f, 5f), 0);
            subject.Collection.CurrentIndex = 1;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 4f);
            Vector3 result = subject.Transform(input);
            Vector3 expectedResult = new Vector3(6f, 12f, 20f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformPredefinedCollection()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(new Vector3(3f, 4f, 5f));
            subject.Collection.Add(new Vector3(2f, 3f, 4f));

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 result = subject.Transform();
            Vector3 expectedResult = new Vector3(6f, 12f, 20f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            subject.Collection.SetAt(new Vector3(3f, 4f, 5f), 0);
            subject.Collection.CurrentIndex = 1;

            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 4f);
            Vector3 result = subject.Transform(input);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            Vector3ObservableList collection = containingObject.AddComponent<Vector3ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector3.zero);
            subject.Collection.Add(Vector3.zero);

            subject.Collection.SetAt(new Vector3(3f, 4f, 5f), 0);
            subject.Collection.CurrentIndex = 1;

            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector3 input = new Vector3(2f, 3f, 4f);
            Vector3 result = subject.Transform(input);

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}