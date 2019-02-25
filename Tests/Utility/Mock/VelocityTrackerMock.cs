using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Utility.Mock
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class VelocityTrackerMock : VelocityTracker
    {
        private bool mockActive;
        private Vector3 mockVelocity;
        private Vector3 mockAngularVelocity;

        public static VelocityTrackerMock Generate(bool active, Vector3 velocity, Vector3 angularVelocity)
        {
            return Generate(active, velocity, angularVelocity, out GameObject _);
        }

        public static VelocityTrackerMock Generate(bool active, Vector3 velocity, Vector3 angularVelocity, out GameObject container)
        {
            container = new GameObject();
            VelocityTrackerMock mock = container.AddComponent<VelocityTrackerMock>();
            mock.Set(active, velocity, angularVelocity);
            return mock;
        }

        public virtual void Set(bool active, Vector3 velocity, Vector3 angularVelocity)
        {
            mockActive = active;
            mockVelocity = velocity;
            mockAngularVelocity = angularVelocity;
        }

        public override bool IsActive()
        {
            return mockActive;
        }

        protected override Vector3 DoGetVelocity()
        {
            return mockVelocity;
        }

        protected override Vector3 DoGetAngularVelocity()
        {
            return mockAngularVelocity;
        }
    }
}