using Zinnia.Data.Operation.Extraction;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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
            Assert.IsFalse(subject.Result.HasValue);

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
            Assert.IsFalse(subject.Result.HasValue);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }

        [Test]
        public void ExtractNoCollisionData()
        {
            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;

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
            SurfaceData surfaceData = new SurfaceData();

            subject.Source = surfaceData;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
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
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsFalse(subject.Result.HasValue);
        }

        /// <summary>
        /// Generates <see cref="RaycastHit"/> data to ensure the dataset is valid.
        /// </summary>
        /// <returns>The valid data.</returns>
        protected virtual RaycastHit GetRayCastData()
        {
            blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 2f;
            Physics.autoSimulation = false;
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.Raycast(Vector3.zero, Vector3.forward, out RaycastHit hitData);
            Physics.autoSimulation = true;
            return hitData;
        }
    }
}