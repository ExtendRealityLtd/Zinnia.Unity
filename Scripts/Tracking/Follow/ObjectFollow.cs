namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using EmptyUnityEvent = UnityEngine.Events.UnityEvent;
    using System;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Enum;
    using VRTK.Core.Extension;
    using VRTK.Core.Process.Component;
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
            /// <summary>
            /// An optional <see cref="Transform"/> to offset the source follow against the target.
            /// </summary>
            public Transform followOffset;

            public EventData Set(EventData source)
            {
                return Set(source.source, source.target, source.followOffset);
            }

            public EventData Set(Transform source, Transform target, Transform followOffset = null)
            {
                this.source = source;
                this.target = target;
                this.followOffset = followOffset;
                return this;
            }

            public void Clear()
            {
                Set(default(Transform), default(Transform), default(Transform));
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
        /// An optional <see cref="Transform"/> to offset the source against the target when following.
        /// </summary>
        [Tooltip("An optional Transform to offset the source against the target when following.")]
        public Transform followOffset;
        /// <summary>
        /// The <see cref="Transform"/> properties to apply the follow offset to.
        /// </summary>
        [UnityFlags]
        [Tooltip("The Transform properties to apply the follow offset to.")]
        public TransformProperties applyOffset = TransformProperties.Position | TransformProperties.Rotation;
        /// <summary>
        /// The <see cref="Transform"/> properties to follow.
        /// </summary>
        [UnityFlags]
        [Tooltip("The Transform properties to follow.")]
        public TransformProperties follow = (TransformProperties)(-1);
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

        /// <summary>
        /// Sets the <see cref="followOffset"/> to the the given <see cref="GameObject.transform"/>.
        /// </summary>
        /// <param name="offset">The new offset.</param>
        public virtual void SetFollowOffset(GameObject offset)
        {
            followOffset = offset?.transform;
        }

        /// <inheritdoc />
        protected override void ProcessComponent(Component source, Component target)
        {
            Transform sourceTransform = source.TryGetTransform();
            Transform targetTransform = target.TryGetTransform();

            if (followModifier != null)
            {
                BeforeTransformUpdated?.Invoke(eventData.Set(sourceTransform, targetTransform, followOffset));

                UpdateScale(sourceTransform, targetTransform, followOffset);
                UpdateRotation(sourceTransform, targetTransform, followOffset);
                UpdatePosition(sourceTransform, targetTransform, followOffset);

                AfterTransformUpdated?.Invoke(eventData.Set(sourceTransform, targetTransform, followOffset));
            }
        }

        protected virtual Transform GetValidOffset(TransformProperties property)
        {
            return (applyOffset.HasFlag(property) ? followOffset : null);
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdatePosition(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        /// <param name="offset">The <see cref="Transform"/> to offset the source against the target when following.</param>
        protected virtual void UpdatePosition(Transform source, Transform target, Transform offset = null)
        {
            TransformProperties flag = TransformProperties.Position;
            offset = GetValidOffset(flag);
            if (follow.HasFlag(flag))
            {
                BeforePositionUpdated?.Invoke(eventData.Set(source, target, offset));
                followModifier.UpdatePosition(source, target, offset);
                AfterPositionUpdated?.Invoke(eventData.Set(source, target, offset));
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateRotation(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        /// <param name="offset">The <see cref="Transform"/> to offset the source against the target when following.</param>
        protected virtual void UpdateRotation(Transform source, Transform target, Transform offset = null)
        {
            TransformProperties flag = TransformProperties.Rotation;
            offset = GetValidOffset(flag);
            if (follow.HasFlag(flag))
            {
                BeforeRotationUpdated?.Invoke(eventData.Set(source, target, offset));
                followModifier.UpdateRotation(source, target, offset);
                AfterRotationUpdated?.Invoke(eventData.Set(source, target, offset));
            }
        }

        /// <summary>
        /// Executes the specified <see cref="FollowModifier.UpdateScale(Transform, Transform)"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Transform"/> to apply the <see cref="FollowModifier"/> on.</param>
        /// <param name="target">The target <see cref="Transform"/> to apply the <see cref="FollowModifier"/> with.</param>
        /// <param name="offset">The <see cref="Transform"/> to offset the source against the target when following.</param>
        protected virtual void UpdateScale(Transform source, Transform target, Transform offset = null)
        {
            TransformProperties flag = TransformProperties.Scale;
            offset = GetValidOffset(flag);
            if (follow.HasFlag(flag))
            {
                BeforeScaleUpdated?.Invoke(eventData.Set(source, target, offset));
                followModifier.UpdateScale(source, target, offset);
                AfterScaleUpdated?.Invoke(eventData.Set(source, target, offset));
            }
        }
    }
}