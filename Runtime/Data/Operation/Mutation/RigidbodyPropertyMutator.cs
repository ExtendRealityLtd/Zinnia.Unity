namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the properties of a <see cref="Rigidbody"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class RigidbodyPropertyMutator : MonoBehaviour
    {
        /// <summary>
        /// The rigidbody to mutate.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public Rigidbody Target { get; set; }

        /// <summary>
        /// The <see cref="Rigidbody.mass"/> value.
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.drag"/> value.
        /// </summary>
        public float Drag { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.angularDrag"/> value.
        /// </summary>
        public float AngularDrag { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.useGravity"/> state.
        /// </summary>
        public bool UseGravity { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.isKinematic"/> state.
        /// </summary>
        public bool IsKinematic { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.velocity"/> value.
        /// </summary>
        public Vector3 Velocity { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.angularVelocity"/> value.
        /// </summary>
        public Vector3 AngularVelocity { get; set; }
        /// <summary>
        /// The <see cref="Rigidbody.maxAngularVelocity"/> value.
        /// </summary>
        public float MaxAngularVelocity { get; set; }

        /// <summary>
        /// Sets the <see cref="Target"/> based on the first found <see cref="Rigidbody"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="Rigidbody"/> on.</param>
        [RequiresBehaviourState]
        public virtual void SetTarget(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            Target = target.TryGetComponent<Rigidbody>(true, true);
        }

        /// <summary>
        /// Sets the velocity of the <see cref="Target"/> to zero.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ClearVelocity()
        {
            Velocity = Vector3.zero;
        }

        /// <summary>
        /// Sets the angular velocity of the <see cref="Target"/> to zero.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ClearAngularVelocity()
        {
            AngularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Called after <see cref="Mass"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Mass))]
        protected virtual void OnAfterMassChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.mass = Mass;
        }

        /// <summary>
        /// Called after <see cref="Drag"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Drag))]
        protected virtual void OnAfterDragChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.drag = Drag;
        }

        /// <summary>
        /// Called after <see cref="AngularDrag"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(AngularDrag))]
        protected virtual void OnAfterAngularDragChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.angularDrag = AngularDrag;
        }

        /// <summary>
        /// Called after <see cref="UseGravity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(UseGravity))]
        protected virtual void OnAfterUseGravityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.useGravity = UseGravity;
        }

        /// <summary>
        /// Called after <see cref="IsKinematic"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(IsKinematic))]
        protected virtual void OnAfterIsKinematicChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.isKinematic = IsKinematic;
        }

        /// <summary>
        /// Called after <see cref="Velocity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Velocity))]
        protected virtual void OnAfterVelocityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.velocity = Velocity;
        }

        /// <summary>
        /// Called after <see cref="AngularVelocity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(AngularVelocity))]
        protected virtual void OnAfterAngularVelocityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.angularVelocity = AngularVelocity;
        }

        /// <summary>
        /// Called after <see cref="MaxAngularVelocity"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(MaxAngularVelocity))]
        protected virtual void OnAfterMaxAngularVelocityChange()
        {
            if (Target == null)
            {
                return;
            }

            Target.maxAngularVelocity = MaxAngularVelocity;
        }
    }
}