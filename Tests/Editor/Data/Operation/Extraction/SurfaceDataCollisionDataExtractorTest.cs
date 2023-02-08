using Zinnia.Data.Operation.Extraction;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Helper;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class SurfaceDataCollisionDataExtractorTest
    {
        private GameObject containingObject;
        private SurfaceDataCollisionDataExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("SurfaceDataCollisionDataExtractorTest");
            subject = containingObject.AddComponent<SurfaceDataCollisionDataExtractor>();
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
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.IsNotNull(subject.Result.Value.transform);
            Assert.That(subject.Result.Value.point, Is.EqualTo(Vector3.one).Using(comparer));
        }

        [Test]
        public void ExtractInvalidSource()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [UnityTest]
        public IEnumerator ExtractNoCollisionData()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            yield return null;

            SurfaceData surfaceData = new SurfaceData();
            GameObject blocker = RaycastHitHelper.CreateBlocker("SurfaceDataCollisionDataExtractorTest");
            blocker.SetActive(false);
            RaycastHit hitData = RaycastHitHelper.GetRaycastHit(blocker);
            surfaceData.CollisionData = hitData;

            subject.Source = surfaceData;

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [Test]
        public void ExtractInactiveGameObject()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [Test]
        public void ExtractInactiveComponent()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;
            subject.enabled = false;

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = RaycastHitHelper.GetRaycastHit();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.AreEqual(null, subject.Result);
            Assert.IsFalse(subject.Result.HasValue);
        }
    }
}