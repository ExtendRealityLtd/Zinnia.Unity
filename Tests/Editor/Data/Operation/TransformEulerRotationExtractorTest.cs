﻿using Zinnia.Data.Operation;

namespace Test.Zinnia.Data.Operation
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class TransformEulerRotationExtractorTest
    {
        private GameObject containingObject;
        private TransformEulerRotationExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformEulerRotationExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.one.ToString(), result.ToString());
            Assert.AreEqual(Vector3.one.ToString(), subject.LastExtractedValue.ToString());
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;

            subject.Process();

            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            subject.Process();

            Assert.AreEqual(Vector3.one.ToString(), subject.LastExtractedValue.ToString());
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.gameObject.SetActive(false);

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.enabled = false;

            Vector3 result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}