namespace Zinnia.Tracking.Query
{
    using UnityEngine;
    using UnityEngine.Events;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Zinnia.Process;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> faces (through the local forward direction) another <see cref="GameObject"/>.
    /// </summary>
    /// <remarks>
    /// No physics checks are done and as such occlusion isn't part of the information gained by this component.
    /// </remarks>
    public class FacingQuery : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The object used as the origin to check if it is facing towards <see cref="Target"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Source { get; set; }
        /// <summary>
        /// The object that will be checked to see if <see cref="Source"/> is facing it.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// A sphere radius that defines the volume in which <see cref="Target"/> can still be considered seen by the <see cref="Source"/>.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float TargetRadius { get; set; } = 0.1f;

        /// <summary>
        /// Emitted when <see cref="Source"/> is facing <see cref="Target"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent TargetFaced = new UnityEvent();
        /// <summary>
        /// Emitted when <see cref="Source"/> no longer faces <see cref="Target"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent TargetNotFaced = new UnityEvent();

        /// <summary>
        /// Whether <see cref="Source"/> was previously facing <see cref="Target"/>.
        /// </summary>
        protected bool? wasPreviouslyFacing;

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