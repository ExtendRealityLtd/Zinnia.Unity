namespace VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.Events;
    using EmptyUnityEvent = UnityEngine.Events.UnityEvent;
    using System;
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
            /// The source utilize within the <see cref="FollowModifier"/>.
            /// </summary>
            public GameObject source;
            /// <summary>
            /// The target to apply the <see cref="FollowModifier"/> on.
            /// </summary>            
            public GameObject target;
            /// <summary>
            /// The optional offset the target follow against the source.
            /// </summary>
            public GameObject followOffset;

            public EventData Set(EventData source)
            {
                return Set(source.source, source.target, source.followOffset);
            }

            public EventData Set(GameObject source, GameObject target, GameObject followOffset = null)
            {
                this.source = source;
                this.target = target;
                this.followOffset = followOffset;
                return this;
            }

            public void Clear()
            {
                Set(default(GameObject), default(GameObject), default(GameObject));
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
        /// The way in which to apply modifications.
        /// </summary>
        public enum ModificationType
        {
            /// <summary>
            /// The Source data is applied onto the Target
            /// </summary>
            ModifyTargetUsingSource,
            /// <summary>
            /// The Target data is applied onto the Source.
            /// </summary>
            ModifySourceUsingTarget
        }
        /// <summary>
        /// The process logic for the target.
        /// </summary>
        public enum ProcessTarget
        {
            /// <summary>
            /// Process all targets.
            /// </summary>
            All,
            /// <summary>
            /// Only process the first active target.
            /// </summary>
            FirstActive
        }

        /// <summary>
        /// An offset on the target against the source when following.
        /// </summary>
        [Tooltip("An offset on the target against the source when following.")]
        public GameObject followOffset;
        /// <summary>
        /// The <see cref="FollowModifier"/> to apply.
        /// </summary>
        [Tooltip("The FollowModifier to apply.")]
        public FollowModifier followModifier;
        /// <summary>
        /// Determines which component is to be applied to the other component.
        /// </summary>
        [Tooltip("Determines which component is to be applied to the other component.")]
        public ModificationType modificationType;
        /// <summary>
        /// The mechanism of how to process the targets.
        /// </summary>
        [Tooltip("The mechanism of how to process the targets.")]
        public ProcessTarget processTarget = ProcessTarget.All;

        /// <summary>
        /// Emitted before any processing.
        /// </summary>
        public EmptyUnityEvent Preprocessed = new EmptyUnityEvent();
        /// <summary>
        /// Emitted after all processing is complete.
        /// </summary>
        public EmptyUnityEvent Processed = new EmptyUnityEvent();

        protected EventData eventData = new EventData();

        /// <inheritdoc />
        public override void Process()
        {
            if (!isActiveAndEnabled || followModifier == null)
            {
                return;
            }

            Preprocessed?.Invoke();
            switch (processTarget)
            {
                case ProcessTarget.All:
                    ProcessAllComponents();
                    break;
                case ProcessTarget.FirstActive:
                    ProcessFirstActiveComponent();
                    break;
            }
            Processed?.Invoke();
        }

        /// <summary>
        /// Sets the <see cref="followOffset"/> to the the given offset.
        /// </summary>
        /// <param name="offset">The new offset.</param>
        public virtual void SetFollowOffset(GameObject offset)
        {
            followOffset = offset;
        }

        /// <summary>
        /// Clears the existing follow offset.
        /// </summary>
        public virtual void ClearFollowOffset()
        {
            followOffset = null;
        }

        /// <inheritdoc />
        protected override void ProcessComponent(Component source, Component target)
        {
            GameObject sourceGameObject = (modificationType == ModificationType.ModifyTargetUsingSource ? source.TryGetGameObject() : target.TryGetGameObject());
            GameObject targetGameObject = (modificationType == ModificationType.ModifyTargetUsingSource ? target.TryGetGameObject() : source.TryGetGameObject());

            followModifier?.Modify(sourceGameObject, targetGameObject, followOffset);
        }
    }
}