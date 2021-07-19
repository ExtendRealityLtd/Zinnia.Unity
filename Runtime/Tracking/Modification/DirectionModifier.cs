namespace Zinnia.Tracking.Modification
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberChangeMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;
    using Zinnia.Process;

    /// <summary>
    /// Modifies the given target direction by rotating it to look at a point in space whilst pivoting on another point in space.
    /// </summary>
    public class DirectionModifier : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The target to use for the rotational up.
        /// </summary>
        public enum RotationTargetType
        {
            /// <summary>
            /// Do not use any target for rotational up.
            /// </summary>
            UseNoTarget,
            /// <summary>
            /// Use the <see cref="Pivot"/> for rotational up.
            /// </summary>
            UsePivotAsTarget,
            /// <summary>
            /// Use the <see cref="LookAt"/> for rotational up.
            /// </summary>
            UseLookAtAsTarget
        }

        #region Reference Settings
        /// <summary>
        /// The target to rotate.
        /// </summary>
        [Serialized, Cleared]
        [field: Header("Reference Settings"), DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// The object to look at when affecting rotation.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject LookAt { get; set; }
        /// <summary>
        /// The object to be used as the pivot point for rotation.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject Pivot { get; set; }
        /// <summary>
        /// The object providing a rotational offset for the <see cref="Target"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject TargetOffset { get; set; }
        /// <summary>
        /// The object providing a rotational offset for the <see cref="Pivot"/>.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
        public GameObject PivotOffset { get; set; }
        #endregion

        #region Control Settings
        /// <summary>
        /// The target object to use for setting the world up during the rotation process.
        /// </summary>
        [Serialized]
        [field: Header("Control Settings"), DocumentedByXml]
        public RotationTargetType RotationUpTarget { get; set; } = RotationTargetType.UsePivotAsTarget;
        /// <summary>
        /// Whether to snap the <see cref="Target"/> origin to the <see cref="LookAt"/> origin.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool SnapToLookAt { get; set; } = true;
        /// <summary>
        /// The speed in which the rotation is reset to the original speed when the orientation is reset. The higher the value the slower the speed.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float ResetOrientationSpeed { get; set; } = 0.1f;
        #endregion

        #region Orientation Events
        /// <summary>
        /// Emitted when the orientation is reset.
        /// </summary>
        [Header("Orientation Events"), DocumentedByXml]
        public UnityEvent OrientationReset = new UnityEvent();
        /// <summary>
        /// Emitted when the orientation reset action is canceled.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent OrientationResetCancelled = new UnityEvent();
        #endregion

        /// <summary>
        /// The initial rotation of the <see cref="Target"/>.
        /// </summary>
        protected Quaternion targetInitialRotation;
        /// <summary>
        /// The rotation of the <see cref="Target"/> when released.
        /// </summary>
        protected Quaternion targetReleaseRotation;
        /// <summary>
        /// The initial rotation of the <see cref="Pivot"/>.
        /// </summary>
        protected Quaternion pivotInitialRotation;
        /// <summary>
        /// The rotation of the <see cref="Pivot"/> when released.
        /// </summary>
        protected Quaternion pivotReleaseRotation;
        /// <summary>
        /// The initial rotation offset.
        /// </summary>
        protected Quaternion offsetInitialRotation;
        /// <summary>
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine resetOrientationRoutine;
        /// <summary>
        /// Determines whether the <see cref="LookAt"/> is in front of the <see cref="Pivot"/> within the <see cref="Target"/> local space.
        /// </summary>
        protected bool IsLookAtInFrontOfPivot => Target != null && Pivot != null && LookAt != null ? Target.transform.InverseTransformPoint(LookAt.transform.position).z > Target.transform.InverseTransformPoint(Pivot.transform.position).z : false;

        /// <summary>
        /// Processes the current direction modification.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            SetTargetRotation();
        }

        /// <summary>
        /// Clears the existing pivot.
        /// </summary>
        public virtual void ClearPivot()
        {
            pivotReleaseRotation = Pivot != null ? Pivot.transform.rotation : Quaternion.identity;
        }

        /// <summary>
        /// Saves the existing orientation of the target.
        /// </summary>
        /// <param name="cancelResetOrientation">Determines whether to cancel any existing orientation reset process.</param>
        [RequiresBehaviourState]
        public virtual void SaveOrientation(bool cancelResetOrientation = true)
        {
            targetInitialRotation = Target != null ? Target.transform.rotation : Quaternion.identity;
            pivotInitialRotation = Pivot != null ? Pivot.transform.rotation : Quaternion.identity;
            if (cancelResetOrientation)
            {
                CancelResetOrientation();
            }
        }

        /// <summary>
        /// Resets the orientation of the target to it's initial rotation.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void ResetOrientation()
        {
            pivotReleaseRotation = Pivot != null ? Pivot.transform.rotation : pivotReleaseRotation;

            if (ResetOrientationSpeed < float.MaxValue && ResetOrientationSpeed > 0f)
            {
                resetOrientationRoutine = StartCoroutine(ResetOrientationRoutine());
            }
            else if (ResetOrientationSpeed.ApproxEquals(0f))
            {
                SetOrientationToSaved();
            }
        }

        /// <summary>
        /// Cancels any existing reset orientation process.
        /// </summary>
        public virtual void CancelResetOrientation()
        {
            if (resetOrientationRoutine != null)
            {
                StopCoroutine(resetOrientationRoutine);
            }
            resetOrientationRoutine = null;
            OrientationResetCancelled?.Invoke();
        }

        protected virtual void OnDisable()
        {
            CancelResetOrientation();
        }

        /// <summary>
        /// Resets the target rotation to the initial rotation.
        /// </summary>
        protected virtual void SetOrientationToSaved()
        {
            if (Target == null)
            {
                return;
            }

            Target.transform.rotation = GetActualInitialRotation();
            OrientationReset?.Invoke();
        }

        /// <summary>
        /// Sets the target rotation to look at the specific point in space.
        /// </summary>
        protected virtual void SetTargetRotation()
        {
            if (Target == null || LookAt == null || Pivot == null)
            {
                return;
            }

            Target.transform.rotation = Quaternion.LookRotation(GetRotation(), GetUpwards()) * (PivotOffset != null && PivotOffset.activeInHierarchy ? Quaternion.Inverse(PivotOffset.transform.localRotation) : Quaternion.identity);

            if (!SnapToLookAt)
            {
                Target.transform.rotation *= offsetInitialRotation;
            }
        }

        /// <summary>
        /// Gets the rotation Vector based on the position of the <see cref="LookAt"/> and <see cref="Pivot"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetRotation()
        {
            return IsLookAtInFrontOfPivot ? LookAt.transform.position - Pivot.transform.position : Pivot.transform.position - LookAt.transform.position;
        }

        /// <summary>
        /// Gets the rotational up Vector based on the <see cref="RotationUpTarget"/> value.
        /// </summary>
        /// <returns>The rotational up to use.</returns>
        protected virtual Vector3 GetUpwards()
        {
            switch (RotationUpTarget)
            {
                case RotationTargetType.UseNoTarget:
                    return Vector3.up;
                case RotationTargetType.UsePivotAsTarget:
                    return Pivot != null ? Pivot.transform.up : Vector3.zero;
                case RotationTargetType.UseLookAtAsTarget:
                    return LookAt != null ? LookAt.transform.up : Vector3.zero;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Rotates the target back to the original rotation over a given period of time.
        /// </summary>
        /// <returns>The enumerator.</returns>
        protected virtual IEnumerator ResetOrientationRoutine()
        {
            if (Target == null)
            {
                yield break;
            }

            float elapsedTime = 0f;
            targetReleaseRotation = Target.transform.rotation;

            while (elapsedTime < ResetOrientationSpeed)
            {
                Target.transform.rotation = Quaternion.Lerp(targetReleaseRotation, GetActualInitialRotation(), elapsedTime / ResetOrientationSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetOrientationToSaved();
        }

        /// <summary>
        /// Gets the actual initial rotation of the target based on any changes to the pivot rotation.
        /// </summary>
        /// <returns>The actual initial rotation.</returns>
        protected virtual Quaternion GetActualInitialRotation()
        {
            return targetInitialRotation * (pivotReleaseRotation * Quaternion.Inverse(pivotInitialRotation));
        }

        /// <summary>
        /// Sets the <see cref="offsetInitialRotation"/> value based on the initial rotations and positions of the reference objects.
        /// </summary>
        protected virtual void SetOffsetRotation()
        {
            offsetInitialRotation = (LookAt != null && Pivot != null
                ? Quaternion.Inverse(Quaternion.LookRotation(GetRotation(), GetUpwards())) * Pivot.transform.rotation
                : Quaternion.identity)
                * (TargetOffset != null ? Quaternion.Inverse(TargetOffset.transform.localRotation) : Quaternion.identity);
        }

        /// <summary>
        /// Called after <see cref="LookAt"/> has been changed.
        /// </summary>
        [CalledAfterChangeOf(nameof(LookAt))]
        protected virtual void OnAfterLookAtChange()
        {
            SetOffsetRotation();
        }
    }
}