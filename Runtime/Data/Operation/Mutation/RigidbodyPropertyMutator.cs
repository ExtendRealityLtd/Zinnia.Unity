namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the properties of a <see cref="Rigidbody"/> with the benefit of being able to specify a containing <see cref="GameObject"/> as the target.
    /// </summary>
    public class RigidbodyPropertyMutator : MonoBehaviour
    {
        [Tooltip("The Rigidbody to mutate.")]
        [SerializeField]
        private Rigidbody target;
        /// <summary>
        /// The <see cref="Rigidbody"/> to mutate.
        /// </summary>
        public Rigidbody Target
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
        /// The <see cref="Rigidbody.mass"/> value.
        /// </summary>
        private float mass;
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterMassChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.drag"/> value.
        /// </summary>
        private float drag;
        public float Drag
        {
            get
            {
                return drag;
            }
            set
            {
                drag = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterDragChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.angularDrag"/> value.
        /// </summary>
        private float angularDrag;
        public float AngularDrag
        {
            get
            {
                return angularDrag;
            }
            set
            {
                angularDrag = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterAngularDragChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.useGravity"/> state.
        /// </summary>
        private bool useGravity;
        public bool UseGravity
        {
            get
            {
                return useGravity;
            }
            set
            {
                useGravity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterUseGravityChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.isKinematic"/> state.
        /// </summary>
        private bool isKinematic;
        public bool IsKinematic
        {
            get
            {
                return isKinematic;
            }
            set
            {
                isKinematic = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterIsKinematicChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.velocity"/> value.
        /// </summary>
        private Vector3 velocity;
        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterVelocityChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.angularVelocity"/> value.
        /// </summary>
        private Vector3 angularVelocity;
        public Vector3 AngularVelocity
        {
            get
            {
                return angularVelocity;
            }
            set
            {
                angularVelocity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterAngularVelocityChange();
                }
            }
        }
        /// <summary>
        /// The <see cref="Rigidbody.maxAngularVelocity"/> value.
        /// </summary>
        private float maxAngularVelocity;
        public float MaxAngularVelocity
        {
            get
            {
                return maxAngularVelocity;
            }
            set
            {
                maxAngularVelocity = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterMaxAngularVelocityChange();
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
        /// Sets the <see cref="Target"/> based on the first found <see cref="Rigidbody"/> as either a direct, descendant or ancestor of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The <see cref="GameObject"/> to search for a <see cref="Rigidbody"/> on.</param>
        public virtual void SetTarget(GameObject target)
        {
            if (!this.IsValidState() || target == null)
            {
                return;
            }

            Target = target.TryGetComponent<Rigidbody>(true, true);
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityX(float value)
        {
            Velocity = new Vector3(value, Velocity.y, Velocity.z);
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityY(float value)
        {
            Velocity = new Vector3(Velocity.x, value, Velocity.z);
        }

        /// <summary>
        /// Sets the <see cref="Velocity"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetVelocityZ(float value)
        {
            Velocity = new Vector3(Velocity.x, Velocity.y, value);
        }

        /// <summary>
        /// Clears <see cref="Velocity"/>.
        /// </summary>
        public virtual void ClearVelocity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Velocity = default;
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityX(float value)
        {
            AngularVelocity = new Vector3(value, AngularVelocity.y, AngularVelocity.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityY(float value)
        {
            AngularVelocity = new Vector3(AngularVelocity.x, value, AngularVelocity.z);
        }

        /// <summary>
        /// Sets the <see cref="AngularVelocity"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetAngularVelocityZ(float value)
        {
            AngularVelocity = new Vector3(AngularVelocity.x, AngularVelocity.y, value);
        }

        /// <summary>
        /// Clears <see cref="AngularVelocity"/>.
        /// </summary>
        public virtual void ClearAngularVelocity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            AngularVelocity = default;
        }

        /// <summary>
        /// Called after <see cref="Mass"/> has been changed.
        /// </summary>
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