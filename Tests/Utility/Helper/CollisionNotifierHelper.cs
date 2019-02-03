using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Utility.Helper
{
    using UnityEngine;

    public static class CollisionNotifierHelper
    {
        public static CollisionNotifier.EventData GetEventData(out GameObject container, Vector3 position = default)
        {
            container = new GameObject();
            BoxCollider collider = container.AddComponent<BoxCollider>();
            collider.transform.position = position;

            return new CollisionNotifier.EventData().Set(null, true, null, collider);
        }
    }
}