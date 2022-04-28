namespace Zinnia.Data.Operation.Mutation
{
    using System;
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Mutates the Euler rotation of a transform with an optional custom rotation origin.
    /// </summary>
    public class TransformEulerRotationMutator : TransformPropertyMutator
    {
        #region Rotation Settings
        [Header("Rotation Settings")]
        [Tooltip("An optional rotation origin to perform the rotation around. The origin must be a child of the TransformPropertyMutator.Target.")]
        [SerializeField]
        private GameObject origin;
        /// <summary>
        /// An optional rotation origin to perform the rotation around. The origin must be a child of the <see cref="TransformPropertyMutator.Target"/>.
        /// </summary>
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
        [Tooltip("Determines which axes to consider from the Origin.")]
        [SerializeField]
        private Vector3State applyOriginOnAxis = Vector3State.True;
        /// <summary>
        /// Determines which axes to consider from the <see cref="Origin"/>.
        /// </summary>
        public Vector3State ApplyOriginOnAxis
        {
            get
            {
                return applyOriginOnAxis;
            }
            set
            {
                applyOriginOnAxis = value;
            }
        }
        #endregion

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
        /// Clears <see cref="ApplyOriginOnAxis"/>.
        /// </summary>
        public virtual void ClearApplyOriginOnAxis()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ApplyOriginOnAxis = default;
        }

        /// <summary>
        /// Sets the <see cref="ApplyOriginOnAxis"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyOriginOnAxisX(bool value)
        {
            ApplyOriginOnAxis = new Vector3State(value, ApplyOriginOnAxis.yState, ApplyOriginOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyOriginOnAxis"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyOriginOnAxisY(bool value)
        {
            ApplyOriginOnAxis = new Vector3State(ApplyOriginOnAxis.xState, value, ApplyOriginOnAxis.zState);
        }

        /// <summary>
        /// Sets the <see cref="ApplyOriginOnAxis"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetApplyOriginOnAxisZ(bool value)
        {
            ApplyOriginOnAxis = new Vector3State(ApplyOriginOnAxis.xState, ApplyOriginOnAxis.yState, value);
        }

        /// <inheritdoc/>
        protected override float GetGlobalAxisValue(int axis)
        {
            return Target.transform.eulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override float GetLocalAxisValue(int axis)
        {
            return Target.transform.localEulerAngles[axis];
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementGlobal(Vector3 input)
        {
            Vector3 originPosition = GetOriginPosition();
            Target.transform.rotation = Quaternion.Euler(Target.transform.eulerAngles + input);
            ApplyRotationOriginPosition(originPosition);

            return Target.transform.eulerAngles;
        }

        /// <inheritdoc/>
        protected override Vector3 IncrementLocal(Vector3 input)
        {
            Vector3 originPosition = GetOriginPosition();
            Target.transform.localRotation = Quaternion.Euler(Target.transform.localEulerAngles + input);
            ApplyRotationOriginPosition(originPosition);

            return Target.transform.localEulerAngles;
        }

        /// <inheritdoc/>
        protected override Vector3 SetGlobal(Vector3 input)
        {
            Vector3 originPosition = GetOriginPosition();
            Target.transform.eulerAngles = input;
            ApplyRotationOriginPosition(originPosition);

            return Target.transform.eulerAngles;
        }

        /// <inheritdoc/>
        protected override Vector3 SetLocal(Vector3 input)
        {
            Vector3 originPosition = GetOriginPosition();
            Target.transform.localEulerAngles = input;
            ApplyRotationOriginPosition(originPosition);

            return Target.transform.localEulerAngles;
        }

        protected virtual void OnEnable()
        {
            OnAfterOriginChange();
        }

        /// <summary>
        /// Returns the <see cref="Origin"/> position if a <see cref="Origin"/> is defined.
        /// </summary>
        /// <returns>The origin position.</returns>
        protected virtual Vector3 GetOriginPosition()
        {
            if (Origin == null)
            {
                return Vector3.zero;
            }

            Vector3 originAxesToApply = ApplyOriginOnAxis.ToVector3();
            Vector3? cachedOriginLocalPosition = null;

            if (!originAxesToApply.ApproxEquals(Vector3.one))
            {
                cachedOriginLocalPosition = Origin.transform.localPosition;
                originAxesToApply.Scale(Origin.transform.localPosition);
                Origin.transform.localPosition = originAxesToApply;
            }

            Vector3 returnValue = Origin.transform.position;

            if (cachedOriginLocalPosition != null)
            {
                Origin.transform.localPosition = cachedOriginLocalPosition.GetValueOrDefault();
            }

            return returnValue;
        }

        /// <summary>
        /// Applies the position of the <see cref="Origin"/> to the <see cref="TransformPropertyMutator.Target"/> to ensure it rotates around the set origin.
        /// </summary>
        /// <param name="originPosition">The offset position to apply.</param>
        protected virtual void ApplyRotationOriginPosition(Vector3 originPosition)
        {
            if (Origin == null)
            {
                return;
            }

            originPosition -= GetOriginPosition();
            Target.transform.position += originPosition;
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
        protected virtual void OnAfterOriginChange()
        {
            if (Origin == null || Target == null)
            {
                return;
            }

            if (!Origin.transform.IsChildOf(Target.transform))
            {
                throw new ArgumentException($"The `RotationOrigin` [{Origin.name}] must be a child of the `Target` [{Target.name}] GameObject.");
            }
        }

        [Obsolete("Use `OnAfterOriginChange` instead.")]
        protected virtual void OnAfterRotationOriginChange()
        {
            OnAfterOriginChange();
        }
    }
}