namespace Zinnia.Tracking.Velocity
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Applies the velocity data from the given <see cref="VelocityTracker"/> to the given <see cref="Rigidbody"/>.
    /// </summary>
    public class VelocityApplier : MonoBehaviour
    {
        /// <summary>
        /// The source <see cref="VelocityTracker "/> to receive the velocity data from.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public VelocityTracker Source { get; set; }
        /// <summary>
        /// The target <see cref="Rigidbody"/> to apply the source velocity data to.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Rigidbody Target { get; set; }

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Applies the velocity data to the <see cref="Target"/>.
        /// </summary>
        public virtual void Apply()
        {
            if (!this.IsValidState() || Source == null || Target == null)
            {
                return;
            }

            Target.velocity = Source.GetVelocity();
            Target.angularVelocity = Source.GetAngularVelocity();
        }
    }
}