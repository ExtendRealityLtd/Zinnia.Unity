namespace Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Linq;
    using Zinnia.Cast;
    using Zinnia.Process;

    /// <summary>
    /// Determines whether any <see cref="Collider"/> obscures a line between two <see cref="GameObject"/>s.
    /// </summary>
    /// <remarks>
    /// The check is done by utilizing physics and as such a <see cref="Collider"/> is needed on <see cref="target"/>.
    /// </remarks>
    public class ObscuranceQuery : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="T:RaycastHit[]"/>.
        /// </summary>
        [Serializable]
        public class HitEvent : UnityEvent<RaycastHit[]>
        {
        }

        /// <summary>
        /// Defines the source location that the raycast will originate from towards the <see cref="target"/> location.
        /// </summary>
        public GameObject source;
        /// <summary>
        /// Defines the target location that the raycast will attain to reach from the originating <see cref="source"/> location.
        /// </summary>
        public Collider target;
        /// <summary>
        /// Optional settings to use when doing the raycast.
        /// </summary>
        public PhysicsCast physicsCast;

        /// <summary>
        /// Emitted when the raycast from <see cref="source"/> to <see cref="target"/> is obscured by another <see cref="Collider"/>.
        /// </summary>
        public HitEvent TargetObscured = new HitEvent();
        /// <summary>
        /// Emitted when the raycast from <see cref="source"/> is reaching <see cref="target"/> and is not obscured by another <see cref="Collider"/>.
        /// </summary>
        public UnityEvent TargetUnobscured = new UnityEvent();

        /// <summary>
        /// Whether the raycast from <see cref="source"/> to <see cref="target"/> was previously obscured by another <see cref="Collider"/>.
        /// </summary>
        protected bool? wasPreviouslyObscured;

        /// <summary>
        /// Casts a ray from the <see cref="source"/> origin location towards the <see cref="target"/> destination location and determines whether the raycast is blocked by another <see cref="Collider"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 difference = target.transform.position - source.transform.position;
            Ray ray = new Ray(source.transform.position, difference);
            RaycastHit[] raycastHits = PhysicsCast.RaycastAll(
                physicsCast,
                ray,
                difference.magnitude,
                Physics.IgnoreRaycastLayer);
            bool isObscured = raycastHits.Any(hit => hit.transform.gameObject != source && hit.collider != target);
            if (isObscured == wasPreviouslyObscured)
            {
                return;
            }

            wasPreviouslyObscured = isObscured;

            if (isObscured)
            {
                TargetObscured?.Invoke(raycastHits);
            }
            else
            {
                TargetUnobscured?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            wasPreviouslyObscured = null;
        }
    }
}