namespace Zinnia.Cast.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// Mutates the properties of a <see cref="PointsCast"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class PointsCastPropertyMutator : MonoBehaviour
    {
        [Tooltip("The PointsCast to mutate.")]
        [SerializeField]
        private PointsCast target;
        /// <summary>
        /// The <see cref="PointsCast"/> to mutate.
        /// </summary>
        public PointsCast Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// The origin point for the cast.
        /// </summary>
        private GameObject origin;
        public GameObject Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterOriginChange();
                }
            }
        }
        /// <summary>
        /// Optionally affects the cast.
        /// </summary>
        private PhysicsCast physicsCast;
        public PhysicsCast PhysicsCast
        {
            get
            {
                return physicsCast;
            }
            set
            {
                physicsCast = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterPhysicsCastChange();
                }
            }
        }
        /// <summary>
        /// Optionally determines targets based on the set rules.
        /// </summary>
        private RuleContainer targetValidity;
        public RuleContainer TargetValidity
        {
            get
            {
                return targetValidity;
            }
            set
            {
                targetValidity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterTargetValidityChange();
                }
            }
        }

        /// <summary>
        /// An override for the destination location point in world space.
        /// </summary>
        private Vector3? destinationPointOverride;
        public Vector3? DestinationPointOverride
        {
            get
            {
                return destinationPointOverride;
            }
            protected set
            {
                destinationPointOverride = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterDestinationPointOverrideChange();
                }
            }
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
        /// Clears <see cref="Origin"/>.
        /// </summary>
        public virtual void ClearOrigin()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Origin = default;
        }

        /// <summary>
        /// Clears <see cref="PhysicsCast"/>.
        /// </summary>
        public virtual void ClearPhysicsCast()
        {
            if (!this.IsValidState())
            {
                return;
            }

            PhysicsCast = default;
        }

        /// <summary>
        /// Clears <see cref="TargetValidity"/>.
        /// </summary>
        public virtual void ClearTargetValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            TargetValidity = default;
        }

        /// <summary>
        /// Sets the <see cref="Target"/> based on the first found <see cref="PointsCast"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="PointsCast"/> on.</param>
        public virtual void SetTarget(GameObject target)
        {
            if (!this.IsValidState() || target == null)
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
        public virtual void ClearDestinationPointOverride()
        {
            if (!this.IsValidState() || Target == null)
            {
                return;
            }

            DestinationPointOverride = default;
            Target.ClearDestinationPointOverride();
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
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