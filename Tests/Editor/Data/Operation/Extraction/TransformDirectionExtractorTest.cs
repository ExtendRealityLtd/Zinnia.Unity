using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
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
            subject.UseLocal = false;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Right;

            containingObject.transform.eulerAngles = Vector3.up * 45f;
            Vector3? result = subject.Extract();

            Vector3 expectedResult = new Vector3(0.7f, 0f, -0.7f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
#pragma warning disable 0618
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractUp()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = false;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Up;

            containingObject.transform.eulerAngles = Vector3.forward * 45f;
            Vector3? result = subject.Extract();

            Vector3 expectedResult = new Vector3(-0.7f, 0.7f, 0f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
#pragma warning disable 0618
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractForward()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = false;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Forward;

            containingObject.transform.eulerAngles = Vector3.up * 45f;
            Vector3? result = subject.Extract();

            Vector3 expectedResult = new Vector3(0.7f, 0f, 0.7f);
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
#pragma warning disable 0618
            Assert.AreEqual(expectedResult.ToString(), subject.LastExtractedValue.ToString());
#pragma warning restore 0618
            Assert.IsTrue(extractedListenerMock.Received);
        }

        [Test]
        public void ExtractForwardGlobalAsChild()
        {
            GameObject parent = new GameObject("parent");
            GameObject child = new GameObject("child");

            child.transform.SetParent(parent.transform);

            parent.transform.localEulerAngles = Vector3.up * 180f;
            child.transform.localEulerAngles = Vector3.up * 180f;

            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = child;
            subject.UseLocal = false;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Forward;
            Vector3? result = subject.Extract();

            Vector3 expectedResult = Vector3.forward;
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.IsTrue(extractedListenerMock.Received);

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractForwardLocalAsChild()
        {
            GameObject parent = new GameObject("parent");
            GameObject child = new GameObject("child");

            child.transform.SetParent(parent.transform);

            parent.transform.localEulerAngles = Vector3.up * 180f;
            child.transform.localEulerAngles = Vector3.up * 180f;

            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = child;
            subject.UseLocal = true;
            subject.Direction = TransformDirectionExtractor.AxisDirection.Forward;
            Vector3? result = subject.Extract();

            Vector3 expectedResult = Vector3.back;
            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            Assert.IsTrue(extractedListenerMock.Received);

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedListenerMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedListenerMock.Listen);
            subject.Source = containingObject;
            subject.UseLocal = false;
            subject.gameObject.SetActive(false);

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.up * 45f;
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
            subject.UseLocal = false;
            subject.enabled = false;

            Vector3? result = subject.Extract();

            Assert.AreEqual(null, result);
#pragma warning disable 0618
            Assert.AreEqual(Vector3.zero, subject.LastExtractedValue);
#pragma warning restore 0618
            Assert.IsFalse(extractedListenerMock.Received);

            containingObject.transform.eulerAngles = Vector3.up * 45f;
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
