using VRTK.Core.Data.Type;

namespace Test.VRTK.Core.Data.Type
{
    using UnityEngine;
    using NUnit.Framework;

    public class SurfaceDataTest
    {
        [Test]
        public void DefaultConstructor()
        {
            SurfaceData surfaceData = new SurfaceData();
            Assert.AreEqual(Vector3.zero, surfaceData.origin);
            Assert.AreEqual(Vector3.zero, surfaceData.direction);
        }

        [Test]
        public void OriginConstructor()
        {
            SurfaceData surfaceData = new SurfaceData(Vector3.one, Vector3.forward);
            Assert.AreEqual(Vector3.one, surfaceData.origin);
            Assert.AreEqual(Vector3.forward, surfaceData.direction);
        }

        [Test]
        public void CollisionData()
        {
            SurfaceData surfaceData = new SurfaceData();

            //Create a couple of GameObjects for collision detection
            GameObject front = GameObject.CreatePrimitive(PrimitiveType.Cube);
            front.transform.position = Vector3.forward * 5f;
            GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottom.transform.position = Vector3.down * 5f;

            //Do an initial collision
            Ray forwardRay = new Ray(Vector3.zero, Vector3.forward);
            RaycastHit forwardHit;
            Physics.Raycast(forwardRay, out forwardHit);

            //Set up the initial collision of a surface
            surfaceData.origin = Vector3.zero;
            surfaceData.direction = Vector3.forward;
            surfaceData.CollisionData = forwardHit;

            //Do an second different collision
            Ray downwardRay = new Ray(Vector3.zero, Vector3.down);
            RaycastHit downwardHit;
            Physics.Raycast(downwardRay, out downwardHit);

            //Set up the initial collision of a surface
            surfaceData.origin = Vector3.zero;
            surfaceData.direction = Vector3.down;
            surfaceData.CollisionData = downwardHit;

            Assert.AreEqual(Vector3.zero, surfaceData.origin);
            Assert.AreEqual(Vector3.down, surfaceData.direction);
            Assert.AreEqual(bottom.transform, surfaceData.CollisionData.transform);
            Assert.AreEqual(front.transform, surfaceData.PreviousCollisionData.transform);

            Object.DestroyImmediate(front);
            Object.DestroyImmediate(bottom);
        }
    }
}