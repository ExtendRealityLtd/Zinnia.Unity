namespace Zinnia.Tracking.Velocity
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Provides the velocity as set on its properties.
    /// </summary>
    public class ConstantVelocityTracker : VelocityTracker
    {
        /// <summary>
        /// The velocity to return.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// The angular velocity to return.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 AngularVelocity { get; set; }

        /// <summary>
        /// Determines whether to extract the local property or the world property.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool UseLocal { get; set; }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return UseLocal ? transform.localRotation * AngularVelocity : AngularVelocity;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return UseLocal ? transform.localRotation * Velocity : Velocity;
        }
    }
}