namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Enum;
    using VRTK.Core.Extension;
    using VRTK.Core.Process;
    using VRTK.Core.Tracking.Follow.Modifier;

    /// <summary>
    /// Mirrors the <see cref="Transform"/> properties of another <see cref="Transform"/> based on the given <see cref="FollowModifier"/>.
    /// </summary>
    public class ObjectFollow : SourceTargetProcessor
    {
        [Header("Object Follow Settings")]

        /// <summary>
        /// The <see cref="Transform"/> properties to follow.
        /// </summary>
        [UnityFlag]
        [Tooltip("The Transform properties to follow.")]
        public TransformProperties follow = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;
        /// <summary>
        /// The <see cref="FollowModifier"/> to apply.
        /// </summary>
        [Tooltip("The FollowModifier to apply.")]
        public FollowModifier followModifier;

        /// <summary>
        /// Defines the event with the source <see cref="Transform"/>, target <see cref="Transform"/> and sender <see cref="object"/>.
        /// </summary>
        [Serializable]
        public class ObjectFollowUnityEvent : UnityEvent<Transform, Transform, object>
        {
        }

        [Header("Object Follow Events")]

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        public ObjectFollowUnityEvent BeforeProcessed = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        public ObjectFollowUnityEvent AfterProcessed = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted before the Transform is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeTransformUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted after the Transform is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterTransformUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted before the Transform's position is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforePositionUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted after the Transform's position is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterPositionUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted before the Transform's rotation is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeRotationUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted after the Transform's rotation is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterRotationUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted before the Transform's scale is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeScaleUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// Emitted after the Transform's scale is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterScaleUpdated = new ObjectFollowUnityEvent();


        /// <inheritdoc />
        public override void Process()
        {
            if (isActiveAndEnabled)
            {
                OnBeforeProcessed();
                if (followModifier != null && followModifier.ProcessFirstAndActiveOnly())
                {
                    ProcessFirstActiveComponent();
                }
                else
                {
                    ProcessAllComponents();
                }
                OnAfterProcessed();
            }
        }

        protected virtual void OnBeforeProcessed()
        {
            if (isActiveAndEnabled)
            {
                BeforeProcessed?.Invoke(null, null, this);
            }
        }

        protected virtual void OnAfterProcessed()
        {
            if (isActiveAndEnabled)
            {
                AfterProcessed?.Invoke(null, null, this);
            }
        }

        protected virtual void OnBeforeTransformUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                BeforeTransformUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnAfterTransformUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                AfterTransformUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnBeforePositionUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                BeforePositionUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnAfterPositionUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                AfterPositionUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnBeforeRotationUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                BeforeRotationUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnAfterRotationUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                AfterRotationUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnBeforeScaleUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                BeforeScaleUpdated?.Invoke(source, target, this);
            }
        }

        protected virtual void OnAfterScaleUpdated(Transform source, Transform target)
        {
            if (isActiveAndEnabled)
            {
                AfterScaleUpdated?.Invoke(source, target, this);
            }
        }

        /// <inheritdoc />
        protected override void ProcessComponent(Component source, Component target)
        {
            Transform sourceTransform = source.TryGetTransform();
            Transform targetTransform = target.TryGetTransform();

            if (followModifier != null)
            {
                OnBeforeTransformUpdated(sourceTransform, targetTransform);

                UpdateScale(sourceTransform, targetTransform);
                UpdateRotation(sourceTransform, targetTransform);
                UpdatePosition(sourceTransform, targetTransform);

                OnAfterTransformUpdated(sourceTransform, targetTransform);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdatePosition(Transform, Transform)"/>.
        /// </summary>
        /// <param name="sourceTransform">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="targetTransform">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdatePosition(Transform sourceTransform, Transform targetTransform)
        {
            if (follow.HasFlag(TransformProperties.Position))
            {
                OnBeforePositionUpdated(sourceTransform, targetTransform);
                followModifier.UpdatePosition(sourceTransform, targetTransform);
                OnAfterPositionUpdated(sourceTransform, targetTransform);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateRotation(Transform, Transform)"/>.
        /// </summary>
        /// <param name="sourceTransform">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="targetTransform">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdateRotation(Transform sourceTransform, Transform targetTransform)
        {
            if (follow.HasFlag(TransformProperties.Rotation))
            {
                OnBeforeRotationUpdated(sourceTransform, targetTransform);
                followModifier.UpdateRotation(sourceTransform, targetTransform);
                OnAfterRotationUpdated(sourceTransform, targetTransform);
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateScale(Transform, Transform)"/>.
        /// </summary>
        /// <param name="sourceTransform">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="targetTransform">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdateScale(Transform sourceTransform, Transform targetTransform)
        {
            if (follow.HasFlag(TransformProperties.Scale))
            {
                OnBeforeScaleUpdated(sourceTransform, targetTransform);
                followModifier.UpdateScale(sourceTransform, targetTransform);
                OnAfterScaleUpdated(sourceTransform, targetTransform);
            }
        }
    }
}