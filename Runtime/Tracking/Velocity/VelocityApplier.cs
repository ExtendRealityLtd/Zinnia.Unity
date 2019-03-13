namespace Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Applies the velocity data from the given <see cref="VelocityTracker"/> to the given <see cref="Rigidbody"/>.
    /// </summary>
    public class VelocityApplier : MonoBehaviour
    {
        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }
        /// <summary>
        /// The target <see cref="Rigidbody"/> to apply the source velocity data to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public Rigidbody Target { get; set; }

        /// <summary>
        /// Applies the velocity data to the <see cref="Target"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Apply()
        {
            if (Source == null || Target == null)
            {
                return;
            }

            Target.velocity = Source.GetVelocity();
            Target.angularVelocity = Source.GetAngularVelocity();
        }
    }
}