using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using UnityEngine;
    using NUnit.Framework;
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

            Object.DestroyImmediate(front);
            Object.DestroyImmediate(bottom);
            Physics.autoSimulation = true;
        }
    }
}