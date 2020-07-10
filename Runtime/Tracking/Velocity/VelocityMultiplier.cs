namespace Zinnia.Tracking.Velocity
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
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
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }
        /// <summary>
        /// The amount to multiply the source velocity by.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 VelocityMultiplierFactor { get; set; } = Vector3.one;
        /// <summary>
        /// The amount to multiply the source angular velocity by.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 AngularVelocityMultiplierFactor { get; set; } = Vector3.one;

        /// <summary>
        /// Sets the <see cref="VelocityMultiplierFactor"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityMultiplierFactorX(float value)
        {
            VelocityMultiplierFactor = new Vector3(value, VelocityMultiplierFactor.y, VelocityMultiplierFactor.z);
        }

        /// <summary>
        /// Sets the <see cref="VelocityMultiplierFactor"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityMultiplierFactorY(float value)
        {
            VelocityMultiplierFactor = new Vector3(VelocityMultiplierFactor.x, value, VelocityMultiplierFactor.z);
        }

        /// <summary>
        /// Sets the <see cref="VelocityMultiplierFactor"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityMultiplierFactorZ(float value)
        {
            VelocityMultiplierFactor = new Vector3(VelocityMultiplierFactor.x, VelocityMultiplierFactor.y, value);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocityMultiplierFactor"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityMultiplierFactorX(float value)
        {
            AngularVelocityMultiplierFactor = new Vector3(value, AngularVelocityMultiplierFactor.y, AngularVelocityMultiplierFactor.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocityMultiplierFactor"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityMultiplierFactorY(float value)
        {
            AngularVelocityMultiplierFactor = new Vector3(AngularVelocityMultiplierFactor.x, value, AngularVelocityMultiplierFactor.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocityMultiplierFactor"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityMultiplierFactorZ(float value)
        {
            AngularVelocityMultiplierFactor = new Vector3(AngularVelocityMultiplierFactor.x, AngularVelocityMultiplierFactor.y, value);
        }

        /// <inheritdoc />
        public override bool IsActive()
        {
            return base.IsActive() && Source != null && Source.isActiveAndEnabled;
        }

        /// <inheritdoc />
        protected override Vector3 DoGetVelocity() => Vector3.Scale(Source.GetVelocity(), VelocityMultiplierFactor);

        /// <inheritdoc />
        protected override Vector3 DoGetAngularVelocity() => Vector3.Scale(Source.GetAngularVelocity(), AngularVelocityMultiplierFactor);
    }
}