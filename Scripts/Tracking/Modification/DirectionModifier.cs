namespace VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections;
    using VRTK.Core.Process;

    /// <summary>
    /// Modifies the given target direction by rotating it to look at a point in space whilst pivoting on another point in space.
    /// </summary>
    public class DirectionModifier : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The target to rotate.
        /// </summary>
        [Tooltip("The target to rotate.")]
        public GameObject target;
        /// <summary>
        /// The object to look at when affecting rotation.
        /// </summary>
        [Tooltip("The object to look at when affecting rotation.")]
        public GameObject lookAt;
        /// <summary>
        /// The object to be used as the pivot point for rotation.
        /// </summary>
        [Tooltip("The object to be used as the pivot point for rotation.")]
        public GameObject pivot;
        /// <summary>
        /// The speed in which the rotation is reset to the original speed when the orientation is reset. The higher the value the slower the speed.
        /// </summary>
        [Tooltip("The speed in which the rotation is reset to the original speed when the orientation is reset. The higher the value the slower the speed.")]
        public float resetOrientationSpeed = 0.1f;
        /// <summary>
        /// Prevent z-axis rotation coming from the <see cref="lookAt"/> target.
        /// </summary>
        [Tooltip("Prevent z-axis rotation coming from the lookAt target.")]
        public bool preventLookAtZRotation = true;

        /// <summary>
        /// Emitted when the orientation is reset.
        /// </summary>
        public UnityEvent OrientationReset = new UnityEvent();
        /// <summary>
        /// Emitted when the orientation reset action is cancelled.
        /// </summary>
        public UnityEvent OrientationResetCancelled = new UnityEvent();

        protected Quaternion targetInitialRotation;
        protected Quaternion targetReleaseRotation;
        protected Quaternion pivotInitialRotation;
        protected Quaternion pivotReleaseRotation;
        protected Coroutine resetOrientationRoutine;

        /// <summary>
        /// Processes the current direction modification.
        /// </summary>
        public virtual void Process()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (preventLookAtZRotation)
            {
                SetTargetRotationWithZLocking();
            }
            else
            {
                SetTargetRotation();
            }
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="target">The new target.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Clears the existing target.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Sets the lookAt.
        /// </summary>
        /// <param name="lookAt">The new lookAt.</param>
        public virtual void SetLookAt(GameObject lookAt)
        {
            this.lookAt = lookAt;
        }

        /// <summary>
        /// Clears the existing lookAt.
        /// </summary>
        public virtual void ClearLookAt()
        {
            lookAt = null;
        }

        /// <summary>
        /// Sets the pivot.
        /// </summary>
        /// <param name="pivot">The new pivot.</param>
        public virtual void SetPivot(GameObject pivot)
        {
            this.pivot = pivot;
        }

        /// <summary>
        /// Clears the existing pivot.
        /// </summary>
        public virtual void ClearPivot()
        {
            pivotReleaseRotation = (pivot != null ? pivot.transform.rotation : Quaternion.identity);
            pivot = null;
        }

        /// <summary>
        /// Saves the existing orientation of the target.
        /// </summary>
        /// <param name="cancelResetOrientation">Determines whether to cancel any existing orientation reset process.</param>
        public virtual void SaveOrientation(bool cancelResetOrientation = true)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            targetInitialRotation = (target != null ? target.transform.rotation : Quaternion.identity);
            pivotInitialRotation = (pivot != null ? pivot.transform.rotation : Quaternion.identity);
            if (cancelResetOrientation)
            {
                CancelResetOrientation();
            }
        }

        /// <summary>
        /// Resets the orientation of the target to it's initial rotation.
        /// </summary>
        public virtual void ResetOrientation()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            pivotReleaseRotation = (pivot != null ? pivot.transform.rotation : pivotReleaseRotation);

            if (resetOrientationSpeed < float.MaxValue && resetOrientationSpeed > 0f)
            {
                resetOrientationRoutine = StartCoroutine(ResetOrientationRoutine());
            }
            else if (resetOrientationSpeed == 0f)
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
            if (target == null)
            {
                return;
            }

            target.transform.rotation = GetActualInitialRotation();
            OrientationReset?.Invoke();
        }

        /// <summary>
        /// Sets the target rotation to look at the specific point in space.
        /// </summary>
        protected virtual void SetTargetRotation()
        {
            if (target == null || lookAt == null || pivot == null)
            {
                return;
            }

            target.transform.rotation = Quaternion.LookRotation(lookAt.transform.position - pivot.transform.position, lookAt.transform.forward);
        }

        /// <summary>
        /// Sets the target rotation to look at the specific point in space whilst applying z locking on the look at target.
        /// </summary>
        protected virtual void SetTargetRotationWithZLocking()
        {
            if (target == null || lookAt == null || pivot == null)
            {
                return;
            }

            Vector3 normalizedForward = (lookAt.transform.position - pivot.transform.position).normalized;
            Quaternion rightLocked = Quaternion.LookRotation(normalizedForward, Vector3.Cross(-pivot.transform.right, normalizedForward).normalized);
            Quaternion rightLockedDelta = Quaternion.Inverse(target.transform.rotation) * rightLocked;
            Quaternion upLocked = Quaternion.LookRotation(normalizedForward, pivot.transform.forward);
            Quaternion upLockedDelta = Quaternion.Inverse(target.transform.rotation) * upLocked;

            target.transform.rotation = (CalculateLockedAngle(upLockedDelta) < CalculateLockedAngle(rightLockedDelta) ? upLocked : rightLocked);
        }

        /// <summary>
        /// Calculates the locked angle.
        /// </summary>
        /// <param name="lockedDelta">The rotation delta to calculate the angle on.</param>
        /// <returns>The calculated angle.</returns>
        protected virtual float CalculateLockedAngle(Quaternion lockedDelta)
        {
            float lockedAngle;
            Vector3 lockedAxis;
            lockedDelta.ToAngleAxis(out lockedAngle, out lockedAxis);
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
            if (target == null)
            {
                yield break;
            }

            float elapsedTime = 0f;
            targetReleaseRotation = target.transform.rotation;

            while (elapsedTime < resetOrientationSpeed)
            {
                target.transform.rotation = Quaternion.Lerp(targetReleaseRotation, GetActualInitialRotation(), (elapsedTime / resetOrientationSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetOrientationToSaved();
        }

        /// <summary>
        /// Gets the actial initial rotation of the target based on any changes to the pivot rotation.
        /// </summary>
        /// <returns>The actual initial rotation.</returns>
        protected virtual Quaternion GetActualInitialRotation()
        {
            return targetInitialRotation * (pivotReleaseRotation * Quaternion.Inverse(pivotInitialRotation));
        }
    }
}