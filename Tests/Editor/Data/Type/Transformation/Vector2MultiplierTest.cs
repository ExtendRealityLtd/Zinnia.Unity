using VRTK.Core.Data.Type.Transformation;

namespace Test.VRTK.Core.Data.Type.Transformation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using System.Collections.Generic;

    public class Vector2MultiplierTest
    {
        private GameObject containingObject;
        private Vector2Multiplier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Vector2Multiplier>();
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
            subject.collection = new List<Vector2>(new Vector2[2]);
            subject.SetElementX(0, 3f);
            subject.SetElementY(0, 4f);
            subject.CurrentIndex = 1;

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);
            Vector2 expectedResult = new Vector2(6f, 12f);

            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(expectedResult, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<Vector2>(new Vector2[2]);
            subject.SetElementX(0, 3f);
            subject.SetElementY(0, 4f);
            subject.CurrentIndex = 1;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.collection = new List<Vector2>(new Vector2[2]);
            subject.SetElementX(0, 3f);
            subject.SetElementY(0, 4f);
            subject.CurrentIndex = 1;
            subject.enabled = false;

            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            Vector2 input = new Vector2(2f, 3f);
            Vector2 result = subject.Transform(input);

            Assert.AreEqual(Vector2.zero, result);
            Assert.AreEqual(Vector2.zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}