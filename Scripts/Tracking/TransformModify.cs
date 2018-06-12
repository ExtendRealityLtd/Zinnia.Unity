namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Enum;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Extension;

    /// <summary>
    /// Applies a transformation onto a given source <see cref="Transform"/> based on an injected target <see cref="Transform"/>.
    /// </summary>
    public class TransformModify : MonoBehaviour
    {
        /// <summary>
        /// The source <see cref="Transform"/> to apply the transformations to.
        /// </summary>
        [Tooltip("The source Transform to apply the transformations to.")]
        public Transform source;
        /// <summary>
        /// A <see cref="Transform"/> to use as an offset/pivot when applying the transformations.
        /// </summary>
        [Tooltip("A Transform to use as an offset/pivot when applying the transformations.")]
        public Transform offset;
        /// <summary>
        /// Determines which axes to apply on when utilising the offset.
        /// </summary>
        [Tooltip("Determines which axes to apply on when utilising the offset.")]
        public Vector3State applyOffsetOnAxis = new Vector3State(true, true, true);
        /// <summary>
        /// The <see cref="Transform"/> properties to apply the transformations on.
        /// </summary>
        [UnityFlag]
        [Tooltip("The Transform properties to apply the transformations on.")]
        public TransformProperties applyTransformations = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;
        /// <summary>
        /// The amount of time to take when transitioning from the current <see cref="Transform"/> state to the modified <see cref="Transform"/> state.
        /// </summary>
        [Tooltip("The amount of time to take when transitioning from the current Transform state to the modified Transform state.")]
        public float transitionDuration = 0f;

        /// <summary>
        /// Defines the event with the source <see cref="TransformData"/>, target <see cref="TransformData"/> and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class TransformModifyUnityEvent : UnityEvent<TransformData, TransformData, object>
        {
        }

        /// <summary>
        /// Emitted before the transformation process occurs.
        /// </summary>
        public TransformModifyUnityEvent BeforeTransformUpdated = new TransformModifyUnityEvent();
        /// <summary>
        /// Emitted after the transformation process has occured.
        /// </summary>
        public TransformModifyUnityEvent AfterTransformUpdated = new TransformModifyUnityEvent();

        protected Vector3 finalPosition;
        protected Quaternion finalRotation;
        protected Vector3 finalScale;
        protected Coroutine transitionRoutine;

        /// <summary>
        /// Applys the properties of the target <see cref="Transform"/> to the source <see cref="Transform"/>.
        /// </summary>
        /// <param name="target">The target <see cref="TransformData"/> to obtain the transformation properties from.</param>
        /// <param name="initiator">Tne object that initiated the modification.</param>
        public virtual void Modify(TransformData target, object initiator = null)
        {
            if (source != null && target != null && isActiveAndEnabled)
            {
                TransformData sourceData = new TransformData(source);
                OnBeforeTransformUpdated(sourceData, target, initiator);
                SetScale(sourceData, target);
                SetPosition(sourceData, target);
                SetRotation(sourceData, target);
                ProcessTransform(sourceData, target);
            }
        }

        protected virtual void OnDisable()
        {
            CancelTransition();
        }

        protected virtual void OnBeforeTransformUpdated(TransformData sourceData, TransformData targetData, object sender)
        {
            BeforeTransformUpdated?.Invoke(sourceData, targetData, sender);
        }

        protected virtual void OnAfterTransformUpdated(TransformData sourceData, TransformData targetData, object sender)
        {
            AfterTransformUpdated?.Invoke(sourceData, targetData, sender);
        }

        /// <summary>
        /// Applies final transformations to the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> to apply transformations to.</param>
        /// <param name="givenTarget">The target <see cref="TransformData"/> to obtain the transformation properties from.</param>
        protected virtual void ProcessTransform(TransformData givenSource, TransformData givenTarget)
        {
            if (transitionDuration.ApproxEquals(0f))
            {
                givenSource.transform.localScale = finalScale;
                givenSource.transform.position = finalPosition;
                givenSource.transform.rotation = finalRotation;
                OnAfterTransformUpdated(givenSource, givenTarget, this);
            }
            else
            {
                CancelTransition();
                transitionRoutine = StartCoroutine(TransitionTransform(givenSource, givenTarget, givenSource.Position, finalPosition, givenSource.Rotation, finalRotation, givenSource.LocalScale, finalScale));
            }
        }

        /// <summary>
        /// Calculates the final position to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will be used to determine the position transformation that is to be applied.</param>
        protected virtual void SetPosition(TransformData givenSource, TransformData target)
        {
            finalPosition = givenSource.Position;
            if (applyTransformations.HasFlag(TransformProperties.Position))
            {
                finalPosition = CalculatePosition(givenSource, target.Position, target.Rotation, finalScale);
            }
        }

        /// <summary>
        /// Calculates the final rotation to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the rotation transformations applied to.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will be used to determine the rotation transformation that is to be applied.</param>
        protected virtual void SetRotation(TransformData givenSource, TransformData target)
        {
            finalRotation = givenSource.Rotation;
            if (applyTransformations.HasFlag(TransformProperties.Rotation))
            {
                SetPositionWithOriginToSource(givenSource, target);
                finalRotation = target.Rotation;
            }
        }

        /// <summary>
        /// Calculates the final scale to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the scale transformations applied to.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will be used to determine the scale transformation that is to be applied.</param>
        protected virtual void SetScale(TransformData givenSource, TransformData target)
        {
            finalScale = givenSource.LocalScale;
            if (applyTransformations.HasFlag(TransformProperties.Scale))
            {
                finalScale = target.LocalScale;
            }
        }

        /// <summary>
        /// Calculates the final position when the source <see cref="TransformData"/> also has an origin <see cref="TransformData"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will be used to determine the position transformation that is to be applied.</param>
        protected virtual void SetPositionWithOriginToSource(TransformData givenSource, TransformData target)
        {
            if (!applyTransformations.HasFlag(TransformProperties.Position) && offset != null)
            {
                target.transform.position = GetModifiedPosition(givenSource);
                finalPosition = CalculatePosition(givenSource, target.Position, target.Rotation, finalScale);
            }
        }

        /// <summary>
        /// Calculates the position of a <see cref="TransformData"/> based on the target position, rotation and scale.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <param name="targetPosition">The target position value.</param>
        /// <param name="targetRotation">The target rotation value.</param>
        /// <param name="targetScale">The target scale value.</param>
        /// <returns>Calculated final position.</returns>
        protected virtual Vector3 CalculatePosition(TransformData givenSource, Vector3 targetPosition, Quaternion targetRotation, Vector3 targetScale)
        {
            if (offset == null)
            {
                return targetPosition;
            }

            if (!applyTransformations.HasFlag(TransformProperties.Rotation))
            {
                return GetOffsetPosition(givenSource.Position, targetPosition, offset.position);
            }

            Vector3 calculatedOffset = GetOffsetPosition(givenSource.Position, Vector3.zero, offset.position) * -1f;
            Quaternion relativeRotation = Quaternion.Inverse(givenSource.Rotation) * targetRotation;
            Vector3 adjustedOffset = relativeRotation * calculatedOffset;
            Vector3 scaleFactor = new Vector3(targetScale.x / givenSource.LocalScale.x, targetScale.y / givenSource.LocalScale.y, targetScale.z / givenSource.LocalScale.z);
            Vector3 scaledOffset = Vector3.Scale(adjustedOffset, scaleFactor);
            return targetPosition - scaledOffset;
        }

        /// <summary>
        /// Applies the offset to the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="givenSource">The source <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <returns>Modified position taking in to consideration any offset transformations.</returns>
        protected virtual Vector3 GetModifiedPosition(TransformData givenSource)
        {
            return new Vector3(
                GetModifiedPositionValue(applyOffsetOnAxis.xState, offset.position.x, givenSource.Position.x),
                GetModifiedPositionValue(applyOffsetOnAxis.yState, offset.position.y, givenSource.Position.y),
                GetModifiedPositionValue(applyOffsetOnAxis.zState, offset.position.z, givenSource.Position.z)
                );
        }

        /// <summary>
        /// Modifies the position value between either the source or the target position.
        /// </summary>
        /// <param name="useTargetValue">Determines whether to utilize the target value.</param>
        /// <param name="targetValue">The target value to apply.</param>
        /// <param name="sourceValue">The source value to apply.</param>
        /// <returns>Modified position which is either the given `targetValue` or given `sourceValue`.</returns>
        protected virtual float GetModifiedPositionValue(bool useTargetValue, float targetValue, float sourceValue)
        {
            return (useTargetValue ? targetValue : sourceValue);
        }

        /// <summary>
        /// Calculates the position with the appropriate offset. 
        /// </summary>
        /// <param name="sourcePosition">The source current position.</param>
        /// <param name="targetPosition">The target current position.</param>
        /// <param name="offsetPosition">The offset current position.</param>
        /// <returns>Position with the applied offset.</returns>
        protected virtual Vector3 GetOffsetPosition(Vector3 sourcePosition, Vector3 targetPosition, Vector3 offsetPosition)
        {
            float xPosition = GetOffsetCoordinate(applyOffsetOnAxis.xState, sourcePosition, targetPosition, offsetPosition, 0);
            float yPosition = GetOffsetCoordinate(applyOffsetOnAxis.yState, sourcePosition, targetPosition, offsetPosition, 1);
            float zPosition = GetOffsetCoordinate(applyOffsetOnAxis.zState, sourcePosition, targetPosition, offsetPosition, 2);
            return new Vector3(xPosition, yPosition, zPosition);
        }

        /// <summary>
        /// Calculates the offset position coordinate.
        /// </summary>
        /// <param name="applyOffset">Determines whether to apply the offset position to the calculation.</param>
        /// <param name="sourcePosition">The source position to use within the calculation.</param>
        /// <param name="targetPosition">The target position to use within the calculation.</param>
        /// <param name="offsetPosition">The offset position to use within the calculation.</param>
        /// <param name="coordinate">The coordinate of the <see cref="Vector3"/> to access.</param>
        /// <returns>Coordinate position with the applied offset.</returns>
        protected virtual float GetOffsetCoordinate(bool applyOffset, Vector3 sourcePosition, Vector3 targetPosition, Vector3 offsetPosition, int coordinate)
        {
            return (applyOffset ? targetPosition[coordinate] - (offsetPosition[coordinate] - sourcePosition[coordinate]) : targetPosition[coordinate]);
        }

        /// <summary>
        /// Cancels the transition of the transformation.
        /// </summary>
        protected virtual void CancelTransition()
        {
            if (transitionRoutine != null)
            {
                StopCoroutine(transitionRoutine);
            }
        }

        /// <summary>
        /// Applies the relevant transformation to the affected <see cref="TransformData"/> over a given duration.
        /// </summary>
        /// <param name="affectTransform">The <see cref="TransformData"/> to apply the transformations to.</param>
        /// <param name="givenTarget">The target <see cref="TransformData"/> to obtain the transformation properties from.</param>
        /// <param name="startPosition">The initial position of the <see cref="Transform"/>.</param>
        /// <param name="destinationPosition">The final position for the <see cref="Transform"/>.</param>
        /// <param name="startRotation">The initial rotation of the <see cref="Transform"/>.</param>
        /// <param name="destinationRotation">The final rotation of the <see cref="Transform"/>.</param>
        /// <param name="startScale">The initial scale of the <see cref="Transform"/>.</param>
        /// <param name="destinationScale">The final scale of the <see cref="Transform"/>.</param>
        /// <returns>Coroutine enumerator.</returns>
        protected virtual IEnumerator TransitionTransform(TransformData affectTransform, TransformData givenTarget, Vector3 startPosition, Vector3 destinationPosition, Quaternion startRotation, Quaternion destinationRotation, Vector3 startScale, Vector3 destinationScale)
        {
            float elapsedTime = 0f;
            WaitForEndOfFrame delayInstruction = new WaitForEndOfFrame();
            while (elapsedTime < transitionDuration)
            {
                float lerpFrame = (elapsedTime / transitionDuration);
                affectTransform.transform.localScale = Vector3.Lerp(startScale, destinationScale, lerpFrame);
                affectTransform.transform.position = Vector3.Lerp(startPosition, destinationPosition, lerpFrame);
                affectTransform.transform.rotation = Quaternion.Lerp(startRotation, destinationRotation, lerpFrame);
                elapsedTime += Time.deltaTime;
                yield return delayInstruction;
            }

            affectTransform.transform.localScale = destinationScale;
            affectTransform.transform.position = destinationPosition;
            affectTransform.transform.rotation = destinationRotation;
            OnAfterTransformUpdated(affectTransform, givenTarget, this);
        }
    }
}