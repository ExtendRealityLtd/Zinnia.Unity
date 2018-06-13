namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using EmptyUnityEvent = UnityEngine.Events.UnityEvent;
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
        /// <summary>
        /// Holds data about a <see cref="ObjectFollow"/> event.
        /// </summary>
        [Serializable]
        public class EventData
        {
            /// <summary>
            /// The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.
            /// </summary>
            public Transform source;
            /// <summary>
            /// The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.
            /// </summary>
            public Transform target;

            public EventData Set(Transform source, Transform target)
            {
                this.source = source;
                this.target = target;
                return this;
            }
        }

        /// <summary>
        /// Defines the event with the <see cref="EventData"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<EventData>
        {
        }

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

        [Header("Object Follow Events")]

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        public EmptyUnityEvent BeforeProcessed = new EmptyUnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        public EmptyUnityEvent AfterProcessed = new EmptyUnityEvent();
        /// <summary>
        /// Emitted before the Transform is updated.
        /// </summary>
        public UnityEvent BeforeTransformUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the Transform is updated.
        /// </summary>
        public UnityEvent AfterTransformUpdated = new UnityEvent();
        /// <summary>
        /// Emitted before the Transform's position is updated.
        /// </summary>
        public UnityEvent BeforePositionUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the Transform's position is updated.
        /// </summary>
        public UnityEvent AfterPositionUpdated = new UnityEvent();
        /// <summary>
        /// Emitted before the Transform's rotation is updated.
        /// </summary>
        public UnityEvent BeforeRotationUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the Transform's rotation is updated.
        /// </summary>
        public UnityEvent AfterRotationUpdated = new UnityEvent();
        /// <summary>
        /// Emitted before the Transform's scale is updated.
        /// </summary>
        public UnityEvent BeforeScaleUpdated = new UnityEvent();
        /// <summary>
        /// Emitted after the Transform's scale is updated.
        /// </summary>
        public UnityEvent AfterScaleUpdated = new UnityEvent();

        protected EventData eventData = new EventData();

        /// <inheritdoc />
        public override void Process()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            BeforeProcessed?.Invoke();
            if (followModifier != null)
            {
                switch (followModifier.ProcessType)
                {
                    case FollowModifier.ProcessTarget.All:
                        ProcessAllComponents();
                        break;
                    case FollowModifier.ProcessTarget.FirstActive:
                        ProcessFirstActiveComponent();
                        break;
                }
            }
            AfterProcessed?.Invoke();
        }

        /// <inheritdoc />
        protected override void ProcessComponent(Component source, Component target)
        {
            Transform sourceTransform = source.TryGetTransform();
            Transform targetTransform = target.TryGetTransform();

            if (followModifier != null)
            {
                BeforeTransformUpdated?.Invoke(eventData.Set(sourceTransform, targetTransform));

                UpdateScale(sourceTransform, targetTransform);
                UpdateRotation(sourceTransform, targetTransform);
                UpdatePosition(sourceTransform, targetTransform);

                AfterTransformUpdated?.Invoke(eventData.Set(sourceTransform, targetTransform));
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdatePosition(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdatePosition(Transform source, Transform target)
        {
            if (follow.HasFlag(TransformProperties.Position))
            {
                BeforePositionUpdated?.Invoke(eventData.Set(source, target));
                followModifier.UpdatePosition(source, target);
                AfterPositionUpdated?.Invoke(eventData.Set(source, target));
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateRotation(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdateRotation(Transform source, Transform target)
        {
            if (follow.HasFlag(TransformProperties.Rotation))
            {
                BeforeRotationUpdated?.Invoke(eventData.Set(source, target));
                followModifier.UpdateRotation(source, target);
                AfterRotationUpdated?.Invoke(eventData.Set(source, target));
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateScale(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        protected virtual void UpdateScale(Transform source, Transform target)
        {
            if (follow.HasFlag(TransformProperties.Scale))
            {
                BeforeScaleUpdated?.Invoke(eventData.Set(source, target));
                followModifier.UpdateScale(source, target);
                AfterScaleUpdated?.Invoke(eventData.Set(source, target));
            }
        }
    }
}