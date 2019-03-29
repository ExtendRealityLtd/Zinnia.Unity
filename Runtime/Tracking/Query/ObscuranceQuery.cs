namespace Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Cast;
    using Zinnia.Data.Type;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Determines whether any <see cref="Collider"/> obscures a line between two <see cref="GameObject"/>s.
    /// </summary>
    /// <remarks>
    /// The check is done by utilizing physics and as such a <see cref="Collider"/> is needed on <see cref="Target"/>.
    /// </remarks>
    public class ObscuranceQuery : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Defines the event with the <see cref="HeapAllocationFreeReadOnlyList{T}"/> of <see cref="RaycastHit"/>s.
        /// </summary>
        [Serializable]
        public class HitEvent : UnityEvent<HeapAllocationFreeReadOnlyList<RaycastHit>>
        {
        }

        public class MissingColliderException : Exception
        {
            public MissingColliderException(UnityEngine.Object owner, GameObject missingColliderGameObject) : base($"The configured {nameof(Target)} '{missingColliderGameObject}' on '{owner}' needs a {nameof(Rigidbody)} or {nameof(Collider)} on it.") { }
        }

        /// <summary>
        /// Defines the source location that the raycast will originate from towards the <see cref="Target"/> location.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// Defines the target location that the raycast will attain to reach from the originating <see cref="Source"/> location.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// Optional settings to use when doing the raycast.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PhysicsCast PhysicsCast { get; set; }

        /// <summary>
        /// Emitted when the raycast from <see cref="Source"/> to <see cref="Target"/> is obscured by another <see cref="Collider"/>.
        /// </summary>
        [DocumentedByXml]
        public HitEvent TargetObscured = new HitEvent();
        /// <summary>
        /// Emitted when the raycast from <see cref="Source"/> is reaching <see cref="Target"/> and is not obscured by another <see cref="Collider"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent TargetUnobscured = new UnityEvent();

        /// <summary>
        /// Whether the raycast from <see cref="Source"/> to <see cref="Target"/> was previously obscured by another <see cref="Collider"/>.
        /// </summary>
        protected bool? wasPreviouslyObscured;

        /// <summary>
        /// Casts a ray from the <see cref="Source"/> origin location towards the <see cref="Target"/> destination location and determines whether the raycast is blocked by another <see cref="Collider"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 sourcePosition = Source.transform.position;
            Vector3 difference = Target.transform.position - sourcePosition;
            Ray ray = new Ray(sourcePosition, difference);
            HeapAllocationFreeReadOnlyList<RaycastHit> raycastHits = PhysicsCast.RaycastAll(
                PhysicsCast,
                ray,
                difference.magnitude,
                Physics.IgnoreRaycastLayer);

            bool isObscured = false;
            foreach (RaycastHit hit in raycastHits)
            {
                GameObject hitGameObject = hit.transform.gameObject;
                if (hitGameObject != Source && hitGameObject != Target)
                {
                    isObscured = true;
                    break;
                }
            }

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

        protected virtual void OnEnable()
        {
            CheckTarget();
        }

        protected virtual void OnDisable()
        {
            wasPreviouslyObscured = null;
        }

        /// <summary>
        /// Throws a <see cref="MissingColliderException"/> if <see cref="Target"/> has neither a <see cref="Rigidbody"/> nor a <see cref="Collider"/> on it.
        /// </summary>
        protected virtual void CheckTarget()
        {
            if (Target != null
                && (Target.TryGetComponent<Rigidbody>() == null || Target.TryGetComponent<Collider>(true) == null)
                && Target.TryGetComponent<Collider>() == null)
            {
                throw new MissingColliderException(this, Target);
            }
        }

        /// <summary>
        /// Called after <see cref="Target"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Target))]
        protected virtual void OnAfterTargetChange()
        {
            CheckTarget();
        }
    }
}