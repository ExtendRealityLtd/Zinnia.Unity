using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

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

            Vector3? result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.one.ToString(), result.ToString());
#pragma warning disable 0618
            Assert.AreEqual(Vector3.one.ToString(), subject.LastExtractedValue.ToString());
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void Process()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;

            subject.Process();

#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            subject.Process();

#pragma warning disable 0618
            Assert.AreEqual(Vector3.one.ToString(), subject.LastExtractedValue.ToString());
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.gameObject.SetActive(false);

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.enabled = false;

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.one;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}