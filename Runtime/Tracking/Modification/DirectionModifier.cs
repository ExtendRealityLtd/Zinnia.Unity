namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using Malimbe.BehaviourStateRequirementMethod;
    using Zinnia.Process;
    using Zinnia.Extension;

    /// <summary>
    /// Modifies the given target direction by rotating it to look at a point in space whilst pivoting on another point in space.
    /// </summary>
    public class DirectionModifier : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The target to rotate.
        /// </summary>
        [Serialized, Cleared]
        [field: DocumentedByXml]
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
        /// The speed in which the rotation is reset to the original speed when the orientation is reset. The higher the value the slower the speed.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public float ResetOrientationSpeed { get; set; } = 0.1f;
        /// <summary>
        /// Prevent z-axis rotation coming from the <see cref="LookAt"/> target.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public bool PreventLookAtZRotation { get; set; } = true;

        /// <summary>
        /// Emitted when the orientation is reset.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent OrientationReset = new UnityEvent();
        /// <summary>
        /// Emitted when the orientation reset action is cancelled.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent OrientationResetCancelled = new UnityEvent();

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
        /// A reference to the started routine.
        /// </summary>
        protected Coroutine resetOrientationRoutine;

        /// <summary>
        /// Processes the current direction modification.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Process()
        {
            if (PreventLookAtZRotation)
            {
                SetTargetRotationWithZLocking();
            }
            else
            {
                SetTargetRotation();
            }
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

            Target.transform.rotation = Quaternion.LookRotation(LookAt.transform.position - Pivot.transform.position, LookAt.transform.forward);
        }

        /// <summary>
        /// Sets the target rotation to look at the specific point in space whilst applying z locking on the look at target.
        /// </summary>
        protected virtual void SetTargetRotationWithZLocking()
        {
            if (Target == null || LookAt == null || Pivot == null)
            {
                return;
            }

            Vector3 normalizedForward = (LookAt.transform.position - Pivot.transform.position).normalized;
            Quaternion rightLocked = Quaternion.LookRotation(normalizedForward, Vector3.Cross(-Pivot.transform.right, normalizedForward).normalized);
            Quaternion targetRotation = Target.transform.rotation;
            Quaternion rightLockedDelta = Quaternion.Inverse(targetRotation) * rightLocked;
            Quaternion upLocked = Quaternion.LookRotation(normalizedForward, Pivot.transform.forward);
            Quaternion upLockedDelta = Quaternion.Inverse(targetRotation) * upLocked;

            Target.transform.rotation = CalculateLockedAngle(upLockedDelta) < CalculateLockedAngle(rightLockedDelta) ? upLocked : rightLocked;
        }

        /// <summary>
        /// Calculates the locked angle.
        /// </summary>
        /// <param name="lockedDelta">The rotation delta to calculate the angle on.</param>
        /// <returns>The calculated angle.</returns>
        protected virtual float CalculateLockedAngle(Quaternion lockedDelta)
        {
            lockedDelta.ToAngleAxis(out float lockedAngle, out Vector3 _);
            if (lockedAngle > 180f)
            {
                lockedAngle -= 360f;
            }
            return Mathf.Abs(lockedAngle);
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
    }
}