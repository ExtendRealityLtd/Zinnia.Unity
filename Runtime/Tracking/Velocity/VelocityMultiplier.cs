namespace Zinnia.Tracking.Velocity
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;

    /// <summary>
    /// Multiplies the given source velocity data.
    /// </summary>
    public class VelocityMultiplier : VelocityTracker
    {
        /// <summary>
        /// The <see cref="VelocityTracker"/> to use as the source data.
        /// </summary>
        [DocumentedByXml]
        public VelocityTracker source;
        /// <summary>
        /// The amount to multiply the source velocity by.
        /// </summary>
        [DocumentedByXml]
        public float velocityMultiplier = 1f;
        /// <summary>
        /// The amount to multiply the source angular velocity by.
        /// </summary>
        [DocumentedByXml]
        public float angularVelocityMultiplier = 1f;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return (base.IsActive() && source != null && source.isActiveAndEnabled);
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return source.GetVelocity() * velocityMultiplier;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return source.GetAngularVelocity() * angularVelocityMultiplier;
        }
    }
}