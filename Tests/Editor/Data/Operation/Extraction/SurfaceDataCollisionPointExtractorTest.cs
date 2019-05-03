using Zinnia.Data.Operation.Extraction;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

    public class SurfaceDataCollisionPointExtractorTest
    {
        private GameObject containingObject;
        private SurfaceDataCollisionPointExtractor subject;
        private GameObject blocker;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<SurfaceDataCollisionPointExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void Extract()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

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
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);
        }

        [Test]
        public void ExtractNoCollisionData()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);
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
            Assert.IsNull(subject.Result);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);
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
            Assert.IsNull(subject.Result);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);
        }

        /// <summary>
        /// Generates <see cref="RaycastHit"/> data to ensure the dataset is valid.
        /// </summary>
        /// <returns>The valid data.</returns>
        protected virtual RaycastHit GetRayCastData()
        {
            blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 2f;
            Physics.Raycast(Vector3.zero, Vector3.forward, out RaycastHit hitData);
            return hitData;
        }
    }
}