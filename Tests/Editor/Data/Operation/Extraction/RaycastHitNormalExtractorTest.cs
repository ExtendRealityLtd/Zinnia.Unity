using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Helper;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class RaycastHitNormalExtractorTest
    {
        private GameObject containingObject;
        private RaycastHitNormalExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<RaycastHitNormalExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            hitData.normal = Vector3.one;
            subject.Source = hitData;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(Vector3.one, subject.Result);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ExtractInvalidSource()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [UnityTest]
        public IEnumerator ExtractNoCollisionData()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            yield return null;

            GameObject blocker = RaycastHitHelper.CreateBlocker();
            blocker.SetActive(false);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit(blocker);

            subject.Source = hitData;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            hitData.normal = Vector3.one;
            subject.Source = hitData;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();

            subject.Source = hitData;
            subject.enabled = false;
            hitData.normal = Vector3.one;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }
    }
}