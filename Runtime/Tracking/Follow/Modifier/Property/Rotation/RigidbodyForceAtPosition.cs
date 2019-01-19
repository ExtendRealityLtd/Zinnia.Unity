namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Updates the rigidbody by applying force at the position difference between the source and the attachment point providing a torque rotation.
    /// </summary>
    public class RigidbodyForceAtPosition : PropertyModifier
    {
        /// <summary>
        /// The point where the attachment was made.
        /// </summary>
        [DocumentedByXml, Cleared]
        public GameObject attachmentPoint;

        /// <summary>
        /// A cached version of the target rigidbody.
        /// </summary>
        protected Rigidbody cachedTargetRigidbody;
        /// <summary>
        /// A cached version of the target.
        /// </summary>
        protected GameObject cachedTarget;

        /// <summary>
        /// Sets the attachment point.
        /// </summary>
        /// <param name="attachmentPoint">The new attachment point.</param>
        public virtual void SetAttachmentPoint(GameObject attachmentPoint)
        {
            this.attachmentPoint = attachmentPoint;
        }

        /// <summary>
        /// Applies a force at the attachment point position to the target rigidbody creating torque for rotation.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            cachedTargetRigidbody = (cachedTargetRigidbody == null || target != cachedTarget ? target.TryGetComponent<Rigidbody>(true) : cachedTargetRigidbody);
            cachedTarget = target;

            if (cachedTargetRigidbody == null || source == null || attachmentPoint == null)
            {
                return;
            }

            Vector3 rotationForce = source.transform.position - attachmentPoint.transform.position;
            cachedTargetRigidbody.AddForceAtPosition(rotationForce, attachmentPoint.transform.position, ForceMode.VelocityChange);
        }
    }
}