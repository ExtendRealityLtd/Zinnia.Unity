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
    /// The ObjectFollow mirrors the transform properties of another object based on the given FollowModifier.
    /// </summary>
    public class ObjectFollow : SourceTargetProcessor
    {
        [Header("Object Follow Settings")]

        [UnityFlag]
        [Tooltip("The transform properties to follow.")]
        public TransformProperties follow = TransformProperties.Position | TransformProperties.Rotation | TransformProperties.Scale;
        [Tooltip("The follow modifier mechanic to apply.")]
        public FollowModifier followModifier;

        /// <summary>
        /// The ObjectFollowUnityEvent emits an event with the source Transform, target Transform and the sender object.
        /// </summary>
        [Serializable]
        public class ObjectFollowUnityEvent : UnityEvent<Transform, Transform, object>
        {
        };

        [Header("Object Follow Events")]

        /// <summary>
        /// The BeforeProcessed event is emitted before any processing.
        /// </summary>
        public ObjectFollowUnityEvent BeforeProcessed = new ObjectFollowUnityEvent();
        /// <summary>
        /// The AfterProcessed event is emitted after all processing is complete.
        /// </summary>
        public ObjectFollowUnityEvent AfterProcessed = new ObjectFollowUnityEvent();
        /// <summary>
        /// The BeforeTransformUpdated event is emitted before the Transform is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeTransformUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The AfterTransformUpdated event is emitted after the Transform is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterTransformUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The BeforePositionUpdated event is emitted before the Transform's position is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforePositionUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The AfterPositionUpdated event is emitted after the Transform's position is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterPositionUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The BeforeRotationUpdated event is emitted before the Transform's rotation is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeRotationUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The AfterRotationUpdated event is emitted after the Transform's rotation is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterRotationUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The BeforeScaleUpdated event is emitted before the Transform's scale is updated.
        /// </summary>
        public ObjectFollowUnityEvent BeforeScaleUpdated = new ObjectFollowUnityEvent();
        /// <summary>
        /// The AfterScaleUpdated event is emitted after the Transform's scale is updated.
        /// </summary>
        public ObjectFollowUnityEvent AfterScaleUpdated = new ObjectFollowUnityEvent();

        /// <summary>
        /// The Process method executes the relevant process on the given FollowModifier.
        /// </summary>
        public override void Process()
        {
            OnBeforeProcessed();
            if (followModifier.ProcessFirstAndActiveOnly())
            {
                ProcessFirstActiveComponent();
            }
            else
            {
                ProcessAllComponents();
            }
            OnAfterProcessed();
        }

        protected virtual void OnBeforeProcessed()
        {
            BeforeProcessed?.Invoke(null, null, this);
        }

        protected virtual void OnAfterProcessed()
        {
            AfterProcessed?.Invoke(null, null, this);
        }

        protected virtual void OnBeforeTransformUpdated(Transform source, Transform target)
        {
            BeforeTransformUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnAfterTransformUpdated(Transform source, Transform target)
        {
            AfterTransformUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnBeforePositionUpdated(Transform source, Transform target)
        {
            BeforePositionUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnAfterPositionUpdated(Transform source, Transform target)
        {
            AfterPositionUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnBeforeRotationUpdated(Transform source, Transform target)
        {
            BeforeRotationUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnAfterRotationUpdated(Transform source, Transform target)
        {
            AfterRotationUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnBeforeScaleUpdated(Transform source, Transform target)
        {
            BeforeScaleUpdated?.Invoke(source, target, this);
        }

        protected virtual void OnAfterScaleUpdated(Transform source, Transform target)
        {
            AfterScaleUpdated?.Invoke(source, target, this);
        }

        /// <summary>
        /// The ProcessComponent method applies the transformations on the source Transform using the taget Transform.
        /// </summary>
        /// <param name="source">The source Component that is a Transform.</param>
        /// <param name="target">The target Component that is a Transform.</param>
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
        /// The UpdatePosition method executes the specified FollowModifier's UpdatePosition method.
        /// </summary>
        /// <param name="sourceTransform">The source Transform to apply the FollowModifier on.</param>
        /// <param name="targetTransform">The target Transform to apply the FollowModifier with.</param>
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
        /// The UpdateRotation method executes the specified FollowModifier's UpdateRotation method.
        /// </summary>
        /// <param name="sourceTransform">The source Transform to apply the FollowModifier on.</param>
        /// <param name="targetTransform">The target Transform to apply the FollowModifier with.</param>
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
        /// The UpdateScale method executes the specified FollowModifier's UpdateScale method.
        /// </summary>
        /// <param name="sourceTransform">The source Transform to apply the FollowModifier on.</param>
        /// <param name="targetTransform">The target Transform to apply the FollowModifier with.</param>
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