﻿namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Multiplies the given source velocity data.
    /// </summary>
    public class VelocityMultiplier : VelocityTracker
    {
        /// <summary>
        /// The <see cref="VelocityTracker"/> to use as the source data.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }
        /// <summary>
        /// The amount to multiply the source velocity by.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float VelocityMultiplierFactor { get; set; } = 1f;
        /// <summary>
        /// The amount to multiply the source angular velocity by.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float AngularVelocityMultiplierFactor { get; set; } = 1f;

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Source != null && Source.isActiveAndEnabled;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity()
        {
            return Source.GetVelocity() * VelocityMultiplierFactor;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity()
        {
            return Source.GetAngularVelocity() * AngularVelocityMultiplierFactor;
        }
    }
}