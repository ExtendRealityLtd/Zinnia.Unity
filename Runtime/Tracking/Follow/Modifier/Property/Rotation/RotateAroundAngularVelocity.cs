namespace Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Tracking.Velocity;

    /// <summary>
    /// Rotates the target around the applied axes based on the angular velocity provided by a source <see cref="VelocityTracker"/>.
    /// </summary>
    public class RotateAroundAngularVelocity : PropertyModifier
    {
        /// <summary>
        /// The <see cref="VelocityTracker"/> that is the source of the angular velocity.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker AngularVelocitySource { get; set; }
        /// <summary>
        /// Multiplies the <see cref="AngularVelocitySource"/> by this value.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 SourceMultiplier { get; set; } = Vector3.one;
        /// <summary>
        /// The axes to apply the angular velocity to.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3State ApplyToAxis { get; set; }

        /// <summary>
        /// Modifies the target rotation to match the given source rotation.
        /// </summary>
        /// <param name="source">The source is unused in this implementation.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (AngularVelocitySource == null)
            {
                return;
            }

            Vector3 input = AngularVelocitySource.GetAngularVelocity();
            input.Scale(SourceMultiplier);
            input.Scale(ApplyToAxis.ToVector3());

            Vector3 point = offset != null ? offset.transform.position : target.transform.position;

            target.transform.RotateAround(point, target.transform.rotation * target.transform.right, input.x);
            target.transform.RotateAround(point, target.transform.rotation * target.transform.up, input.y);
            target.transform.RotateAround(point, target.transform.rotation * target.transform.forward, input.z);
        }
    }
}