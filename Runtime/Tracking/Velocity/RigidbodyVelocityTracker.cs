namespace Zinnia.Tracking.Velocity
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Retrieves the velocity for a <see cref="Rigidbody"/>.
    /// </summary>
    public class RigidbodyVelocityTracker : VelocityTracker
    {
        /// <summary>
        /// The source to track and estimate velocities for.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public Rigidbody Source { get; set; }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Source != null && Source.gameObject.activeInHierarchy;
        }
        
        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return Source != null ? Source.angularVelocity : Vector3.zero;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return Source != null ? Source.velocity : Vector3.zero;
        }
    }
}