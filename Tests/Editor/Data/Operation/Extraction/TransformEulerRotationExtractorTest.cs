﻿using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformEulerRotationExtractorTest
    {
        private GameObject containingObject;
        private TransformEulerRotationExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformEulerRotationExtractorTest");
            subject = containingObject.AddComponent<TransformEulerRotationExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;

            Vector3? result = subject.Extract();

            Assert.That(result, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.That(result, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void Process()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;

            subject.Process();

#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            subject.Process();

#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.one).Using(comparer));
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.gameObject.SetActive(false);

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.enabled = false;

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.That(subject.LastExtractedValue, Is.EqualTo(Vector3.zero).Using(comparer));
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}