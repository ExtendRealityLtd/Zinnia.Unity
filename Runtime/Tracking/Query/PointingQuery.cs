namespace Zinnia.Tracking.Query
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Process;

    /// <summary>
    /// Determines whether a <see cref="GameObject"/> points at another <see cref="GameObject"/>.
    /// </summary>
    /// <remarks>
    /// No physics checks are done and as such occlusion isn't part of the information gained by this component.
    /// </remarks>
    public class PointingQuery : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The object used as the origin to check if it is pointing towards <see cref="target"/>.
        /// </summary>
        [DocumentedByXml]
        public GameObject source;
        /// <summary>
        /// The object that will be checked to see if <see cref="source"/> is pointing at it.
        /// </summary>
        [DocumentedByXml]
        public GameObject target;
        /// <summary>
        /// A sphere radius that defines the volume in which <see cref="target"/> can be pointed at.
        /// </summary>
        [DocumentedByXml]
        public float targetRadius = 0.1f;

        /// <summary>
        /// Emitted when <see cref="source"/> starts pointing at <see cref="target"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent StartedPointingAtTarget = new UnityEvent();
        /// <summary>
        /// Emitted when <see cref="source"/> no longer points at <see cref="target"/>.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent StoppedPointingAtTarget = new UnityEvent();

        /// <summary>
        /// Whether <see cref="source"/> was previously pointing at <see cref="target"/>.
        /// </summary>
        protected bool? wasPreviouslyPointing;

        /// <summary>
        /// Determines whether <see cref="source"/> points at <see cref="target"/> defined by its position and <see cref="targetRadius"/>.
        /// </summary>
        public virtual void Process()
        {
            Vector3 sourcePosition = source.transform.position;
            Vector3 targetPosition = target.transform.position;

            float distance = Vector3.Distance(targetPosition, sourcePosition);
            Vector3 glancePoint = sourcePosition + (source.transform.forward * distance);
            bool isPointing = Vector3.Distance(targetPosition, glancePoint) <= targetRadius;
            if (isPointing == wasPreviouslyPointing)
            {
                return;
            }

            wasPreviouslyPointing = isPointing;

            if (isPointing)
            {
                StartedPointingAtTarget?.Invoke();
            }
            else
            {
                StoppedPointingAtTarget?.Invoke();
            }
        }

        protected virtual void OnDisable()
        {
            wasPreviouslyPointing = null;
        }
    }
}