using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class SurfaceDataTest
    {
        [Test]
        public void DefaultConstructor()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            SurfaceData surfaceData = new SurfaceData();
            Assert.That(surfaceData.Origin, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(surfaceData.Direction, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void OriginConstructor()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            SurfaceData surfaceData = new SurfaceData(Vector3.one, Vector3.forward);
            Assert.That(surfaceData.Origin, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(surfaceData.Direction, Is.EqualTo(Vector3.forward).Using(comparer));
        }

        [Test]
        public void CollisionData()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif
            SurfaceData surfaceData = new SurfaceData();

            //Create a couple of GameObjects for collision detection
            GameObject front = GameObject.CreatePrimitive(PrimitiveType.Cube);
            front.transform.position = Vector3.forward * 5f;
            GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottom.transform.position = Vector3.down * 5f;

            //Do an initial collision
            Ray forwardRay = new Ray(Vector3.zero, Vector3.forward);
            RaycastHit forwardHit;
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.Raycast(forwardRay, out forwardHit);

            //Set up the initial collision of a surface
            surfaceData.Origin = Vector3.zero;
            surfaceData.Direction = Vector3.forward;
            surfaceData.CollisionData = forwardHit;

            //Do an second different collision
            Ray downwardRay = new Ray(Vector3.zero, Vector3.down);
            RaycastHit downwardHit;
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.Raycast(downwardRay, out downwardHit);

            //Set up the initial collision of a surface
            surfaceData.Origin = Vector3.zero;
            surfaceData.Direction = Vector3.down;
            surfaceData.CollisionData = downwardHit;

            Assert.That(surfaceData.Origin, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(surfaceData.Direction, Is.EqualTo(Vector3.down).Using(comparer));
            Assert.AreEqual(bottom.transform, surfaceData.CollisionData.transform);
            Assert.AreEqual(front.transform, surfaceData.PreviousCollisionData.transform);

            Object.DestroyImmediate(front);
            Object.DestroyImmediate(bottom);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.FixedUpdate;
#else
            Physics.autoSimulation = true;
#endif
        }

        [Test]
        public void Comparison()
        {
            Transform subject = new GameObject("SurfaceDataTest").transform;

            SurfaceData subjectA = new SurfaceData(subject);
            SurfaceData subjectB = new SurfaceData(subject);

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsTrue(subjectA.Equals(subjectB));
            Assert.AreEqual(subjectA, subjectB);

            subjectA.Origin = Vector3.zero;
            subjectB.Origin = Vector3.one;

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsFalse(subjectA.Equals(subjectB));
            Assert.AreNotEqual(subjectA, subjectB);

            Object.DestroyImmediate(subject.gameObject);
        }
    }
}