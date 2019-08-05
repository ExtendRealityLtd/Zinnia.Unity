using Zinnia.Data.Type;
using Zinnia.Action;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class SurfaceChangeActionTest
    {
        private GameObject containingObject;
        private SurfaceChangeActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<SurfaceChangeActionMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SurfaceChanged()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.ChangeDistance = 1f;
            subject.CheckAxis = Vector3State.True;

            SurfaceData surfaceData = new SurfaceData(Vector3.one, Vector3.down);

            //set current surface to zero
            RaycastHit ray = new RaycastHit
            {
                point = Vector3.zero
            };
            surfaceData.CollisionData = ray;
            surfaceData.PositionOverride = ray.point;
            //set surface to one so there is a change between surface positions
            ray = new RaycastHit
            {
                point = Vector3.one
            };
            surfaceData.CollisionData = ray;
            surfaceData.PositionOverride = ray.point;

            subject.Receive(surfaceData);

            Assert.IsTrue(activatedListenerMock.Received);
        }

        [Test]
        public void SurfaceUnchanged()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.ChangeDistance = 1f;
            subject.CheckAxis = new Vector3State(false, true, false);

            SurfaceData surfaceData = new SurfaceData(Vector3.one, Vector3.down);

            //set current surface to zero
            RaycastHit ray = new RaycastHit
            {
                point = Vector3.zero
            };
            surfaceData.CollisionData = ray;
            surfaceData.PositionOverride = ray.point;
            //set surface to one so there is a change between surface positions
            ray = new RaycastHit
            {
                point = Vector3.one
            };
            surfaceData.CollisionData = ray;
            surfaceData.PositionOverride = ray.point;

            subject.Receive(surfaceData);

            Assert.IsFalse(activatedListenerMock.Received);
        }
    }

    public class SurfaceChangeActionMock : SurfaceChangeAction
    {
        //As The transform in the RaycastHit cannot be set without doing an actual raycast, just ignore that check for the test.
        protected override bool ValidSurfaceData(SurfaceData surfaceData)
        {
            return true;
        }

        protected override Vector3 GetCollisionPoint(RaycastHit collisionData)
        {
            return GeneratePoint(collisionData.point);
        }
    }
}