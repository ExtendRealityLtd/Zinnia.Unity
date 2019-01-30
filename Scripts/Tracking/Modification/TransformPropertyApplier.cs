namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using Zinnia.Extension;
    using Zinnia.Data.Attribute;
    using Zinnia.Data.Enum;
    using Zinnia.Data.Type;

    /// <summary>
    /// Applies the transform properties from a given source <see cref="Transform"/> onto the given target <see cref="Transform"/>.
    /// </summary>
    public class TransformPropertyApplier : MonoBehaviour
    {
        /// <summary>
        /// Holds data about a <see cref="TransformPropertyApplier"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The source <see cref="TransformData"/> to obtain the transformation properties from.
            /// </summary>
            public TransformData source;
            /// <summary>
            /// The target <see cref="TransformData"/> to apply transformations to.
            /// </summary>
            public TransformData target;

            public EventData Set(EventData source)
            {
                return Set(source.source, source.target);
            }

            public EventData Set(TransformData source, TransformData target)
            {
                this.source = source;
                this.target = target;
                return this;
            }

            public void Clear()
            {
                Set(default, default);
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

        /// <summary>
        /// The source to obtain the transformation properties from.
        /// </summary>
        [Tooltip("The source to obtain the transformation properties from.")]
        public TransformData source;
        /// <summary>
        /// The target to apply the transformations to.
        /// </summary>
        [Tooltip("The target to apply the transformations to.")]
        public GameObject target;
        /// <summary>
        /// The offset/pivot when applying the transformations.
        /// </summary>
        [Tooltip("The offset/pivot when applying the transformations.")]
        public GameObject offset;
        /// <summary>
        /// Determines which axes to apply on when utilizing the position offset.
        /// </summary>
        [Tooltip("Determines which axes to apply on when utilizing the position offset.")]
        public Vector3State applyPositionOffsetOnAxis = new Vector3State(true, true, true);
        /// <summary>
        /// Determines which axes to apply on when utilizing the rotation offset.
        /// </summary>
        [Tooltip("Determines which axes to apply on when utilizing the rotation offset.")]
        public Vector3State applyRotationOffsetOnAxis = new Vector3State(true, true, true);
        /// <summary>
        /// The <see cref="Transform"/> properties to apply the transformations on.
        /// </summary>
        [Tooltip("The Transform properties to apply the transformations on."), UnityFlags]
        public TransformProperties applyTransformations = (TransformProperties)(-1);
        /// <summary>
        /// The amount of time to take when transitioning from the current <see cref="Transform"/> state to the modified <see cref="Transform"/> state.
        /// </summary>
        [Tooltip("The amount of time to take when transitioning from the current Transform state to the modified Transform state.")]
        public float transitionDuration = 0f;

        /// <summary>
        /// Emitted before the transformation process occurs.
        /// </summary>
        public UnityEvent BeforeTransformUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the transformation process has occured.
        /// </summary>
        public UnityEvent AfterTransformUpdated = new UnityEvent();

        /// <summary>
        /// The final calculated position to apply to the target.
        /// </summary>
        protected Vector3 finalPosition;
        /// <summary>
        /// The final calculated rotation to apply to the target.
        /// </summary>
        protected Quaternion finalRotation;
        /// <summary>
        /// The final calculated scale to apply to the target.
        /// </summary>
        protected Vector3 finalScale;
        /// <summary>
        /// The routine for managing the transition of the transform.
        /// </summary>
        protected Coroutine transitionRoutine;
        /// <summary>
        /// The cached event data payload.
        /// </summary>
        protected EventData eventData = new EventData();

        /// <summary>
        /// Sets the <see cref="source"/> parameter.
        /// </summary>
        /// <param name="source">The new source value.</param>
        public virtual void SetSource(GameObject source)
        {
            if (source != null)
            {
                SetSource(new TransformData(source));
            }
        }

        /// <summary>
        /// Sets the <see cref="source"/> parameter.
        /// </summary>
        /// <param name="source">The new source value.</param>
        public virtual void SetSource(TransformData source)
        {
            this.source = source;
        }

        /// <summary>
        /// Clears the <see cref="source"/> parameter.
        /// </summary>
        public virtual void ClearSource()
        {
            source = null;
        }

        /// <summary>
        /// Sets the <see cref="target"/> parameter.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(GameObject target)
        {
            this.target = target;
        }

        /// <summary>
        /// Sets the <see cref="target"/> parameter.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(TransformData target)
        {
            SetTarget(target.TryGetGameObject());
        }

        /// <summary>
        /// Clears the <see cref="target"/> parameter.
        /// </summary>
        public virtual void ClearTarget()
        {
            target = null;
        }

        /// <summary>
        /// Sets the <see cref="offset"/> parameter.
        /// </summary>
        /// <param name="offset">The new offset value.</param>
        public virtual void SetOffset(GameObject offset)
        {
            this.offset = offset;
        }

        /// <summary>
        /// Sets the <see cref="offset"/> parameter.
        /// </summary>
        /// <param name="offset">The new offset value.</param>
        public virtual void SetOffset(TransformData offset)
        {
            SetOffset(offset.TryGetGameObject());
        }

        /// <summary>
        /// Clears the <see cref="offset"/> parameter.
        /// </summary>
        public virtual void ClearOffset()
        {
            offset = null;
        }

        /// <summary>
        /// Applies the properties of the <see cref="source"/> parameter to the target.
        /// </summary>
        public virtual void Apply()
        {
            if (!isActiveAndEnabled || target == null || source?.transform == null)
            {
                return;
            }

            TransformData targetData = new TransformData(target);
            BeforeTransformUpdated?.Invoke(eventData.Set(source, targetData));
            SetScale(source, targetData);
            SetPosition(source, targetData);
            SetRotation(source, targetData);
            ProcessTransform(source, targetData);
        }

        protected virtual void OnDisable()
        {
            CancelTransition();
        }

        /// <summary>
        /// Applies final transformations to the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> to obtain the transformation properties from.</param>
        /// <param name="target">The target <see cref="TransformData"/> to apply transformations to.</param>
        protected virtual void ProcessTransform(TransformData source, TransformData target)
        {
            if (transitionDuration.ApproxEquals(0f))
            {
                target.transform.SetGlobalScale(finalScale);
                target.transform.position = finalPosition;
                target.transform.rotation = finalRotation;
                AfterTransformUpdated?.Invoke(eventData.Set(source, target));
            }
            else
            {
                CancelTransition();
                transitionRoutine = StartCoroutine(TransitionTransform(source, target, target.Position, finalPosition, target.Rotation, finalRotation, target.Scale, finalScale));
            }
        }

        /// <summary>
        /// Calculates the final position to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the position transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the position transformations applied to.</param>
        protected virtual void SetPosition(TransformData source, TransformData target)
        {
            finalPosition = target.Position;
            if (applyTransformations.HasFlag(TransformProperties.Position))
            {
                finalPosition = CalculatePosition(target, source.Position, source.Rotation, finalScale);
            }
        }

        /// <summary>
        /// Calculates the final rotation to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the rotation transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the rotation transformations applied to.</param>
        protected virtual void SetRotation(TransformData source, TransformData target)
        {
            finalRotation = target.Rotation;
            if (applyTransformations.HasFlag(TransformProperties.Rotation))
            {
                SetPositionWithOriginToSource(source, target);
                finalRotation = CalculateRotationWithOffset(source.Rotation, target.Rotation, offset);
            }
        }

        /// <summary>
        /// Calculates the final scale to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the scale transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the scale transformations applied to.</param>
        protected virtual void SetScale(TransformData source, TransformData target)
        {
            finalScale = target.Scale;
            if (applyTransformations.HasFlag(TransformProperties.Scale))
            {
                finalScale = source.Scale;
            }
        }

        /// <summary>
        /// Calculates the final position when the target <see cref="TransformData"/> also has an origin <see cref="TransformData"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the position transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the position transformations applied to.</param>
        protected virtual void SetPositionWithOriginToSource(TransformData source, TransformData target)
        {
            if (!applyTransformations.HasFlag(TransformProperties.Position) && offset != null)
            {
                Vector3 updatedPosition = GetModifiedPosition(target);
                finalPosition = CalculatePosition(target, updatedPosition, source.Rotation, finalScale);
            }
        }

        /// <summary>
        /// Calculates the position of a <see cref="TransformData"/> based on the source position, rotation and scale.
        /// </summary>
        /// <param name="target">The target <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <param name="sourcePosition">The source position value.</param>
        /// <param name="sourceRotation">The source rotation value.</param>
        /// <param name="sourceScale">The source scale value.</param>
        /// <returns>Calculated final position.</returns>
        protected virtual Vector3 CalculatePosition(TransformData target, Vector3 sourcePosition, Quaternion sourceRotation, Vector3 sourceScale)
        {
            if (offset == null)
            {
                return sourcePosition;
            }

            if (!applyTransformations.HasFlag(TransformProperties.Rotation))
            {
                return GetOffsetPosition(sourcePosition, target.Position, offset.transform.position);
            }

            Vector3 calculatedOffset = GetOffsetPosition(Vector3.zero, target.Position, offset.transform.position) * -1f;
            Quaternion relativeRotation = Quaternion.Inverse(target.Rotation) * sourceRotation;
            Vector3 adjustedOffset = relativeRotation * calculatedOffset;
            Vector3 scaleFactor = sourceScale.Divide(target.Scale);
            Vector3 scaledOffset = Vector3.Scale(adjustedOffset, scaleFactor);
            return sourcePosition - scaledOffset;
        }

        /// <summary>
        /// Applies the offset to the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="target">The target <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <returns>Modified position taking in to consideration any offset transformations.</returns>
        protected virtual Vector3 GetModifiedPosition(TransformData target)
        {
            return new Vector3(
                GetModifiedPositionValue(applyPositionOffsetOnAxis.xState, offset.transform.position.x, target.Position.x),
                GetModifiedPositionValue(applyPositionOffsetOnAxis.yState, offset.transform.position.y, target.Position.y),
                GetModifiedPositionValue(applyPositionOffsetOnAxis.zState, offset.transform.position.z, target.Position.z)
                );
        }

        /// <summary>
        /// Modifies the position value between either the source or the target position.
        /// </summary>
        /// <param name="useSourceValue">Determines whether to utilize the source value.</param>
        /// <param name="sourceValue">The source value to apply.</param>
        /// <param name="targetValue">The target value to apply.</param>
        /// <returns>Modified position which is either the given `targetValue` or given `sourceValue`.</returns>
        protected virtual float GetModifiedPositionValue(bool useSourceValue, float sourceValue, float targetValue)
        {
            return (useSourceValue ? sourceValue : targetValue);
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
            float xPosition = GetOffsetCoordinate(applyPositionOffsetOnAxis.xState, sourcePosition, targetPosition, offsetPosition, 0);
            float yPosition = GetOffsetCoordinate(applyPositionOffsetOnAxis.yState, sourcePosition, targetPosition, offsetPosition, 1);
            float zPosition = GetOffsetCoordinate(applyPositionOffsetOnAxis.zState, sourcePosition, targetPosition, offsetPosition, 2);
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
            return (applyOffset ? sourcePosition[coordinate] - (offsetPosition[coordinate] - targetPosition[coordinate]) : sourcePosition[coordinate]);
        }

        /// <summary>
        /// Calculates the rotation with an optional offset.
        /// </summary>
        /// <param name="sourceRotation">The source rotation to apply to the target.</param>
        /// <param name="targetRotation">The current original target rotation before any changes to it.</param>
        /// <param name="offset">The offset rotation to apply.</param>
        /// <returns>The calculated rotation based on the offset.</returns>
        protected virtual Quaternion CalculateRotationWithOffset(Quaternion sourceRotation, Quaternion targetRotation, GameObject offset)
        {
            if (offset == null)
            {
                return sourceRotation;
            }

            Quaternion offsetAdjustedRotation = sourceRotation * (targetRotation * Quaternion.Inverse(offset.transform.rotation));
            float xRotation = applyRotationOffsetOnAxis.xState ? offsetAdjustedRotation.eulerAngles.x : sourceRotation.eulerAngles.x;
            float yRotation = applyRotationOffsetOnAxis.yState ? offsetAdjustedRotation.eulerAngles.y : sourceRotation.eulerAngles.y;
            float zRotation = applyRotationOffsetOnAxis.zState ? offsetAdjustedRotation.eulerAngles.z : sourceRotation.eulerAngles.z;

            return Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
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
        /// <param name="source">The target <see cref="TransformData"/> to obtain the transformation properties from.</param>
        /// <param name="target">The <see cref="TransformData"/> to apply the transformations to.</param>
        /// <param name="startPosition">The initial position of the <see cref="Transform"/>.</param>
        /// <param name="destinationPosition">The final position for the <see cref="Transform"/>.</param>
        /// <param name="startRotation">The initial rotation of the <see cref="Transform"/>.</param>
        /// <param name="destinationRotation">The final rotation of the <see cref="Transform"/>.</param>
        /// <param name="startScale">The initial scale of the <see cref="Transform"/>.</param>
        /// <param name="destinationScale">The final scale of the <see cref="Transform"/>.</param>
        /// <returns>Coroutine enumerator.</returns>
        protected virtual IEnumerator TransitionTransform(TransformData source, TransformData target, Vector3 startPosition, Vector3 destinationPosition, Quaternion startRotation, Quaternion destinationRotation, Vector3 startScale, Vector3 destinationScale)
        {
            float elapsedTime = 0f;
            WaitForEndOfFrame delayInstruction = new WaitForEndOfFrame();
            while (elapsedTime < transitionDuration)
            {
                float lerpFrame = (elapsedTime / transitionDuration);
                target.transform.SetGlobalScale(Vector3.Lerp(startScale, destinationScale, lerpFrame));
                target.transform.position = Vector3.Lerp(startPosition, destinationPosition, lerpFrame);
                target.transform.rotation = Quaternion.Lerp(startRotation, destinationRotation, lerpFrame);
                elapsedTime += Time.deltaTime;
                yield return delayInstruction;
            }

            target.transform.SetGlobalScale(destinationScale);
            target.transform.position = destinationPosition;
            target.transform.rotation = destinationRotation;
            AfterTransformUpdated?.Invoke(eventData.Set(source, target));
        }
    }
}