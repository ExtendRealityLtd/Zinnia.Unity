using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformDirectionExtractorTest
    {
        private GameObject containingObject;
        private TransformDirectionExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformDirectionExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ExtractRight()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Right;

            containingObject.transform.eulerAngles = Vector3.up * 45f;
            Vector3 result = subject.Extract();

            Vector3 expectedResult = new Vector3(0.7f, 0f, -0.7f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractUp()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Up;

            containingObject.transform.eulerAngles = Vector3.forward * 45f;
            Vector3 result = subject.Extract();

            Vector3 expectedResult = new Vector3(-0.7f, 0.7f, 0f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractForward()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Forward;

            containingObject.transform.eulerAngles = Vector3.up * 45f;
            Vector3 result = subject.Extract();

            Vector3 expectedResult = new Vector3(0.7f, 0f, 0.7f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
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

            containingObject.transform.eulerAngles = Vector3.up * 45f;
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

            containingObject.transform.eulerAngles = Vector3.up * 45f;
            extractedListenerMock.Reset();

            Assert.IsFalse(extractedListenerMock.Received);

            result = subject.Extract();

            Assert.AreEqual(Vector3.zero, result);
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
            Assert.IsFalse(extractedListenerMock.Received);
        }
    }
}
