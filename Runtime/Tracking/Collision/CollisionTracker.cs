namespace Zinnia.Tracking.Collision
{
    using UnityEngine;

    /// <summary>
    /// Tracks collisions on the <see cref="GameObject"/> this component is on.
    /// </summary>
    public class CollisionTracker : CollisionNotifier
    {
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if ((StatesToProcess & CollisionStates.Enter) == 0)
            {
                return;
            }

            OnCollisionStarted(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            if ((StatesToProcess & CollisionStates.Stay) == 0)
            {
                return;
            }

            OnCollisionChanged(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if ((StatesToProcess & CollisionStates.Exit) == 0)
            {
                return;
            }

            OnCollisionStopped(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if ((StatesToProcess & CollisionStates.Enter) == 0)
            {
                return;
            }

            OnCollisionStarted(eventData.Set(this, true, null, collider));
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            if ((StatesToProcess & CollisionStates.Stay) == 0)
            {
                return;
            }

            OnCollisionChanged(eventData.Set(this, true, null, collider));
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            if ((StatesToProcess & CollisionStates.Exit) == 0)
            {
                return;
            }

            OnCollisionStopped(eventData.Set(this, true, null, collider));
        }
    }
}