namespace Zinnia.Tracking.Query
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
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
        public class HitEvent : UnityEvent<HeapAllocationFreeReadOnlyList<RaycastHit>> { }

        public class MissingColliderException : Exception
        {
            public MissingColliderException(UnityEngine.Object owner, GameObject missingColliderGameObject) : base($"The configured {nameof(Target)} '{missingColliderGameObject}' on '{owner}' needs a {nameof(Collider)} on it or if it has a {nameof(Rigidbody)} on it then a child {nameof(Collider)} is required.") { }
        }

        [Tooltip("Defines the source location that the RayCast will originate from towards the Target location.")]
        [SerializeField]
        private GameObject source;
        /// <summary>
        /// Defines the source location that the RayCast will originate from towards the <see cref="Target"/> location.
        /// </summary>
        public GameObject Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("Defines the target location that the RayCast will attain to reach from the originating Source location.")]
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// Defines the target location that the RayCast will attain to reach from the originating <see cref="Source"/> location.
        /// </summary>
        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterTargetChange();
                }
            }
        }
        [Tooltip("Optional settings to use when doing the RayCast.")]
        [SerializeField]
        private PhysicsCast physicsCast;
        /// <summary>
        /// Optional settings to use when doing the RayCast.
        /// </summary>
        public PhysicsCast PhysicsCast
        {
            get
            {
                return physicsCast;
            }
            set
            {
                physicsCast = value;
            }
        }

        /// <summary>
        /// Emitted when the RayCast from <see cref="Source"/> to <see cref="Target"/> is obscured by another <see cref="Collider"/>.
        /// </summary>
        public HitEvent TargetObscured = new HitEvent();
        /// <summary>
        /// Emitted when the RayCast from <see cref="Source"/> is reaching <see cref="Target"/> and is not obscured by another <see cref="Collider"/>.
        /// </summary>
        public UnityEvent TargetUnobscured = new UnityEvent();

        /// <summary>
        /// Whether the RayCast from <see cref="Source"/> to <see cref="Target"/> was previously obscured by another <see cref="Collider"/>.
        /// </summary>
        protected bool? wasPreviouslyObscured;

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Clears <see cref="PhysicsCast"/>.
        /// </summary>
        public virtual void ClearPhysicsCast()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PhysicsCast = default;
        }

        /// <summary>
        /// Casts a ray from the <see cref="Source"/> origin location towards the <see cref="Target"/> destination location and determines whether the RayCast is blocked by another <see cref="Collider"/>.
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
        /// Throws a <see cref="MissingColliderException"/> if <see cref="Target"/> is missing a <see cref="Collider"/> or if the <see cref="Target"/> has a <see cref="Rigidbody"/> and is missing a child <see cref="Collider"/>.
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
        protected virtual void OnAfterTargetChange()
        {
            CheckTarget();
        }
    }
}