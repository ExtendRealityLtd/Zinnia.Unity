using Zinnia.Data.Operation.Extraction;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class SurfaceDataCollisionPointExtractorTest
    {
        private GameObject containingObject;
#pragma warning disable 0618
        private SurfaceDataCollisionPointExtractor subject;
#pragma warning restore 0618
        private GameObject blocker;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("SurfaceDataCollisionPointExtractorTest");
#pragma warning disable 0618
            subject = containingObject.AddComponent<SurfaceDataCollisionPointExtractor>();
#pragma warning restore 0618
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
            Assert.IsFalse(subject.Result.HasValue);

            RaycastHit hitData = GetRayCastData();
            hitData.point = Vector3.one;
            surfaceData.CollisionData = hitData;

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.That(subject.Result, Is.EqualTo(Vector3.one).Using(comparer));
            Object.DestroyImmediate(blocker);
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
            Object.DestroyImmediate(blocker);
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
            Object.DestroyImmediate(blocker);
        }

        /// <summary>
        /// Generates <see cref="RaycastHit"/> data to ensure the dataset is valid.
        /// </summary>
        /// <returns>The valid data.</returns>
        protected virtual RaycastHit GetRayCastData()
        {
            blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.name = "SurfaceDataCollisionPointExtractorTest_Blocker";
            blocker.transform.position = Vector3.forward * 2f;
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.Raycast(Vector3.zero, Vector3.forward, out RaycastHit hitData);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.FixedUpdate;
#else
            Physics.autoSimulation = true;
#endif
            return hitData;
        }
    }
}