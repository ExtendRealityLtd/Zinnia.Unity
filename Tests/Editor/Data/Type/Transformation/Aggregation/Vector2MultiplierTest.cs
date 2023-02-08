using Zinnia.Data.Collection.List;
using Zinnia.Data.Type.Transformation.Aggregation;

namespace Test.Zinnia.Data.Type.Transformation.Aggregation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class Vector2MultiplierTest
    {
        private GameObject containingObject;
        private Vector2Multiplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("Vector2MultiplierTest");
            subject = containingObject.AddComponent<Vector2Multiplier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Transform()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            subject.SetComponentX(3f, 0);
            subject.SetComponentY(4f, 0);
            subject.Collection.CurrentIndex = 1;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector2(6f, 12f);

            Assert.That(result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(expectedResult).Using(comparer));
            Assert.IsTrue(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformEmptyCollection()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector2 result = subject.Transform();

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsTrue(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            subject.SetComponentX(3f, 0);
            subject.SetComponentY(4f, 0);
            subject.Collection.CurrentIndex = 1;

            subject.gameObject.SetActive(false);

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            Vector2EqualityComparer comparer = new Vector2EqualityComparer(0.1f);
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock failedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Failed.AddListener(failedListenerMock.Listen);
            Vector2ObservableList collection = containingObject.AddComponent<Vector2ObservableList>();
            subject.Collection = collection;
            subject.Collection.Add(Vector2.zero);
            subject.Collection.Add(Vector2.zero);

            subject.SetComponentX(3f, 0);
            subject.SetComponentY(4f, 0);
            subject.Collection.CurrentIndex = 1;

            subject.enabled = false;

            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.That(result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.That(subject.Result, Is.EqualTo(Vector2.zero).Using(comparer));
            Assert.IsFalse(transformedListenerMock.Received);
            Assert.IsFalse(failedListenerMock.Received);
        }
    }
}