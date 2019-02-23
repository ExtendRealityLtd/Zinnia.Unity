namespace Zinnia.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.MemberClearanceMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
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
            [DocumentedByXml]
            public TransformData source;
            /// <summary>
            /// The target <see cref="TransformData"/> to apply transformations to.
            /// </summary>
            [DocumentedByXml]
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
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public TransformData Source { get; set; }
        /// <summary>
        /// The target to apply the transformations to.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject Target { get; set; }
        /// <summary>
        /// The offset/pivot when applying the transformations.
        /// </summary>
        [Serialized, Validated, Cleared]
        [field: DocumentedByXml]
        public GameObject Offset { get; set; }
        /// <summary>
        /// Determines which axes to apply on when utilizing the position offset.
        /// </summary>
        [DocumentedByXml]
        public Vector3State applyPositionOffsetOnAxis = new Vector3State(true, true, true);
        /// <summary>
        /// Determines which axes to apply on when utilizing the rotation offset.
        /// </summary>
        [DocumentedByXml]
        public Vector3State applyRotationOffsetOnAxis = new Vector3State(true, true, true);
        /// <summary>
        /// The <see cref="Transform"/> properties to apply the transformations on.
        /// </summary>
        [UnityFlags, DocumentedByXml]
        public TransformProperties applyTransformations = (TransformProperties)(-1);
        /// <summary>
        /// The amount of time to take when transitioning from the current <see cref="Transform"/> state to the modified <see cref="Transform"/> state.
        /// </summary>
        [DocumentedByXml]
        public float transitionDuration;
        /// <summary>
        /// The threshold the current <see cref="Transform"/> properties can be within of the destination properties to be considered equal.
        /// </summary>
        [DocumentedByXml]
        public float transitionDestinationThreshold = 0.01f;

        /// <summary>
        /// Emitted before the transformation process occurs.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent BeforeTransformUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the transformation process has occured.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent AfterTransformUpdated = new UnityEvent();

        /// <summary>
        /// The routine for managing the transition of the transform.
        /// </summary>
        protected Coroutine transitionRoutine;
        /// <summary>
        /// The cached event data payload.
        /// </summary>
        protected EventData eventData = new EventData();

        /// <summary>
        /// Sets the <see cref="Source"/> parameter.
        /// </summary>
        /// <param name="source">The new source value.</param>
        public virtual void SetSource(GameObject source)
        {
            if (source != null)
            {
                Source = new TransformData(source);
            }
        }

        /// <summary>
        /// Sets the <see cref="Target"/> parameter.
        /// </summary>
        /// <param name="target">The new target value.</param>
        public virtual void SetTarget(TransformData target)
        {
            Target = target.TryGetGameObject();
        }

        /// <summary>
        /// Sets the <see cref="Offset"/> parameter.
        /// </summary>
        /// <param name="offset">The new offset value.</param>
        public virtual void SetOffset(TransformData offset)
        {
            Offset = offset.TryGetGameObject();
        }

        /// <summary>
        /// Applies the properties of the <see cref="Source"/> parameter to the target.
        /// </summary>
        [RequiresBehaviourState]
        public virtual void Apply()
        {
            if (Target == null || Source?.transform == null)
            {
                return;
            }

            TransformData targetData = new TransformData(Target);
            BeforeTransformUpdated?.Invoke(eventData.Set(Source, targetData));
            Vector3 finalScale = CalculateScale(Source, targetData);
            Quaternion finalRotation = CalculateRotation(Source, targetData);
            Vector3 finalPosition = CalculatePosition(Source, targetData, finalScale, finalRotation);
            ProcessTransform(Source, targetData, finalScale, finalRotation, finalPosition);
        }

        protected virtual void OnDisable()
        {
            CancelTransition();
        }

        /// <summary>
        /// Calculates the final scale to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the scale transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the scale transformations applied to.</param>
        /// <returns>The calculated scale.</returns>
        protected virtual Vector3 CalculateScale(TransformData source, TransformData target)
        {
            if ((applyTransformations & TransformProperties.Scale) == 0)
            {
                return target.Scale;
            }

            return source.Scale;
        }

        /// <summary>
        /// Calculates the final rotation to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the rotation transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the rotation transformations applied to.</param>
        /// <returns>The calculated rotation.</returns>
        protected virtual Quaternion CalculateRotation(TransformData source, TransformData target)
        {
            if ((applyTransformations & TransformProperties.Rotation) == 0)
            {
                return target.Rotation;
            }

            if (Offset == null)
            {
                return source.Rotation;
            }

            Quaternion rotationAdjustedByOffset = source.Rotation * (target.Rotation * Quaternion.Inverse(Offset.transform.rotation));
            Vector3 axisAdjustedRotation = GetOffsetData(applyRotationOffsetOnAxis, rotationAdjustedByOffset.eulerAngles, source.Rotation.eulerAngles);
            return Quaternion.Euler(axisAdjustedRotation);
        }

        /// <summary>
        /// Calculates the final position to apply based on the given target <see cref="Transform"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> that will be used to determine the position transformation that is to be applied.</param>
        /// <param name="target">The target <see cref="TransformData"/> that will have the position transformations applied to.</param>
        /// <param name="currentScale">The current scale to apply to the target.</param>
        /// <param name="currentRotation">The current rotation to apply to the target.</param>
        /// <returns>The calculated position.</returns>
        protected virtual Vector3 CalculatePosition(TransformData source, TransformData target, Vector3 currentScale, Quaternion currentRotation)
        {
            Vector3 currentPosition = source.Position;
            if ((applyTransformations & TransformProperties.Position) == 0)
            {
                if (Offset == null)
                {
                    return target.Position;
                }

                currentPosition = GetOffsetData(applyPositionOffsetOnAxis, Offset.transform.position, target.Position);
            }

            if (Offset == null)
            {
                return currentPosition;
            }

            if ((applyTransformations & TransformProperties.Rotation) == 0)
            {
                return CalculatePositionWithOffset(currentPosition, target.Position, Offset.transform.position);
            }

            Vector3 calculatedOffset = CalculatePositionWithOffset(Vector3.zero, target.Position, Offset.transform.position) * -1f;
            Quaternion relativeRotation = Quaternion.Inverse(target.Rotation) * currentRotation;
            Vector3 adjustedOffset = relativeRotation * calculatedOffset;
            Vector3 scaleFactor = currentScale.Divide(target.Scale);
            Vector3 scaledOffset = Vector3.Scale(adjustedOffset, scaleFactor);
            return currentPosition - scaledOffset;
        }

        /// <summary>
        /// Calculates the position with the appropriate offset.
        /// </summary>
        /// <param name="sourcePosition">The source current position.</param>
        /// <param name="targetPosition">The target current position.</param>
        /// <param name="offsetPosition">The offset current position.</param>
        /// <returns>Position with the applied offset.</returns>
        protected virtual Vector3 CalculatePositionWithOffset(Vector3 sourcePosition, Vector3 targetPosition, Vector3 offsetPosition)
        {
            Vector3 positionAdjustedByOffset = sourcePosition - (offsetPosition - targetPosition);
            return GetOffsetData(applyPositionOffsetOnAxis, positionAdjustedByOffset, sourcePosition);
        }

        /// <summary>
        /// Applies final transformations to the given <see cref="TransformData"/>.
        /// </summary>
        /// <param name="source">The source <see cref="TransformData"/> to obtain the transformation properties from.</param>
        /// <param name="target">The target <see cref="TransformData"/> to apply transformations to.</param>
        /// <param name="currentScale">The current scale to apply to the target.</param>
        /// <param name="currentRotation">The current rotation to apply to the target.</param>
        /// <param name="currentPosition">The current position to apply to the target.</param>
        protected virtual void ProcessTransform(TransformData source, TransformData target, Vector3 currentScale, Quaternion currentRotation, Vector3 currentPosition)
        {
            if (transitionDuration.ApproxEquals(0f))
            {
                target.transform.SetGlobalScale(currentScale);
                target.transform.rotation = currentRotation;
                target.transform.position = currentPosition;
                AfterTransformUpdated?.Invoke(eventData.Set(source, target));
            }
            else
            {
                CancelTransition();
                transitionRoutine = StartCoroutine(TransitionTransform(source, target, target.Scale, currentScale, target.Rotation, currentRotation, target.Position, currentPosition));
            }
        }

        /// <summary>
        /// Creates the return data based on whether to use the offset values or the original value depending on the given state.
        /// </summary>
        /// <param name="applyStates">The state to determine which value to use on each coordinate.</param>
        /// <param name="offsetValue">The offset values.</param>
        /// <param name="originalValue">The original values.</param>
        /// <returns>The combined result containing the relevant offset and original values based on the given state.</returns>
        protected virtual Vector3 GetOffsetData(Vector3State applyStates, Vector3 offsetValue, Vector3 originalValue)
        {
            return new Vector3(
                applyStates.xState ? offsetValue.x : originalValue.x,
                applyStates.yState ? offsetValue.y : originalValue.y,
                applyStates.zState ? offsetValue.z : originalValue.z
                );
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
        /// <param name="startScale">The initial scale of the <see cref="Transform"/>.</param>
        /// <param name="destinationScale">The final scale of the <see cref="Transform"/>.</param>
        /// <param name="startRotation">The initial rotation of the <see cref="Transform"/>.</param>
        /// <param name="destinationRotation">The final rotation of the <see cref="Transform"/>.</param>
        /// <param name="startPosition">The initial position of the <see cref="Transform"/>.</param>
        /// <param name="destinationPosition">The final position for the <see cref="Transform"/>.</param>
        /// <returns>Coroutine enumerator.</returns>
        protected virtual IEnumerator TransitionTransform(TransformData source, TransformData target, Vector3 startScale, Vector3 destinationScale, Quaternion startRotation, Quaternion destinationRotation, Vector3 startPosition, Vector3 destinationPosition)
        {
            float elapsedTime = 0f;
            WaitForEndOfFrame delayInstruction = new WaitForEndOfFrame();
            while (elapsedTime < transitionDuration)
            {
                float lerpFrame = elapsedTime / transitionDuration;
                target.transform.SetGlobalScale(Vector3.Lerp(startScale, destinationScale, lerpFrame));
                target.transform.position = Vector3.Lerp(startPosition, destinationPosition, lerpFrame);
                target.transform.rotation = Quaternion.Lerp(startRotation, destinationRotation, lerpFrame);
                elapsedTime += ArePropertiesEqual(startPosition, destinationPosition, startRotation, destinationRotation, startScale, destinationScale) ? transitionDuration : Time.deltaTime;
                yield return delayInstruction;
            }

            target.transform.SetGlobalScale(destinationScale);
            target.transform.position = destinationPosition;
            target.transform.rotation = destinationRotation;
            AfterTransformUpdated?.Invoke(eventData.Set(source, target));
        }

        /// <summary>
        /// Determines whether the start properties equal the destination properties.
        /// </summary>
        /// <param name="startPosition">The initial position of the <see cref="Transform"/>.</param>
        /// <param name="destinationPosition">The final position for the <see cref="Transform"/>.</param>
        /// <param name="startRotation">The initial rotation of the <see cref="Transform"/>.</param>
        /// <param name="destinationRotation">The final rotation of the <see cref="Transform"/>.</param>
        /// <param name="startScale">The initial scale of the <see cref="Transform"/>.</param>
        /// <param name="destinationScale">The final scale of the <see cref="Transform"/>.</param>
        /// <returns>Whether the start properties equal the destination properties.</returns>
        protected virtual bool ArePropertiesEqual(Vector3 startPosition, Vector3 destinationPosition, Quaternion startRotation, Quaternion destinationRotation, Vector3 startScale, Vector3 destinationScale)
        {
            return startPosition.ApproxEquals(destinationPosition, transitionDestinationThreshold)
                && startRotation.eulerAngles.ApproxEquals(destinationRotation.eulerAngles, transitionDestinationThreshold)
                && startScale.ApproxEquals(destinationScale, transitionDestinationThreshold);
        }
    }
}