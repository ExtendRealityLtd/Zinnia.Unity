namespace Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> faces (through the local forward direction) another <see cref="GameObject"/>.
    /// </summary>
    /// <remarks>
    /// No physics checks are done and as such occlusion isn't part of the information gained by this component.
    /// </remarks>
    public class FacingQuery : MonoBehaviour, IProcessable
    {
        [Tooltip("The object used as the origin to check if it is facing towards Target.")]
        [SerializeField]
        private GameObject source;
        /// <summary>
        /// The object used as the origin to check if it is facing towards <see cref="Target"/>.
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
        [Tooltip("The object that will be checked to see if Source is facing it.")]
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// The object that will be checked to see if <see cref="Source"/> is facing it.
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
            }
        }
        [Tooltip("A sphere radius that defines the volume in which Target can still be considered seen by the Source.")]
        [SerializeField]
        private float targetRadius = 0.1f;
        /// <summary>
        /// A sphere radius that defines the volume in which <see cref="Target"/> can still be considered seen by the <see cref="Source"/>.
        /// </summary>
        public float TargetRadius
        {
            get
            {
                return targetRadius;
            }
            set
            {
                targetRadius = value;
            }
        }

        /// <summary>
        /// Emitted when <see cref="Source"/> is facing <see cref="Target"/>.
        /// </summary>
        public UnityEvent TargetFaced = new UnityEvent();
        /// <summary>
        /// Emitted when <see cref="Source"/> no longer faces <see cref="Target"/>.
        /// </summary>
        public UnityEvent TargetNotFaced = new UnityEvent();

        /// <summary>
        /// Whether <see cref="Source"/> was previously facing <see cref="Target"/>.
        /// </summary>
        protected bool? wasPreviouslyFacing;

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
        /// Determines whether <see cref="Source"/> is facing <see cref="Target"/> defined by its position and <see cref="TargetRadius"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 sourcePosition = Source.transform.position;
            Vector3 targetPosition = Target.transform.position;

            float distance = Vector3.Distance(targetPosition, sourcePosition);
            Vector3 glancePoint = sourcePosition + (Source.transform.forward * distance);
            bool isFacing = Vector3.Distance(targetPosition, glancePoint) <= TargetRadius;

            if (isFacing == wasPreviouslyFacing)
            {
                return;
            }

            wasPreviouslyFacing = isFacing;

            if (isFacing)
            {
                TargetFaced?.Invoke();
            }
            else
            {
                TargetNotFaced?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            wasPreviouslyFacing = null;
        }
    }
}