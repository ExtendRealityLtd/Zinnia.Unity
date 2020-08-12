using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class ActiveCollisionConsumerMock : ActiveCollisionConsumer
    {
        public bool received;

        public override bool Consume(ActiveCollisionPublisher.PayloadData publisher, CollisionNotifier.EventData currentCollision)
        {
            received = false;
            if (isActiveAndEnabled)
            {
                received = true;
            }

            return received;
        }

        public virtual void SetConsumerContainer(GameObject container)
        {
            ConsumerContainer = container;
        }
    }
}