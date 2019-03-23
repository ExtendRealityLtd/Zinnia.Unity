namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Cast;

    /// <summary>
    /// Rotates the <see cref="GameObject"/> to look into a normal direction.
    /// </summary>
    public class PointNormalRotator : MonoBehaviour
    {
        /// <summary>
        /// The target to apply the rotations to.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }

        /// <summary>
        /// Handles the provided data to rotate the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="data">The data to take the rotation info from.</param>
        [RequiresBehaviourState]
        public virtual void HandleData(PointsCast.EventData data)
        {
            if (Target == null || data.HitData == null)
            {
                return;
            }

            Target.transform.rotation = Quaternion.FromToRotation(Vector3.up, data.HitData.Value.normal);
        }
    }
}