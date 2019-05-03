namespace Zinnia.Cast.Operation.Mutation
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Rule;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the properties of a <see cref="PointsCast"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class PointsCastPropertyMutator : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="PointsCast"/> to mutate.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public PointsCast Target { get; set; }

        /// <summary>
        /// The origin point for the cast.
        /// </summary>
        [Cleared]
        public GameObject Origin { get; set; }
        /// <summary>
        /// Optionally affects the cast.
        /// </summary>
        [Cleared]
        public PhysicsCast PhysicsCast { get; set; }
        /// <summary>
        /// Optionally determines targets based on the set rules.
        /// </summary>
        [Cleared]
        public RuleContainer TargetValidity { get; set; }

        /// <summary>
        /// An override for the destination location point in world space.
        /// </summary>
        public Vector3? DestinationPointOverride { get; set; }

        /// <summary>
        /// Sets the <see cref="Target"/> based on the first found <see cref="PointsCast"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="PointsCast"/> on.</param>
        [RequiresBehaviourState]
        public virtual void SetTarget(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            Target = target.TryGetComponent<PointsCast>(true, true);
        }

        /// <summary>
        /// Sets the <see cref="DestinationPointOverride"/> from a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="destinationPointOverride">The new value.</param>
        public virtual void SetDestinationPointOverride(Vector3 destinationPointOverride)
        {
            DestinationPointOverride = destinationPointOverride;
        }

        /// <summary>
        /// Clears the <see cref="DestinationPointOverride"/>.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ClearDestinationPointOverride()
        {
            if (Target == null)
            {
                return;
            }

            Target.ClearDestinationPointOverride();
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Origin))]
        protected virtual void OnAfterOriginChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.Origin = Origin;
        }

        /// <summary>
        /// Called after <see cref="PhysicsCast"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(PhysicsCast))]
        protected virtual void OnAfterPhysicsCastChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.PhysicsCast = PhysicsCast;
        }

        /// <summary>
        /// Called after <see cref="TargetValidity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(TargetValidity))]
        protected virtual void OnAfterTargetValidityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.TargetValidity = TargetValidity;
        }

        /// <summary>
        /// Called after <see cref="DestinationPointOverride"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(DestinationPointOverride))]
        protected virtual void OnAfterDestinationPointOverrideChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.DestinationPointOverride = DestinationPointOverride;
        }
    }
}