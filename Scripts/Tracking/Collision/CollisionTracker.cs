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
            OnCollisionStarted(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            OnCollisionChanged(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            OnCollisionStopped(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            OnCollisionStarted(eventData.Set(this, true, null, collider));
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            OnCollisionChanged(eventData.Set(this, true, null, collider));
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            OnCollisionStopped(eventData.Set(this, true, null, collider));
        }
    }
}