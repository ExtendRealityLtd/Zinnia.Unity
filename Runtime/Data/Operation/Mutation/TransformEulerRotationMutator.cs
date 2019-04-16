namespace Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using System;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.PropertySerializationAttribute;

    /// <summary>
    /// Mutates the euler rotation of a transform with an optional custom rotation origin.
    /// </summary>
    public class TransformEulerRotationMutator : TransformPropertyMutator
    {
        /// <summary>
        /// An optional rotation origin to perform the rotation around. The origin must be a child of the <see cref="TransformPropertyMutator.Target"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Origin { get; set; }

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
            OnAfterRotationOriginChange();
        }

        /// <summary>
        /// Returns the <see cref="Origin"/> position if a <see cref="Origin"/> is defined.
        /// </summary>
        /// <returns>The origin position.</returns>
        protected virtual Vector3 GetOriginPosition()
        {
            return Origin != null ? Origin.transform.position : Vector3.zero;
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

            originPosition -= Origin.transform.position;
            Target.transform.position += originPosition;
        }

        /// <summary>
        /// Called after <see cref="Origin"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(Origin))]
        protected virtual void OnAfterRotationOriginChange()
        {
            if (Origin == null)
            {
                return;
            }

            if (!Origin.transform.IsChildOf(Target.transform))
            {
                throw new ArgumentException($"The `RotationOrigin` [{Origin.name}] must be a child of the `Target` [{Target.name}] GameObject.");
            }
        }
    }
}