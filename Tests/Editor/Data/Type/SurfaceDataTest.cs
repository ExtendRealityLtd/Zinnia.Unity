using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class SurfaceDataTest
    {
        [Test]
        public void DefaultConstructor()
        {
            SurfaceData surfaceData = new SurfaceData();
            Assert.AreEqual(Vector3.zero, surfaceData.Origin);
            Assert.AreEqual(Vector3.zero, surfaceData.Direction);
        }

        [Test]
        public void OriginConstructor()
        {
            SurfaceData surfaceData = new SurfaceData(Vector3.one, Vector3.forward);
            Assert.AreEqual(Vector3.one, surfaceData.Origin);
            Assert.AreEqual(Vector3.forward, surfaceData.Direction);
        }

        [Test]
        public void CollisionData()
        {
            Physics.autoSimulation = false;
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

            Assert.AreEqual(Vector3.zero, surfaceData.Origin);
            Assert.AreEqual(Vector3.down, surfaceData.Direction);
            Assert.AreEqual(bottom.transform, surfaceData.CollisionData.transform);
            Assert.AreEqual(front.transform, surfaceData.PreviousCollisionData.transform);

            Assert.AreEqual("{ Transform = [null] | UseLocalValues = False | PositionOverride = [null] | RotationOverride = [null] | ScaleOverride = [null] | Origin = (0.0, 0.0, 0.0) | Direction = (0.0, -1.0, 0.0) | CollisionData = { barycentricCoordinate = (1.0, 0.0, 0.0) | Collider = Cube (UnityEngine.BoxCollider) | Distance = 4.5 | Lightmap Coord = (0.0, 0.0) | Normal = (0.0, 1.0, 0.0) | Point = (0.0, -4.5, 0.0) | Rigidbody = [null] | Texture Coord = (0.0, 0.0) | Texture Coord2 = (0.0, 0.0) | Transform = Cube (UnityEngine.Transform) | Triangle Index = -1 } }", surfaceData.ToString());

            Object.DestroyImmediate(front);
            Object.DestroyImmediate(bottom);
            Physics.autoSimulation = true;
        }

        [Test]
        public void Comparison()
        {
            Transform subject = new GameObject().transform;

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